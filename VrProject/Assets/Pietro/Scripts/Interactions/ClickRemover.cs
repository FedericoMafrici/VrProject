using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRemover : AnimalPartRemover {
    private RaycastManager<RemovablePart> _raycastManager;
    private const KeyCode _interactKey = KeyCode.Mouse0;

    public ClickRemover(Item.ItemName itemName) : base(itemName) { }

    //protected override
    public void Start() {
        base.Start();
        _raycastManager = new RaycastManager<RemovablePart>(_interactRange, false);
    }

    public override UseResult Use(PlayerItemManager itemManager) {
        // get reference to camera in order to determine raycast origin
        Camera playerCamera = itemManager.GetCamera();

        // generate default return value
        UseResult useResult = new UseResult();
        useResult.itemUsed = false;
        useResult.itemConsumed = false;

        //do raycast through RaycastManager
        bool inputPressed = Input.GetKey(_interactKey);
        InteractionResult<RemovablePart> result = _raycastManager.CheckRaycast(playerCamera, inputPressed, CanBeRemoved);

        RemovablePart toRemove = result.currentInteracted;

        //check result of raycast and determine if some removable that can be removed was found
        //update return value accordingly
        if (result.canCallBehaviour) {
            RemovePart(result.currentInteracted);
            if (result.interactedWithNewTarget) {
                toRemove.RemovalStarted();
            }
            useResult.itemUsed = true;
        }

        if (result.abandonedPreviousTarget) {
            result.previousInteracted.RemovalStopped();
        }

        if (result.enteredRange) {
            ThrowInRangeEvent();
        } else if (result.exitedRange /*&& result.previousInteracted!= null && CanBeRemoved(result.previousInteracted)*/) {
            ThrowOutOfRangeEvent();
        }

        return useResult;
    }

    public override string GetActionText() {
        string text = "Premi " + _interactKey.ToString() + " per ";

        switch (_targetType) {
            case RemovableType.WOOL:
                text += " tosare";
                break;

            case RemovableType.MUD:
                text += " pulire";
                break;

            default:
                text += " interagire";
                break;
        }

        return text;
    }
}
