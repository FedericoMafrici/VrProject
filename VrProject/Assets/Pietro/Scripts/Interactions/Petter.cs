using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Petter : MonoBehaviour
{

    [SerializeField] SpriteManager _cameraSpriteManager;
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

    // Start is called before the first frame update
    void Start()
    {
        if (_playerCamera == null)
            Debug.LogError("No camera associated to " + transform.name);
        if(_cameraSpriteManager == null)
            Debug.LogError("no sprite manager detected for " + transform.name);

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
                    if (hitTransform.parent != null) {
                        petted = hitTransform.parent.GetComponent<Pettable>();
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
                                _lastPetted.HideProgressBar();
                            }
                            petted.ShowProgressBar();
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
                _lastPetted.HideProgressBar();
            }

            _accumulatedDistance = 0;
            _isPetting= false;
            _lastPetted = null;

            _cameraSpriteManager.UpdateCurrentSprite(SpriteType.DOT);
            if (petted != null && !_previousRayDidHit) {
                _cameraSpriteManager.SetCurrentSpriteColor(Color.green);
            } else if (petted == null && _previousRayDidHit) {
                _cameraSpriteManager.SetCurrentSpriteColor(Color.white);
            }

            _previousRayDidHit = (petted != null);
        } else {
            _cameraSpriteManager.UpdateCurrentSprite(SpriteType.HAND);
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