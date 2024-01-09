using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Petter : MonoBehaviour {

    [SerializeField] Camera _playerCamera;
    [SerializeField] private const float _petDistance = 1.5f;
    private RaycastManager<Pettable> _raycastManager;
    private KeyCode _interactKey = KeyCode.Mouse1;

    public static event EventHandler StartedPetting;
    public static event EventHandler StoppedPetting;
    public static event EventHandler InPetRange;
    public static event EventHandler OutOfPetRange;




    // Start is called before the first frame update
    void Start() {
        if (_playerCamera == null)
            Debug.LogError("No camera associated to " + transform.name);

        _raycastManager = new RaycastManager<Pettable>(_petDistance, true);
    }

    // Update is called once per frame
    void Update() {
        bool inputPressed = Input.GetKey(_interactKey);
        InteractionResult<Pettable> interactResult = _raycastManager.CheckRaycast(_playerCamera, _interactKey, inputPressed);

        bool didPet = interactResult.didInteract;
        Pettable previousPetted = interactResult.previousInteracted;
        Pettable petted = interactResult.currentInteracted;
        bool currentRayDidHit = interactResult.currentRayDidHit;
        bool previousRayDidHit = interactResult.previousRayDidHit;

        if (interactResult.interactedWithNewTarget) {
            petted.PettingStarted();
            if (StartedPetting != null)
                StartedPetting(this, EventArgs.Empty);
        }

        if (interactResult.abandonedPreviousTarget) {
            previousPetted.PettingStopped();
            if (StoppedPetting != null)
                StoppedPetting(this, EventArgs.Empty);
        }

        if (interactResult.enteredRange) {
            if (InPetRange != null)
                InPetRange(this, EventArgs.Empty);
        } else if (interactResult.exitedRange) {
            if (OutOfPetRange != null)
                OutOfPetRange(this, EventArgs.Empty);
        }

        if (interactResult.canCallBehaviour) {
            petted.Pet(this, interactResult.travelledDistance);
        }

    }

    public string GetPetText() {
        return "Premi " + _interactKey.ToString() + " per accarezzare";
    }

}
