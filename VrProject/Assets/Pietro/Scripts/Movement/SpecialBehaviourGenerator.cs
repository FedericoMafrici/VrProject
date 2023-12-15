using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBehaviourGenerator : MonoBehaviour {
    [Header("Basic Follow Path Attributes")]
    [SerializeField] private Transform _path;

    public MovementBehaviour generateBehaviour(SpecialBehaviourID behaviourID, NPCMover npcMover) {
        MovementBehaviour newBehaviour = null;
        switch (behaviourID) {

            case SpecialBehaviourID.BASIC_FOLLOW_PATH:
                Transform[] waypoints = new Transform[_path.childCount];

                for (int i =0; i < _path.childCount; i++) {
                    //Debug.Log(npcMover.transform.name + ": path[" + i + "] = " + _path.GetChild(i).transform.name);
                    waypoints[i] = _path.GetChild(i).transform;
                }

                newBehaviour = new BasicFollowPathBehaviour(npcMover.transform, waypoints);
                break;

            default:
                Debug.LogWarning("generateBehaviour() was called for " + npcMover.name + " but no behaviour was generated, requested behaviour was: " + behaviourID);
                break;
                
        }
        return newBehaviour;
    }
  
}
