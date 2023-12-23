using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFollowPathBehaviour : MovementBehaviour {
    private Transform[] _waypoints;
    private int _waypointId;

    public BasicFollowPathBehaviour(Transform toMoveTransform, Transform[] path) : base(toMoveTransform) {
        _waypoints = path;
        ValidateParameters();
        if (HasValidParameters) {
            _waypointId = 0;
            _agent.destination = _waypoints[_waypointId].position;
        }  else {
            _waypointId = -1;
        }
    }

    public override void Move() {

        if (!_agent.isStopped) {
            if (_agent.remainingDistance <= _agent.stoppingDistance) {
                _waypointId++;
                //Debug.Log(_toMoveTransform.name + " going for: " + waypoints[_waypointId].name);

                if (_waypointId >= _waypoints.Length) {
                    _waypointId = -1; //set waypoint ID to invalid
                }

                if (_waypointId >= 0) {
                    Transform currentTarget = _waypoints[_waypointId];

                    if (currentTarget != null) {
                        _agent.destination = currentTarget.position;

                    } else {
                        Debug.LogWarning("A null waypoint was found for " + _toMoveTransform.name + " while following path, returning to PatrolBehaviour");
                    }

                }
            }

            
        }

        //if no waypoint was found change to patrol behaviour
        if (_waypointId < 0) {
            SetNewPatrolBehaviour();
        }


    }

    private void ValidateParameters() {
        if (HasValidParameters) {
            if (_waypoints == null) {
                Debug.LogWarning("No path set for " + _toMoveTransform.name + ", BasicFollowPathBehaviour not activated");
                HasValidParameters = false;
            } else if (_waypoints.Length <= 0) {
                Debug.LogWarning("Path set for " + _toMoveTransform.name + "has no waypoints, BasicFollowPathBehaviour not activated");
            }
        }
    }
}
