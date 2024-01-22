using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Petter : MonoBehaviour {

    [SerializeField] Camera _playerCamera;
    [SerializeField] private float _petDistance = 1.0f;
    [SerializeField] private string _clueText = "Premi [CLICK DESTRO] e muovi [MOUSE] per accarezzare";
    private RaycastManager<Pettable> _raycastManager;
    private KeyCode _interactKey = KeyCode.Mouse1;

    public static event EventHandler<ClueEventArgs> StartedPetting;
    public static event EventHandler<ClueEventArgs> StoppedPetting;
    public static event EventHandler<ClueEventArgs> InPetRange;
    public static event EventHandler<ClueEventArgs> OutOfPetRange;

    public static event Action LookedAtPettable;

    // Start is called before the first frame update
    void Start() {
        if (_playerCamera == null)
            Debug.LogError("No camera associated to " + transform.name);

        _raycastManager = new RaycastManager<Pettable>(_petDistance, true);
    }

    // Update is called once per frame
    void Update() {
        bool inputPressed = InputManager.InputsAreEnabled() ? Input.GetKey(_interactKey) : false;
        InteractionResult<Pettable> interactResult = _raycastManager.CheckRaycast(_playerCamera, inputPressed, LayerMask.GetMask("Animals"));

        bool didPet = interactResult.didInteract;
        Pettable previousPetted = interactResult.previousInteracted;
        Pettable petted = interactResult.currentInteracted;
        bool currentRayDidHit = interactResult.currentRayDidHit;
        bool previousRayDidHit = interactResult.previousRayDidHit;

        if (interactResult.interactedWithNewTarget) {
            petted.PettingStarted();
            if (StartedPetting != null)
                StartedPetting(this, new ClueEventArgs(ClueID.PET, _clueText));
        }

        if (interactResult.abandonedPreviousTarget) {
            previousPetted.PettingStopped();
            if (StoppedPetting != null)
                StoppedPetting(this, new ClueEventArgs(ClueID.PET, _clueText));
        }

        if (interactResult.enteredRange) {
            if (InPetRange != null)
                InPetRange(this, new ClueEventArgs(ClueID.PET, _clueText));

            if (LookedAtPettable != null)
                LookedAtPettable();

        } else if (interactResult.exitedRange) {
            if (OutOfPetRange != null)
                OutOfPetRange(this, new ClueEventArgs(ClueID.PET, _clueText));
        }

        if (interactResult.canCallBehaviour) {
            petted.Pet(this, interactResult.travelledDistance);
        }

    }

    public string GetPetText() {
        return "Premi " + _interactKey.ToString() + " per accarezzare";
    }

    void OnDestroy() {
        if (StoppedPetting != null)
            StoppedPetting(this, new ClueEventArgs(ClueID.PET, _clueText));

        if (OutOfPetRange != null)
            OutOfPetRange(this, new ClueEventArgs(ClueID.PET, _clueText));
    }

}
