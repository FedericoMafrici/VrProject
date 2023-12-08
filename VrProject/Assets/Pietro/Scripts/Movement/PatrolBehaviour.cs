using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour : MovementBehaviour {

    private Vector3 _target;
    private BoxCollider _patrolArea;
    private Vector2 _delayBounds;
    private bool _targetReached;
    private bool _targetOnNavMesh;
    private bool _onlyMoveInSurroundings = false;
    private float _range = 5.0f;
    private bool _endCoroutine;

    public PatrolBehaviour(Transform toMoveTransform, BoxCollider patrolArea, Vector2 delayBounds) : base(toMoveTransform) {
        _delayBounds = delayBounds;
        _patrolArea = patrolArea;
        _endCoroutine = false;
        validateParameters();
    }

    public override void Delete() {
        _endCoroutine = true;
    }

    public override void Move(Transform objectToMove) {
       //check if target has been reached
       if (_agent.remainingDistance <= _agent.stoppingDistance) {
            _targetReached = true;
        }
        updateBehaviour();
    }

    public IEnumerator updateTarget() {
        //generate random trget inside the patrol area
        if (HasValidParameters) {

            while (!_endCoroutine) {


                //generate random target inside patrol area
                generateRandomTarget();

                //wait for target to be reached before generating a new one
                //if target is not on NavMesh and thus cannot be reached do not wait
                while (_targetOnNavMesh && !_targetReached) {
                    yield return null;
                    if (_endCoroutine) {
                        break;
                    }
                }
                if (_endCoroutine) {
                    break;
                }

                float toWait = Random.Range(_delayBounds.x, _delayBounds.y); //random delay befor generating new target
                Debug.Log(_toMoveTransform.name + ": target reached, waiting " + toWait + " seconds before generating new one");
                yield return new WaitForSeconds(toWait);
                if(_endCoroutine) {
                    break;
                }
            }
        }
    }

    private void validateParameters() {
        float _minPatrolSecondsDelayThreshold = 0.0f; //_delayBounds.x cannot be lower than this value

        if (_patrolArea == null) {
            Debug.LogWarning("No patrol area set for " + _toMoveTransform.name + ", PatrolBehaviour not activated");
            HasValidParameters = false;
        }

        //adjust patrol behaviour delays
        if (_delayBounds.x < _minPatrolSecondsDelayThreshold) {
            _delayBounds.x = _minPatrolSecondsDelayThreshold + 0.5f;
            Debug.LogWarning("Min Patrol Seconds Delay for " + _toMoveTransform.name + " is lower than " + _minPatrolSecondsDelayThreshold + ", setting it to " + _delayBounds.x);
        }

        if (_delayBounds.y < _delayBounds.x) {
            _delayBounds.y = _delayBounds.x + 1.0f;
            Debug.LogWarning("Max Patrol Seconds Delay for " + _toMoveTransform.name + " is lower than _minPatrolDelaySeconds, setting it to " + _delayBounds.y);
        }

    }

    private void generateRandomTarget() {
        NavMeshHit hit;
        Vector3 tmpTarget;

        if (!_onlyMoveInSurroundings) {
            tmpTarget = new Vector3(Random.Range(_patrolArea.bounds.min.x, _patrolArea.bounds.max.x), _toMoveTransform.position.y, Random.Range(_patrolArea.bounds.min.z, _patrolArea.bounds.max.z));
        } else {
            tmpTarget = _toMoveTransform.position + Random.insideUnitSphere * _range; //random point in a sphere surrounding the agent
            bringInsidePatrolArea(ref tmpTarget);
        }

        //find closest point on NavMesh within 1 unit
        if (NavMesh.SamplePosition(tmpTarget, out hit, 1.0f, -1)) {
            _target = hit.position;
            _targetOnNavMesh = true;
        } else {
            _target = tmpTarget;
            _targetOnNavMesh = false;
        }

        _agent.destination = _target;
        _targetReached = false;
        Debug.Log("New patrol destination set");

    }

    private void bringInsidePatrolArea(ref Vector3 point) {
        //if a point is outside of patrol area bring it back inside
        if (point.x < _patrolArea.bounds.min.x)
            point.x = _patrolArea.bounds.min.x;

         else if (point.x > _patrolArea.bounds.max.x)
            point.x = _patrolArea.bounds.max.x;

        if (point.z < _patrolArea.bounds.min.z)
            point.z = _patrolArea.bounds.min.z;

        else if (point.x > _patrolArea.bounds.max.z)
            point.z = _patrolArea.bounds.max.z;

        point.y = _toMoveTransform.position.y;
    }

}
