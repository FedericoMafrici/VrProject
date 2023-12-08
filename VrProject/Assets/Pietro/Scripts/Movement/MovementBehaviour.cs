using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovementBehaviour {

    protected NavMeshAgent _agent;
    protected NPCMover _npcMover;
    protected Transform _toMoveTransform;
    public bool HasValidParameters;

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

    public abstract void Move(Transform objectToMove);

    protected virtual void updateBehaviour() {
        MovingState nextState = MovingState.PATROL;
        Collider target = null;

        if (targetInRange(_npcMover.getVeryCloseRadius(), ref target)) {
            nextState = MovingState.VERY_CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getCloseRadius(), ref target)) {
            nextState = MovingState.CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getInRangeRadius(), ref target)) {
            nextState = _npcMover.getState();
        }

        if (nextState != _npcMover.getState()) {
            updateState(nextState, target);
        }
    }

    private bool targetInRange(float range, ref Collider target) {
        bool targetFound = false;
        Collider[] intersectedTargets = Physics.OverlapSphere(_toMoveTransform.position, range, 1 << 6);

        if (intersectedTargets.Length > 0) {
            int i = 0;
            Collider tmpTarget;

            //iterate over all intersected objects
            while (!targetFound && i < intersectedTargets.Length) {
                tmpTarget = intersectedTargets[i];
                Targettable tmpTargettable = tmpTarget.transform.GetComponent<Targettable>();

                //check if intersected object is a Targettable,
                //if it is check if NPCMover is interested in its type and if target's follower count has not reached its max
                if (tmpTargettable != null && _npcMover.InterestsSet.Contains(tmpTargettable.getType()) && tmpTargettable.trySubscribe(_npcMover)) { //TODO: visibility check through raycast
                    targetFound = true;
                    target = tmpTarget;

                }

                i++;
            }
        }
        return targetFound;
    }

    private void updateState(MovingState nextState, Collider target) {
        MovingState currentState = _npcMover.getState();

        if (nextState == MovingState.PATROL && currentState != MovingState.PATROL) {
            _npcMover.setNextBehaviour(new PatrolBehaviour(_toMoveTransform, _npcMover.getPatrolArea(), _npcMover.getDelayBounds()));
            //Debug.Log(_toMoveTransform.name + ": state changed to Patrol");

        } else if (nextState != MovingState.PATROL && currentState == MovingState.PATROL) {
            _npcMover.setNextBehaviour(new TargetBehaviour(_toMoveTransform, target.transform));
            //Debug.Log(_toMoveTransform.name + ": State changed to Follow");
        }
        _npcMover.setState(nextState);
    }
}
