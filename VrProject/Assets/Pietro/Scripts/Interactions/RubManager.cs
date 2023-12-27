using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct RubbingResult<T> {
    public bool canCallBehaviour;
    public bool didRub;
    public bool previousRayDidHit;
    public bool currentRayDidHit;
    public bool enteredRange;
    public bool exitedRange;
    public bool interactedWithNewTarget;
    public bool abandonedPreviousTarget;
    public float travelledDistance;
    public T currentRubbed;
    public T previousRubbed;

}

public class RubManager<T> where T : class {
    private Camera _playerCamera;
    private float _range;
    private float _distanceEpsilon = 0.01f;
    private float _accumulatedDistance = 0f;
    private float _minTravelledDistance = 0.5f;
    private Vector3 _previousHitPosition = Vector3.zero;
    private bool _isRubbing = false;
    private bool _previousRayDidHit = false;
    private bool _canRub = true;
    T _previousRubbed = null;

    public RubManager(Camera cam, float r) {
        _playerCamera = cam;
        _range = r;
    }

    public RubbingResult<T> CheckRubs(KeyCode inputKey, int maxParentDepth = 5) {
        bool didRub = false;
        T rubbed= null;
        RaycastHit hit;
        RubbingResult<T> result = new RubbingResult<T>();
        result.canCallBehaviour = false;
        result.travelledDistance = 0;
        result.previousRubbed = _previousRubbed;
        result.currentRayDidHit= false;
        result.previousRayDidHit = _previousRayDidHit;
        result.enteredRange = false;
        result.exitedRange = false;
        result.interactedWithNewTarget = false;
        result.abandonedPreviousTarget = false;

        if (_canRub) {
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _range)) {
                Transform hitTransform = hit.transform;
                rubbed = hitTransform.GetComponent<T>();

                //check if intersected object is child of a "T" class
                if (rubbed == null) {
                    Transform tmpTransform = hitTransform;
                    int curDepth = 0;
                    while (rubbed == null && tmpTransform.parent != null && curDepth < maxParentDepth) {
                        curDepth++;
                        rubbed = tmpTransform.parent.GetComponent<T>();
                        tmpTransform = tmpTransform.parent;
                    }
                }

                if (rubbed != null) {

                    if (Input.GetKey(inputKey)) {
                        if (_isRubbing == true && _previousRubbed != null && _previousRubbed == rubbed) {
                            float petTravelledDistance = GetRubDistance(hit);

                            //if movement is too small it won't be recorded
                            if (petTravelledDistance >= _distanceEpsilon) {

                                _accumulatedDistance += petTravelledDistance;

                                //accumulated distance should be at least equal to a minimum required distance
                                if (_accumulatedDistance >= _minTravelledDistance) {
                                    //"hand" movement recorded
                                    result.travelledDistance = _accumulatedDistance;
                                    result.canCallBehaviour = true;
                                    _accumulatedDistance = 0;
                                }
                            }

                            Vector3 direction = hit.point - _previousHitPosition;

                        } else {
                            _accumulatedDistance = 0;
                        }

                        didRub = true;
                        _isRubbing = true;
                        _previousHitPosition = hit.point;
                    }
                }

                if (!didRub) { 
                    _isRubbing = false;
                    _accumulatedDistance = 0;
                }
            }
        }

        result.currentRayDidHit = (rubbed != null);
        result.didRub = didRub;
        result.currentRubbed = rubbed;

        //check if the user entered or exited the range needed to perform the action
        if (result.currentRayDidHit && !_previousRayDidHit) {
            result.enteredRange = true;
        } else if (!result.currentRayDidHit && _previousRayDidHit) {
            result.exitedRange = true;
        }

        //check if the user interacted with a new target and/or abandoned a target it was interacting with
        if (didRub) {
            if (rubbed != _previousRubbed) {
                if (rubbed != null) {
                    result.interactedWithNewTarget = true;
                }

                if (_previousRubbed != null) {
                    //Debug.Log("Abandoned previous target");
                    result.abandonedPreviousTarget = true;
                }
            }

            _previousRubbed = rubbed;
        } else {
            if (_previousRubbed != null) {
                result.abandonedPreviousTarget = true;
            }


            //user did not rub anything during this call
            _previousRubbed = null;
        }

        //keep memory of this execution
        _previousRayDidHit = result.currentRayDidHit;
        return result;
    }

    private float GetRubDistance(RaycastHit hit) {
        Vector3 distance = hit.point - _previousHitPosition;
        /*
        //project the distance between current hit point and last one onto camera viewport plane

        Vector3 distancePerp = ((Vector3.Dot(distance, _playerCamera.transform.forward))
                                   / (Vector3.Dot(_playerCamera.transform.forward, _playerCamera.transform.forward)))
                                   * _playerCamera.transform.forward;
        Vector3 distanceParallelToPlane = distance - distancePerp;
        */


        float travelledDistance = distance.magnitude;

        // Draw the ray in the Scene view
        Debug.DrawRay(_previousHitPosition, distance, Color.green);
        return travelledDistance;


    }

    public void SetCanRub(bool value) {
        _canRub = value;
    }
}
