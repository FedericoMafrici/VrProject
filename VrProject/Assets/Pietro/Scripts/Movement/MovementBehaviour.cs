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
        validateParameters();
    }

    public virtual void Delete() {

    }

    private void validateParameters() {
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

    protected void updateBehaviour() {
        MovingState nextState = MovingState.PATROL;
        Targettable target = null;

        if (targetInRange(_npcMover.getVeryCloseRadius(), ref target)) {
            nextState = MovingState.VERY_CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getCloseRadius(), ref target)) {
            nextState = MovingState.CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getInRangeRadius(), ref target)) {
            nextState = _npcMover.getState();
        }

        manageStateUpdate(nextState, target);
        _npcMover.setState(nextState);
    }

    protected bool targetInRange(float range, ref Targettable target) {
        bool targetFound = false;
        Collider[] intersectedTargets = Physics.OverlapSphere(_toMoveTransform.position, range, 1 << 6);

        if (intersectedTargets.Length > 0) {
            int i = 0;
            Collider tmpTarget;

            //iterate over all intersected objects
            while (!targetFound && i < intersectedTargets.Length) {
                tmpTarget = intersectedTargets[i];
                Targettable tmpTargettable = tmpTarget.transform.GetComponent<Targettable>();

               
                if (wantsToFollowTarget(tmpTargettable)) { //TODO: visibility check through raycast
                    targetFound = true;
                    target = tmpTargettable;

                }

                i++;
            }
        }
        return targetFound;
    }

    protected virtual bool wantsToFollowTarget(Targettable targettable) {
        bool result = false;

        //check if intersected object is a Targettable,
        //if it is check if NPCMover is interested in its type and if target's follower count has not reached its max
        if (targettable != null && _npcMover.InterestsSet.Contains(targettable.getType()) && targettable.canSubscribe(_npcMover)) { //TODO: visibility check through raycast
            result = true;
        }

        return result;
    }

    protected virtual void manageStateUpdate(MovingState nextState, Targettable newTarget) {
        if (nextState != _npcMover.getState()) {
            MovingState currentState = _npcMover.getState();

            if (nextState == MovingState.PATROL) {
                setNewPatrolBehaviour();
                //Debug.Log(_toMoveTransform.name + ": state changed to Patrol");


            } else if (nextState == MovingState.IN_TARGET_RANGE || nextState == MovingState.CLOSE_TO_TARGET || nextState == MovingState.VERY_CLOSE_TO_TARGET) {
                setNewTargetBehaviour(newTarget.transform);
                //Debug.Log(_toMoveTransform.name + ": State changed to Follow");
            }
        }
    }

    protected void setNewPatrolBehaviour() {
        _npcMover.setBehaviour(new PatrolBehaviour(_toMoveTransform, _npcMover.getPatrolArea(), _npcMover.getDelayBounds()));
    }

    protected void setNewTargetBehaviour(Transform target) {
        _npcMover.setBehaviour(new TargetBehaviour(_toMoveTransform, target));
    }

    }
