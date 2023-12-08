using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MovementBehaviour {

    private Vector3 _target;
    private BoxCollider _patrolArea;
    private Vector2 _delayBounds;
    private bool _targetReached;

    public PatrolBehaviour(Transform toMoveTransform, BoxCollider patrolArea, Vector2 delayBounds) : base(toMoveTransform) {
        _delayBounds = delayBounds;
        _patrolArea = patrolArea;
        _targetReached = true;
        validateParameters();
    }

    public override void Move(Transform objectToMove) {   
       //check if target has been reached
       if (objectToMove.position.x == _target.x && objectToMove.position.z == _target.z) {
            _targetReached = true;
        }
    }

    public IEnumerator updateTarget() {
        //generate random trget inside the patrol area
        if (HasValidParameters) {

            while (true) {
                //wait for target to be reached before generating a new one
                while (!_targetReached) {
                    yield return null;
                }

                float toWait = Random.Range(_delayBounds.x, _delayBounds.y); //random delay befor generating new target
                Debug.Log(_toMoveTransform.name + ": target reached, waiting " + toWait + " seconds before generating new one");
                yield return new WaitForSeconds(toWait);

                //generate random target inside patrol area
                _target = new Vector3(Random.Range(_patrolArea.bounds.min.x, _patrolArea.bounds.max.x), 0, Random.Range(_patrolArea.bounds.min.z, _patrolArea.bounds.max.z));
                _agent.destination = _target;
                _targetReached = false;
            }
        }
    }

    private void validateParameters() {
        float _minPatrolSecondsDelayThreshold = 0.0f; //_delayBounds.x cannot be lower than this value

        if (_patrolArea == null) {
            Debug.LogWarning("No patrol area set for " + _toMoveTransform.name + ", disabling movement");
            HasValidParameters = false;
        }

        //adjust patrol behaviour delays
        if (_delayBounds.x < _minPatrolSecondsDelayThreshold) {
            _delayBounds.x = _minPatrolSecondsDelayThreshold + 0.5f;
            Debug.LogWarning("_minPatrolSecondsDelay for " + _toMoveTransform.name + " is lower than " + _minPatrolSecondsDelayThreshold + ", setting it to " + _delayBounds.x);
        }

        if (_delayBounds.y < _delayBounds.x) {
            _delayBounds.y = _delayBounds.x + 1.0f;
            Debug.LogWarning("_maxPatrolSecondsDelay for " + _toMoveTransform.name + " is lower than _minPatrolDelaySeconds, setting it to " + _delayBounds.y);
        }

    }

}
