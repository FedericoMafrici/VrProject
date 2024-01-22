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
    private float _range = 5.0f;
    private float _toWaitForNextTarget;

    public PatrolBehaviour(Transform toMoveTransform, BoxCollider patrolArea, Vector2 delayBounds) : base(toMoveTransform) {
        _delayBounds = delayBounds;
        _patrolArea = patrolArea;
        _toWaitForNextTarget = 0.0f;
        _targetReached = true;
        ValidateParameters();
    
    }

    public override void Move() {


        //check if target has been reached or is not reachable (not on NavMesh)
        if (!_agent.isStopped) {
            if (!_targetReached) {
                if (_agent.remainingDistance <= _agent.stoppingDistance || !_targetOnNavMesh) {
                    _targetReached = true;
                }
            }


            if (_targetReached) {
                _toWaitForNextTarget -= Time.deltaTime;
                if (_toWaitForNextTarget <= 0) {

                    //sample patrol area to find a new target, if no target is found re-sample during next update
                    if (GenerateRandomTarget())
                        _toWaitForNextTarget = Random.Range(_delayBounds.x, _delayBounds.y); //random delay befor generating new target
                    else
                        Debug.LogWarning(_npcMover.transform + " target generation failed, resampling at next update");

                }
            }

            UpdateBehaviour();
        }
    }

    private void ValidateParameters() {
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

    private bool GenerateRandomTarget() {
        NavMeshHit hit;
        Vector3 tmpTarget;
        bool targetAssigned = false;

        tmpTarget = SamplePatrolArea();
        

        //try to find closest point on NavMesh within 1 unit
        if (NavMesh.SamplePosition(tmpTarget, out hit, 1.0f, -1)) {
            _target = hit.position;
            _targetOnNavMesh = true;
            _agent.destination = _target;
            _targetReached = false;
            targetAssigned= true;
        } /*else {
            _target = tmpTarget;
            _targetOnNavMesh = false;
        }*/
        /*
        _agent.destination = _target;
        _targetReached = false;
        */


        return targetAssigned;
    }

    private Vector3 SamplePatrolArea() {
        float x = Random.Range(_patrolArea.bounds.min.x, _patrolArea.bounds.max.x);
        float z = Random.Range(_patrolArea.bounds.min.z, _patrolArea.bounds.max.z);
        float y = _patrolArea.transform.position.y;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(x, y, z), Vector3.down, out hit, _patrolArea.bounds.max.y - _patrolArea.bounds.min.y, NavMesh.AllAreas)) {
            // Set the agent's destination to the hit point with the correct Y coordinate
            return hit.point;
        } else {
            return new Vector3(x, _toMoveTransform.position.y, z);
        }
    }

    private void BringInsidePatrolArea(ref Vector3 point) {
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

    public override void Stop() {
        _agent.isStopped= true;
        base.Stop();
    }

    public override void Start() {
        _agent.isStopped= false;
        base.Start();
    }
}
