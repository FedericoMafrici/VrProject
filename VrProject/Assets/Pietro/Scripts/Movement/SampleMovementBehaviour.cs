using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleMovementBehaviour : MovementBehaviour {

    private Transform _target;

    public SampleMovementBehaviour(Transform toMoveTransform, Transform target) : base(toMoveTransform) {
        _target = target;
        validateParameters();
    }

    public override void Move(Transform objectToMove) {
        updateTarget();
    }

    private void validateParameters() {
        if (_target == null) {
            Debug.LogWarning("No target set for " + _toMoveTransform.name + ", SampleMovementBehaviour not activated");
            HasValidParameters = false;
        }
    }

    private void updateTarget() {
        if (HasValidParameters) {
            _agent.destination = _target.position;
        }
    }

}
