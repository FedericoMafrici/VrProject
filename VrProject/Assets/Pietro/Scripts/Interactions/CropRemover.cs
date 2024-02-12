using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropRemover : ItemTool {
    [SerializeField] protected float _interactRange = 3f;
    private string _clueText = "Premi [CLICK SINISTRO] per rimuovere la pianta";
    private RaycastManager<FarmingLand> _raycastManager;

    public static event EventHandler<ClueEventArgs> InRange;
    public static event EventHandler<ClueEventArgs> OutOfRange;

    CropRemover() : base(ItemName.Shovel) { }

    // Start is called before the first frame update
    public void Start() {
        base.Start();
        InitRaycastManager();
    }


    void InitRaycastManager() {
        _raycastManager = new RaycastManager<FarmingLand>(_interactRange, false);
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
        InteractionResult<FarmingLand> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, HasCrop/*, LayerMask.GetMask("Animals")*/);

        FarmingLand land = interactionResult.currentInteracted;

        //check result of raycast and determine if food can be given to animal
        //update return value accordingly
        if (interactionResult.canCallBehaviour) {
            land.DestroyCrop();
            useResult.itemUsed = true;
            //RemovePart(result.currentInteracted);
        }

        if (interactionResult.enteredRange) {
            if (InRange != null) {
                InRange(this, new ClueEventArgs(ClueID.REMOVE_CROP, _clueText));
            }
        } else if (interactionResult.exitedRange) {
            if (OutOfRange != null) {
                OutOfRange(this, new ClueEventArgs(ClueID.REMOVE_CROP, _clueText));
            }
        }

        return useResult;
    }

    private bool HasCrop(FarmingLand land) {
        return land.crop != null;
    }

    private void OnDestroy() {
        if (OutOfRange != null) {
            OutOfRange(this, new ClueEventArgs(ClueID.REMOVE_CROP, _clueText));
        }
    }

}
