using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pettable : MonoBehaviour
{
    private float _friendshipPercentage;
    float _distanceDelta = 0f; //minimum distance that needs to be travelled in order to consider the "pet" as valid
    [SerializeField] float _friendshipGrowthRate = 1.0f;

    private void Start() {
        _friendshipPercentage = 0;
    }

    public void Pet(Petter petter, float travelledDistance) {
        Debug.Log("Petted " + transform.name + " travelled distance = " + travelledDistance);
    }
}
