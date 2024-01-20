using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _minDistance = minDist;
        _maxDistance = maxDist;
        _target= trgt;
        _reachTarget= rt;
        ValidateParameters();
    }

    public override void Move() {

        bool justReachedTarget = false;
        _agent.destination = _target.position;

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
