using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petter : MonoBehaviour
{

   
    [SerializeField] Camera _playerCamera;
    float _petTravelledDistance = 0;
    float _minTravelledDistance = 1.0f;
    float _lastPetElapsed = 0f;
    float _samplePeriod = 0.5f;
    private bool _can_pet = true;
    private float _petdistance = 100f; //defines distance at which petter can interact with pettables
    private bool _isPetting = false;
    Pettable _lastPetted = null;
    Vector3 _lastPetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerCamera == null)
            Debug.LogError("No camera associated to " + transform.name);

    }

    // Update is called once per frame
    void Update() {

        bool didPet = false;

        if (_can_pet) {
            RaycastHit hit;
            
            if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _petdistance)) {
                Transform hitTransform = hit.transform;
                Pettable petted = hitTransform.GetComponent<Pettable>();

                //check if intersected object is child of a "Pettable"
                if (petted == null) {
                    if (hitTransform.parent != null) { 
                        petted = hitTransform.parent.GetComponent<Pettable>();
                    }
                }

                if (petted != null) {
                    if (Input.GetMouseButton(1)) {
                        if (_isPetting == true && _lastPetted != null && _lastPetted == petted) {
                            Vector3 distance = hit.point - _lastPetPosition;
                            Vector3 distancePerp = (Vector3.Dot(distance, _playerCamera.transform.forward)
                                                    / Vector3.Dot(_playerCamera.transform.forward, _playerCamera.transform.forward))
                                                    * _playerCamera.transform.forward;
                            float distanceOnCameraPlane = (distance - distancePerp).magnitude;

                            _petTravelledDistance += distanceOnCameraPlane;
                            if (_petTravelledDistance >= _minTravelledDistance) {
                                //call pet method
                                petted.Pet(this, _petTravelledDistance);

                                //reset travelled distance
                                _petTravelledDistance= 0;
                            }

                        } else {
                            _petTravelledDistance= 0;
                            _isPetting = true;
                            _lastPetted = petted;
                        }

                        didPet = true;
                        _lastPetPosition = hit.point;
                    }
                    
                }
            }
        }

        if (!didPet) {
            _petTravelledDistance= 0;
            _isPetting= false;
            _lastPetted = null;
        }
        
    }

    public void set_can_pet(bool cp) {
        _can_pet = cp;
    }

    private void samplePets(RaycastHit hit) {
        //TODO: this
        Vector3 distance = hit.point - _lastPetPosition;
        Vector3 distancePerp = (Vector3.Dot(distance, _playerCamera.transform.forward)
                                / Vector3.Dot(_playerCamera.transform.forward, _playerCamera.transform.forward))
                                * _playerCamera.transform.forward;
        float distanceOnCameraPlane = (distance - distancePerp).magnitude;
    }
}
