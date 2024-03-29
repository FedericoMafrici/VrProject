using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetType {
    DEFAULT,
    TARGET_1,
    TARGET_2,
    TARGET_3,
    TARGET_4,
    APPLE_FOOD,
    APPLE_SEED,
    CHICKEN_FOOD,
    TREE_BRANCH,
    CARROT,
    WHEAT,
    BAOBAB_FRUIT,
    ALOE_LEAF,
    MEAT
}

public class Targettable : MonoBehaviour {
    [SerializeField] private TargetType _type;
    [SerializeField] private int _maxFollowers = 3;
    private int _currentFollowers = 0;
    HashSet<NPCMover> _followers = new HashSet<NPCMover>();

    private void Start() {
        _currentFollowers = 0;
    }

    public TargetType GetTargetType() {
        return _type;
    }

    public bool CanSubscribe(NPCMover newFollower) {
        bool retValue = true;

        //if new follower is not yet contained in followers set check if it can be added
        if (!_followers.Contains(newFollower) && _currentFollowers >= _maxFollowers)
            retValue = false;


        if (!retValue) {
            //Debug.Log(newFollower.transform.name + " tried to subscribe to " + transform.name + " but failed");
        }

        return retValue;

    }

    public void Subscribe(NPCMover newFollower) {
        if (!_followers.Contains(newFollower)) {

            _followers.Add(newFollower);
            _currentFollowers++;
            //Debug.Log(newFollower.transform.name + " subscribed to " + transform.name);
        }
    }

    public void Unsubscribe(NPCMover follower) {
        
        if (_followers.Contains(follower)) {
            _followers.Remove(follower);
            //Debug.Log(follower.transform.name + " is unsubscribing from " + transform.name + ", follower count before unsubscribing = " + _currentFollowers);
            _currentFollowers--;
            //Debug.Log(follower.transform.name + " unsubscribed from " + transform.name + ", follower count = " + _currentFollowers);
            if (_currentFollowers < 0) {
                _currentFollowers = 0;
                Debug.LogWarning("Follower count for " + transform.name + " went below 0, setting follower count to 0");
            }
        } else {
            Debug.LogWarning("Not subscripted follower " + follower.transform.name + " tried to unsubscribe from Targettable: " + transform.name);
        }
        
    }

}
