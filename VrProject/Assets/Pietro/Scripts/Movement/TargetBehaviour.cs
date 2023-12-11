using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class TargetBehaviour : MovementBehaviour {
    //TODO: manage destruction of the target

    private Transform _target;
    private Targettable _targettable;
    private const float _rotationDotThreshold = 0.99f; //if dot product between agent direction and target direction is above this threshold then agent is looking at it
    public TargetBehaviour(Transform toMoveTransform, Transform target) : base(toMoveTransform) {
        _target = target;
        _targettable = target.GetComponent<Targettable>();
        validateParameters();
        if(HasValidParameters) {
            _targettable.subscribe(_npcMover);
        }
    }

    public override void Delete() {
        deleteTarget();
    }

    public override void Move(Transform objectToMove) {
        updateMovement();
        updateBehaviour();
    }

    private void validateParameters() {
        if (_target == null) {
            Debug.LogWarning("No target set for " + _toMoveTransform.name + ", TargetBehaviour not activated");
            HasValidParameters = false;
        }

        if (_targettable == null) {
            Debug.LogWarning("Target Transform reference for " + _toMoveTransform.name + " has no targettable component, TargetBehaviour not activated");
            HasValidParameters = false;
        }
    }

    private void updateMovement() {
        if (HasValidParameters && _target != null) {
            if (_npcMover.getState() == MovingState.VERY_CLOSE_TO_TARGET) {
                _agent.destination = _toMoveTransform.position;
                //rotate towards target
                Vector3 targetDirection = _target.position - _toMoveTransform.position;

                float rotationStep = 10.0f * Time.deltaTime; 
                Vector3 newDirection = Vector3.RotateTowards(_toMoveTransform.forward, targetDirection, rotationStep, 0.0f);
                newDirection.y = _toMoveTransform.forward.y;
                Debug.DrawRay(_toMoveTransform.position, newDirection, Color.red);
                _toMoveTransform.rotation = Quaternion.LookRotation(newDirection);

                //if agent is very close to target and is looking at it destory the target
                if (_npcMover.getState() == MovingState.VERY_CLOSE_TO_TARGET && Vector3.Dot(_toMoveTransform.forward, targetDirection) >= _rotationDotThreshold) {
                    //_npcMover.DestroyTarget(_target);
                }

            } else {
                _agent.destination = _target.position;
            }
        }
    }

    protected override void updateBehaviour() {
        MovingState nextState = MovingState.PATROL;
        Targettable target = null;

        if (targetInRange(_npcMover.getVeryCloseRadius(), ref target)) {
            nextState = MovingState.VERY_CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getCloseRadius(), ref target)) {
            nextState = MovingState.CLOSE_TO_TARGET;
        } else if (targetInRange(_npcMover.getInRangeRadius(), ref target)) {
            nextState = _npcMover.getState();
        }

        if (nextState != _npcMover.getState() || target.transform != _target) {
            updateState(nextState, target);
        }
    }

    protected override bool targetInRange(float range, ref Targettable target) {
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
                if (tmpTargettable != null && _npcMover.InterestsSet.Contains(tmpTargettable.getType()) && wantsToFollowTarget(tmpTargettable.transform) && tmpTargettable.canSubscribe(_npcMover)) { //TODO: visibility check through raycast
                    targetFound = true;
                    target = tmpTargettable;

                }

                i++;
            }
        }
        return targetFound;
    }

    private bool wantsToFollowTarget(Transform target) {
        //might be used to implement a priority system, for now do not follow a target which is different from the one you're currently following
        if (_target == target) {
            return true;
        }
        return false;
    }

    private void deleteTarget() {
        if (_targettable != null) {
            _targettable.unsubscribe(_npcMover);
            _targettable = null;
        }

        if (_target != null) {
            _target = null;
        }
    }
}
