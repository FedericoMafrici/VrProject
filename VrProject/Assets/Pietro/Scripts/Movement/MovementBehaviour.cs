using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class MovementBehaviour {

    protected NavMeshAgent _agent;
    protected Transform _toMoveTransform;
    public bool HasValidParameters;

    public MovementBehaviour(Transform toMoveTransform) {
        _agent = toMoveTransform.GetComponent<NavMeshAgent>();
        _toMoveTransform = toMoveTransform;
        HasValidParameters = true;
        validateParameters();
    }

    private void validateParameters() {
            if (_agent == null) {
                Debug.LogWarning("No NavMeshAgent detected for " + _toMoveTransform.name + ", disabling movement");
                HasValidParameters = false;
            }
    }

    public abstract void Move(Transform objectToMove);
}
