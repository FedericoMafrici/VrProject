using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleMovementBehaviour : MovementBehaviour {

    private Transform _target;

    public SampleMovementBehaviour(Transform toMoveTransform, Transform target) : base(toMoveTransform) {
        _target = target;
        ValidateParameters();
    }

    public override void Move() {
        UpdateTarget();
    }

    private void ValidateParameters() {
        if (_target == null) {
            Debug.LogWarning("No target set for " + _toMoveTransform.name + ", SampleMovementBehaviour not activated");
            HasValidParameters = false;
        }
    }

    private void UpdateTarget() {
        if (HasValidParameters) {
            _agent.destination = _target.position;
        }
    }

}
