using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TargetMinigameActivator : ItemTool {
    [SerializeField] private string _clueTextAppendix = "mungere";
    [SerializeField] private float _interactRange = 2f;
    [SerializeField] private Animal.AnimalName _animalType;

    private RaycastManager<TargetMinigame> _raycastManager;
    //private KeyCode _interactKey = KeyCode.Mouse0;

    private const string _clueText = "Premi [CLICK SINISTRO] per ";

    public static event EventHandler<ClueEventArgs> InMinigameRange;
    public static event EventHandler<ClueEventArgs> OutOfMinigameRange;

    TargetMinigameActivator(Item.ItemName name) : base(name) { }

    // Start is called before the first frame update
    public void Start() {
        base.Start();
        InitRaycastManager();
    }


    void InitRaycastManager() {
        _raycastManager = new RaycastManager<TargetMinigame>(_interactRange, false);
    }

    public override UseResult Use(PlayerItemManager itemManager) {
        // get reference to camera in order to determine raycast origin
        Camera playerCamera = itemManager.GetCamera();

        if (_raycastManager == null) {
            InitRaycastManager();
        }

        // generate default return value
        UseResult useResult = new UseResult();
        useResult.itemUsed = false;
        useResult.itemConsumed = false;

        //do raycast through RaycastManager
        bool inputPressed = InputManager.InteractionsAreEnabled() ? Input.GetMouseButtonDown(0) : false;

        InteractionResult<TargetMinigame> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, CanStartMinigame, LayerMask.GetMask("Animals"));

        TargetMinigame minigame = interactionResult.currentInteracted;

        //check result of raycast and determine if minigame can be started
        //update return value accordingly
        if (interactionResult.canCallBehaviour) {
            Debug.LogWarning("Starting minigame");
            minigame.BeginMinigame(playerCamera, itemManager.GetPlayerPickUpDrop());
            useResult.itemUsed = true;
            useResult.itemConsumed = false;
        }

        if (interactionResult.enteredRange) {
            if (InMinigameRange != null) {
                InMinigameRange(this, new ClueEventArgs(ClueID.TARGET_MINIGAME, MakeClueText()));
            }
        } else if (interactionResult.exitedRange) {
            if (OutOfMinigameRange != null) {
                OutOfMinigameRange(this, new ClueEventArgs(ClueID.TARGET_MINIGAME, MakeClueText()));
            }
        }

        return useResult;
    }

    private bool CanStartMinigame(TargetMinigame minigame) {
        return (minigame.GetAnimalType() == _animalType) && (!minigame.IsMinigameRunning());
    }

    void OnDestroy() {
        if (OutOfMinigameRange != null) {
            OutOfMinigameRange(this, new ClueEventArgs(ClueID.TARGET_MINIGAME, MakeClueText()));
        }
    }

    private string MakeClueText() {
        return _clueText + _clueTextAppendix;
    }
}
