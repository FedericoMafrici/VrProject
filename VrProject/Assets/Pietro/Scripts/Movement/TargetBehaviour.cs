using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TargetBehaviour : MovementBehaviour {

    private Transform _target;
    private Targettable _targettable;

    public TargetBehaviour(Transform toMoveTransform, Transform target) : base(toMoveTransform) {
        _target = target;
        _targettable = target.GetComponent<Targettable>();
        validateParameters();
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
                //TODO: keep rotating towards target
                Vector3 targetDirection = _target.position - _toMoveTransform.position;

                float rotationStep = 10.0f * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(_toMoveTransform.forward, targetDirection, rotationStep, 0.0f);
                newDirection.y = _toMoveTransform.forward.y;
                Debug.DrawRay(_toMoveTransform.position, newDirection, Color.red);
                _toMoveTransform.rotation = Quaternion.LookRotation(newDirection);

            } else {
                _agent.destination = _target.position;
            }
        }
    }

    private void deleteTarget() {
        if (_target != null) {
            _target = null;
        }

        if (_targettable != null) {
            _targettable.unsubscribe(_npcMover);
            _targettable = null;
        }
    }
}
