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

    protected override bool wantsToFollowTarget(Targettable targettable) {
        //first check if base class method allows to follow target
        bool result = base.wantsToFollowTarget(targettable);

        //if base class method returns true do additional checks
        if (result)
            result = prefersTarget(targettable.transform);

        return result;
    }

    protected override void manageStateUpdate(MovingState nextState, Targettable newTarget) {
        // if target a target was found only call the base class method check if new target is different from current one
        // otherwise just call the base class method

        if (newTarget != null) {
            if (newTarget.transform != _target) {
                base.manageStateUpdate(nextState, newTarget);
            }
        } else {
            base.manageStateUpdate(nextState, newTarget);
        }
    }

    private bool prefersTarget(Transform target) {

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
