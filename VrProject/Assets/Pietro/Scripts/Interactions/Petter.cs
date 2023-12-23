using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Petter : MonoBehaviour
{

    [SerializeField] Camera _playerCamera;
    private const float _distanceEpsilon = 0.01f;
    private const float _minTravelledDistance = 0.5f;
    private float _accumulatedDistance = 0;
    //private float _minTravelledDistance = 1.0f;
    private bool _canPet = true;
    private const float _petDistance = 1.5f; //defines distance at which petter can interact with pettables
    private bool _isPetting = false;
    private Pettable _lastPetted = null;
    private Vector3 _lastPetPosition = Vector3.zero;
    private bool _previousRayDidHit = false; //bool value used to manage sprite changes in order to give cues to the player

    public static event EventHandler StartedPetting;
    public static event EventHandler StoppedPetting;
    public static event EventHandler InPetRange;
    public static event EventHandler OutOfPetRange;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerCamera == null)
            Debug.LogError("No camera associated to " + transform.name);

    }

    // Update is called once per frame
    void Update() {

        Pettable petted = null;
        bool didPet = false;

        if (_canPet) {
            RaycastHit hit;
            
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _petDistance)) {
                Transform hitTransform = hit.transform;
                petted = hitTransform.GetComponent<Pettable>();

                //check if intersected object is child of a "Pettable"
                if (petted == null) {
                    Transform tmpTransform = hitTransform;
                    int maxDepth = 5;
                    int curDepth = 0;
                    while (petted == null && tmpTransform.parent != null && curDepth < maxDepth) {
                        curDepth++;
                        petted = tmpTransform.parent.GetComponent<Pettable>();
                        tmpTransform = tmpTransform.parent;
                    }
                }

                if (petted != null) {
                    if (Input.GetMouseButton(1)) {
                        if (_isPetting == true && _lastPetted != null && _lastPetted == petted) {
                            float petTravelledDistance = GetPetDistance(hit);

                            //if movement is too small it won't be recorded
                            if (petTravelledDistance >= _distanceEpsilon) {

                                _accumulatedDistance += petTravelledDistance;
                                //accumulated distance should be at least equal to a minimum required distance before calling Pet()
                                if (_accumulatedDistance >= _minTravelledDistance) {
                                    //call pet method
                                    petted.Pet(this, _accumulatedDistance);
                                    _accumulatedDistance = 0;
                                }

                                //"hand" movement recorded, update pet method
                                _lastPetPosition = hit.point;
                            }

                            Vector3 direction = hit.point - _lastPetPosition;

                            // Draw the ray in the Scene view
                            Debug.DrawRay(_lastPetPosition, direction, Color.green);

                        } else {
                            if (_lastPetted != null) {
                                _lastPetted.PettingStopped();
                                //_lastPetted.HideProgressBar();
                            } else {
                                //just started petting a Pettable, send event
                                StartedPetting(this, EventArgs.Empty);
                            }
                            //petted.ShowProgressBar();
                            petted.PettingStarted();
                            _accumulatedDistance = 0;

                        }

                        didPet = true;
                        _isPetting = true;
                        _lastPetted = petted;
                        _lastPetPosition = hit.point;
                    }
                }
            }
        }

        if (!didPet) {
            if (_lastPetted != null) {
                _lastPetted.PettingStopped();
                StoppedPetting(this, EventArgs.Empty);
            }

            _accumulatedDistance = 0;
            _isPetting= false;
            _lastPetted = null;

            if (petted != null && !_previousRayDidHit) {
                InPetRange(this, EventArgs.Empty);
            } else if (petted == null && _previousRayDidHit) {
                OutOfPetRange(this, EventArgs.Empty);
            }

            _previousRayDidHit = (petted != null);
        } else {
            _previousRayDidHit = true;
        }
        
    }

    private float GetPetDistance(RaycastHit hit) {
        //project the distance between current hit point and last one onto camera viewport plane
        Vector3 distance = hit.point - _lastPetPosition;
        Vector3 distancePerp = (Vector3.Dot(distance, _playerCamera.transform.forward)
                                   / Vector3.Dot(_playerCamera.transform.forward, _playerCamera.transform.forward))
                                   * _playerCamera.transform.forward;
        float distanceOnCameraPlane = (distance - distancePerp).magnitude;
        return distanceOnCameraPlane;


    }

}
