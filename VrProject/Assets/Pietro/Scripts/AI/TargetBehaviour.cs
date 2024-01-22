using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetBehaviour : MovementBehaviour {
    private float _minDistance;
    private float _maxDistance;
    private float _speedMultiplier = 2f; //multiplies speed of agent when it's too distant from the target
    private Transform _target;
    private bool _reachTarget = false;
    private bool _isFar = false;
    private bool _targetReached = false;

    public event EventHandler TargetReachedEvent;

    public TargetBehaviour(float minDist, float maxDist, Transform trgt, Transform toMoveTransform, bool rt = false) : base(toMoveTransform) { 
        _target= trgt;
        _reachTarget= rt;
        if (_reachTarget) {
            _minDistance = 1;
        } else {
            _minDistance = minDist;
        }
        _maxDistance = maxDist;
        ValidateParameters();
    }

    public override void Move() {

        NavMeshHit hit;
        bool justReachedTarget = false;
        if( NavMesh.SamplePosition(_target.position, out hit, 2, NavMesh.AllAreas)) {
            _agent.destination = hit.position;
        } else {
            _agent.destination = _target.position;
        }

        if (!_reachTarget) {
            //NPC will stop at a given distance from the target
            if (_agent.remainingDistance <= _minDistance) {
                justReachedTarget = true;
                _agent.isStopped = true;
                if (_isFar) {
                    _agent.speed *= _speedMultiplier;
                    _isFar = false;
                }
            } else {
                _agent.isStopped = false;
                if (!_isFar && _agent.remainingDistance >= _maxDistance) {
                    _agent.speed /= _speedMultiplier;
                    _isFar = true;
                }
            }
        } else if (_agent.remainingDistance <= _agent.stoppingDistance) { 
            justReachedTarget = true;
        }

        //event is only thrown once
        if (!_targetReached && justReachedTarget) {
            _targetReached = true;
            if (TargetReachedEvent != null) {
                TargetReachedEvent(this, EventArgs.Empty);
            }
        }

    }

    private Vector3 GetTargetPosOnNavMesh() {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(_target.position.x, _target.position.y, _target.position.z), Vector3.down, out hit, NavMesh.AllAreas)) {
            // Set the agent's destination to the hit point with the correct Y coordinate
            return hit.point;
        } else {
            return _target.position;
        }
    }

    private void ValidateParameters() {
        if (_target == null) {
            Debug.LogWarning("No target set for " + _toMoveTransform.name + ", FollowTargetBehaviour not activated");
            HasValidParameters = false;
        }
    }

    public override void Stop() {
        //method disabled
    }

    public override void Start() {
        //method disabled
    }

    public bool IsTargetReached() {
        return _targetReached; 
    }
}
