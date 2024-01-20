using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGenerator : MonoBehaviour {
    [Header("Basic Follow Path Attributes")]
    [SerializeField] private Transform _path;

    [Header("Follow Target Attributes")]
    [SerializeField] private Transform _targetToFollow;
    [SerializeField] private float _minDistance = 2f;
    [SerializeField] private float _maxDistance = 5f;

    public MovementBehaviour GenerateBehaviour(BehaviourID behaviourID, NPCMover npcMover) {
        MovementBehaviour newBehaviour = null;
        bool subscribeToTargetReached = false;
        switch (behaviourID) {

            case BehaviourID.BASIC_FOLLOW_PATH:
                Transform[] waypoints = new Transform[_path.childCount];

                for (int i =0; i < _path.childCount; i++) {
                    //Debug.Log(npcMover.transform.name + ": path[" + i + "] = " + _path.GetChild(i).transform.name);
                    waypoints[i] = _path.GetChild(i).transform;
                }

                newBehaviour = new BasicFollowPathBehaviour(npcMover.transform, waypoints);
                break;

            case BehaviourID.FOLLOW_TARGET:
                newBehaviour = new TargetBehaviour(_minDistance, _maxDistance, _targetToFollow, npcMover.transform);
                subscribeToTargetReached = true;
                break;

            case BehaviourID.REACH_TARGET:
                newBehaviour = new TargetBehaviour(_minDistance, _maxDistance, _targetToFollow, npcMover.transform, true);
                subscribeToTargetReached = true;
                break;

            default:
                break;
                
        }

        if (subscribeToTargetReached) {
            TargetBehaviour tmpTgtBehaviour = newBehaviour as TargetBehaviour;
            tmpTgtBehaviour.TargetReachedEvent += npcMover.OnDestinationReached;
        }

        return newBehaviour;
    }
  
}
