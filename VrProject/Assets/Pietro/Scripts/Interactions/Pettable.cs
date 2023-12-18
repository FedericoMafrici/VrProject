using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pettable : MonoBehaviour
{
    private const float _maxFriendship = 100.0f;
    private float _friendship;
    [SerializeField] float _friendshipGrowthRate = 10.0f;

    private void Start() {
        _friendship = 0;
    }

    public void Pet(Petter petter, float travelledDistance) {
        if (_friendship < _maxFriendship) {
            _friendship += travelledDistance * _friendshipGrowthRate;
            if (_friendship >= _maxFriendship) {
                _friendship = _maxFriendship;
            }
            Debug.Log(transform.name + ": friendship at " + _friendship + "%");
            //Debug.Log("Petted " + transform.name + " travelled distance = " + travelledDistance);
        }

        
    }
}
