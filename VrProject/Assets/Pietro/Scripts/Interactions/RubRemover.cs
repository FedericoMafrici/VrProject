using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubRemover : AnimalPartRemover {

    private RaycastManager<RemovablePart> _raycastManager;
    private const KeyCode _interactKey = KeyCode.Mouse0;

    public RubRemover(Item.ItemName itemName) : base(itemName) { }

    //protected override

    public void Start() {
        base.Start();
        _raycastManager = new RaycastManager<RemovablePart>(_interactRange, true);
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
        InteractionResult<RemovablePart> interactResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, CanBeRemoved);

        //check result of raycast and determine if some removable that can be removed was found
        //update return value accordingly
        RemovablePart toRemove = interactResult.currentInteracted;
        RemovablePart previousRemovable = interactResult.previousInteracted;
        if (interactResult.canCallBehaviour) {
            RemovePart(toRemove);
            if (interactResult.interactedWithNewTarget) {
                toRemove.RemovalStarted();
            }
        }


        if (interactResult.abandonedPreviousTarget) {
            previousRemovable.RemovalStopped();
        }

        if (interactResult.enteredRange) {
            //went in range
        } else if (interactResult.exitedRange) {
            //went out of range
        }

        if (interactResult.enteredRange) {
            ThrowInRangeEvent();
        } else if (interactResult.exitedRange /*&& interactResult.previousInteracted != null && CanBeRemoved(interactResult.previousInteracted)*/) {
            ThrowOutOfRangeEvent();
        }

        return useResult;
    }
    public override string GetActionText() {
        string text = "Premi " + _interactKey.ToString() + " e muovi la visuale per ";

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
