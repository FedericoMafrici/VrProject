using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType {
    DEFAULT,
    TARGET_1,
    TARGET_2
}

public class Targettable : MonoBehaviour {
    [SerializeField] private TargetType _type;
    [SerializeField] private int maxFollowers = 3;
    [SerializeField] private int currentFollowers;
    HashSet<NPCMover> _followers = new HashSet<NPCMover>();

    private void Start() {
        currentFollowers = 0;
    }

    public TargetType getType() {
        return _type;
    }

    public bool trySubscribe(NPCMover newFollower) {
        bool retValue = true;
        if (!_followers.Contains(newFollower)) {
            if (currentFollowers >= maxFollowers)
                retValue = false;
            else {
                _followers.Add(newFollower);
                currentFollowers++;
            }
        }
        return retValue;
    }

    public void unsubscribe(NPCMover follower) {
        if (_followers.Contains(follower)) {
            _followers.Remove(follower);
            currentFollowers--;
            if (currentFollowers < 0) {
                currentFollowers = 0;
                Debug.Log("Follower count for " + transform.name + " went below 0, setting follower count to 0");
            }
        } else {
            Debug.Log("Not subscripted follower " + follower.transform.name + " tried to unsubscribe from Targettable: " + transform.name);
        }
    }

}
