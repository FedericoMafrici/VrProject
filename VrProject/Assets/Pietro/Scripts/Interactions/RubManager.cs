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
    private Vector3 _lastRubPosition = Vector3.zero;
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


                                _lastRubPosition = hit.point;
                            }

                            Vector3 direction = hit.point - _lastRubPosition;

                            // Draw the ray in the Scene view
                            Debug.DrawRay(_lastRubPosition, direction, Color.green);

                        } else {
                            _accumulatedDistance = 0;
                        }

                        didRub = true;
                        _isRubbing = true;
                        _previousRubbed = rubbed;
                        _lastRubPosition = hit.point;
                    }
                }

                if (!didRub) {                    
                    _isRubbing = false;
                    _accumulatedDistance = 0;
                    _previousRubbed = null;
                }
            }
        }

        result.currentRayDidHit = (rubbed != null);

        result.didRub = didRub;
        result.currentRubbed = rubbed;
        _previousRayDidHit = result.currentRayDidHit;
        return result;
    }

    private float GetRubDistance(RaycastHit hit) {
        //project the distance between current hit point and last one onto camera viewport plane
        Vector3 distance = hit.point - _lastRubPosition;
        Vector3 distancePerp = (Vector3.Dot(distance, _playerCamera.transform.forward)
                                   / Vector3.Dot(_playerCamera.transform.forward, _playerCamera.transform.forward))
                                   * _playerCamera.transform.forward;
        float distanceOnCameraPlane = (distance - distancePerp).magnitude;
        return distanceOnCameraPlane;


    }

    public void SetCanRub(bool value) {
        _canRub = value;
    }
}
