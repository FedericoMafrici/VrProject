using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovementBehaviour {
    //TODO: system to allow a MonoBehaviour script to create a MovementBehaviour

    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected NPCMover _npcMover;
    [SerializeField] protected Transform _toMoveTransform;
    [HideInInspector] public bool HasValidParameters;

    public MovementBehaviour(Transform toMoveTransform) {
        _agent = toMoveTransform.GetComponent<NavMeshAgent>();
        _npcMover = toMoveTransform.GetComponent<NPCMover>();
        _toMoveTransform = toMoveTransform;
        HasValidParameters = true;
        ValidateParameters();
    }

    public virtual void Delete() {

    }

    private void ValidateParameters() {
        if (_agent == null) {
            Debug.LogWarning("No NavMeshAgent detected for " + _toMoveTransform.name + ", MovementBehaviour not activated");
            HasValidParameters = false;
        }

        if (_npcMover == null) {
            Debug.LogWarning("No NPCMover detected for " + _toMoveTransform.name + ", MovementBehaviour not activated");
            HasValidParameters = false;
        }
    }

    public abstract void Move();

    protected void UpdateBehaviour() {
        MovingState nextState = MovingState.PATROL;
        Targettable target = null;

        //Debug.Log(_npcMover.name + ": position = " + _toMoveTransform.position);


        //nested calls
        //this way if there's no Targettable inside the larger radius TargetInRange() will not be called more than once
        if (TargetInRange(_npcMover.GetInRangeRadius(), ref target)) {
            nextState = _npcMover.GetState();
            if (TargetInRange(_npcMover.GetCloseRadius(), ref target)) {
                nextState = MovingState.CLOSE_TO_TARGET;
                if (TargetInRange(_npcMover.GetVeryCloseRadius(), ref target)) {
                    nextState = MovingState.VERY_CLOSE_TO_TARGET;
                }
            }
        }

        /*
        if (TargetInRange(_npcMover.GetVeryCloseRadius(), ref target)) {
            nextState = MovingState.VERY_CLOSE_TO_TARGET;
        } else if (TargetInRange(_npcMover.GetCloseRadius(), ref target)) {
            nextState = MovingState.CLOSE_TO_TARGET;
        } else if (TargetInRange(_npcMover.GetInRangeRadius(), ref target)) {
            nextState = _npcMover.GetState();
        }*/

        ManageStateUpdate(nextState, target);
        _npcMover.SetState(nextState);
    }

    protected bool TargetInRange(float range, ref Targettable target) {
        bool targetFound = false;
        Collider[] intersectedTargets = Physics.OverlapSphere(_toMoveTransform.position, range, LayerMask.GetMask("AnimalTargets", "Item"));

        if (intersectedTargets.Length > 0) {
            int i = 0;
            Collider tmpTarget;

            //iterate over all intersected objects
            while (!targetFound && i < intersectedTargets.Length) {
                tmpTarget = intersectedTargets[i];
                Targettable tmpTargettable = tmpTarget.transform.GetComponent<Targettable>();


                if (WantsToFollowTarget(tmpTargettable)) { //TODO: visibility check through raycast
                    targetFound = true;
                    target = tmpTargettable;

                }

                i++;
            }
        }

        return targetFound;
    }

    protected virtual bool WantsToFollowTarget(Targettable targettable) {
        bool result = false;

        //check if intersected object is a Targettable,
        //if it is check if it can be followed
        if (targettable != null && CanBeFollowed(targettable)) { //TODO: visibility check through raycast
            result = true;
        }

        return result;
    }

    private bool CanBeFollowed(Targettable targettable) {
        bool result = false;

        //if Targettable is also an item ensure it is reachable
        bool inaccessibleItem = false;
        Item item = targettable.GetComponent<Item>();
        inaccessibleItem = (item != null && (item.isDeposited || item.isCollected));
        AnimalFood animalFood = targettable.GetComponent<AnimalFood>();
        FoodEater foodEater = _npcMover.GetComponent<FoodEater>();

        if (animalFood != null && foodEater != null) {
            inaccessibleItem = inaccessibleItem || !foodEater.FoodInterestsAnimal(animalFood);
        }

        //inaccessibleItem = inaccessibleItem || (animalFood != null && animalFood.IsPlanted()) || (foodEater != null && foodEater);

        //return true if:
        //NPCMover is interested in Targettable type
        //Targettable still allows for subscribers
        //If targettable item it is accessible
        return !inaccessibleItem && _npcMover.InterestsSet.Contains(targettable.GetTargetType()) && targettable.CanSubscribe(_npcMover);
    }

    protected virtual void ManageStateUpdate(MovingState nextState, Targettable newTarget) {
        if (nextState != _npcMover.GetState()) {
            MovingState currentState = _npcMover.GetState();

            if (nextState == MovingState.PATROL) {
                SetNewPatrolBehaviour();
                //Debug.Log(_toMoveTransform.name + ": state changed to Patrol");


            } else if (nextState == MovingState.IN_TARGET_RANGE || nextState == MovingState.CLOSE_TO_TARGET || nextState == MovingState.VERY_CLOSE_TO_TARGET) {
                SetNewTargetBehaviour(newTarget.transform);
                //Debug.Log(_toMoveTransform.name + ": State changed to Follow");
            }
        }
    }

    protected void SetNewPatrolBehaviour() {
        _npcMover.SetBehaviour(new PatrolBehaviour(_toMoveTransform, _npcMover.GetPatrolArea(), _npcMover.GetDelayBounds()));
    }

    protected void SetNewTargetBehaviour(Transform target) {
        _npcMover.SetBehaviour(new InterestBehaviour(_toMoveTransform, target, _npcMover.GetFoodEaterReference()));
    }

    virtual public void Stop() {
        _agent.isStopped = true;
    }

    virtual public void Start() {
        _agent.isStopped = false;
    }

    public float GetCurVelocity() {
        return _agent.velocity.magnitude;
    }

    public float GetMovementSpeed() {
        return _agent.speed;
    }

    protected void SetAgentMovingSpeed(float newSpeed) {
        _agent.speed = newSpeed;
        _npcMover.UpdateAnimationSpeed();
    }

    }
