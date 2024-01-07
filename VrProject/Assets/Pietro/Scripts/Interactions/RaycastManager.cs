using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public struct InteractionResult<T> {
    public bool canCallBehaviour;
    public bool didInteract;
    public bool previousRayDidHit;
    public bool currentRayDidHit;
    public bool enteredRange;
    public bool exitedRange;
    public bool interactedWithNewTarget;
    public bool abandonedPreviousTarget;
    public float travelledDistance;
    public T currentInteracted;
    public T previousInteracted;

}

public class RaycastManager<T> where T : class {
    private Camera _playerCamera;
    private float _range;
    private bool useRubs;
    private float _distanceEpsilon = 0.01f;
    private float _accumulatedDistance = 0f;
    private float _minTravelledDistance = 0.5f;
    private Vector3 _previousHitPosition = Vector3.zero;
    private bool _isInteracting = false;
    private bool _previousRayDidHit = false;
    private bool _canInteract = true;
    T _previousInteracted = null;

    public RaycastManager(Camera cam, float r, bool ur) {
        _playerCamera = cam;
        _range = r;
        useRubs = ur;
    }

    public void Reset() {
        _accumulatedDistance= 0f;
        _previousHitPosition = Vector3.zero;
        _isInteracting= false;
        _previousRayDidHit= false;
        _previousInteracted= null;
    }

    public InteractionResult<T> CheckRaycast(KeyCode inputKey, bool inputPressed, int layerMask = ~0, int maxParentDepth = 5) {
        bool didInteract = false;
        T currentInteracted= null;
        RaycastHit hit;
        InteractionResult<T> result = new InteractionResult<T>();
        result.canCallBehaviour = false;
        result.travelledDistance = 0;
        result.previousInteracted = _previousInteracted;
        result.currentRayDidHit= false;
        result.previousRayDidHit = _previousRayDidHit;
        result.enteredRange = false;
        result.exitedRange = false;
        result.interactedWithNewTarget = false;
        result.abandonedPreviousTarget = false;

        if (_canInteract) {
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _range, layerMask)) {
                Transform hitTransform = hit.transform;
                currentInteracted = hitTransform.GetComponent<T>();

                //check if intersected object is child of a "T" class
                if (currentInteracted == null) {
                    Transform tmpTransform = hitTransform;
                    int curDepth = 0;
                    while (currentInteracted == null && tmpTransform.parent != null && curDepth < maxParentDepth) {
                        curDepth++;
                        currentInteracted = tmpTransform.parent.GetComponent<T>();
                        tmpTransform = tmpTransform.parent;
                    }
                }

                if (currentInteracted != null) {

                    if (inputPressed) {
                        if (useRubs) {
                            CheckRubs(ref currentInteracted, hit, ref result);
                        } else {
                            result.canCallBehaviour = true;
                        }

                        didInteract = true;
                        _isInteracting = true;
                        _previousHitPosition = hit.point;
                    }
                }

                if (!didInteract) { 
                    _isInteracting = false;
                    _accumulatedDistance = 0;
                }
            }
        }

        result.currentRayDidHit = (currentInteracted != null);
        result.didInteract = didInteract;
        result.currentInteracted = currentInteracted;

        //check if the user entered or exited the range needed to perform the action
        if (result.currentRayDidHit && !_previousRayDidHit) {
            result.enteredRange = true;
        } else if (!result.currentRayDidHit && _previousRayDidHit) {
            result.exitedRange = true;
        }

        //check if the user interacted with a new target and/or abandoned a target it was interacting with
        if (didInteract) {
            if (currentInteracted != _previousInteracted) {
                if (currentInteracted != null) {
                    result.interactedWithNewTarget = true;
                }

                if (_previousInteracted != null) {
                    //Debug.Log("Abandoned previous target");
                    result.abandonedPreviousTarget = true;
                }
            }

            _previousInteracted = currentInteracted;

        } else {
            if (_previousInteracted != null) {
                result.abandonedPreviousTarget = true;
            }


            //user did not interact with anything during this call
            _previousInteracted = null;
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

    private void CheckRubs(ref T currentInteracted, RaycastHit hit, ref InteractionResult<T> result) {
        if (_isInteracting == true && _previousInteracted != null && _previousInteracted == currentInteracted) {
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

        } else {
            _accumulatedDistance = 0;
        }
    }

    public void SetCanRub(bool value) {
        _canInteract = value;
    }
}
