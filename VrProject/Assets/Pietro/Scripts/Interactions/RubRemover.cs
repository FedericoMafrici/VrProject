using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubRemover : AnimalPartRemover {

    private RaycastManager<RemovablePart> _raycastManager;
    private const KeyCode _interactKey = KeyCode.Mouse0;
    protected override void Start() {
        base.Start();
        _raycastManager = new RaycastManager<RemovablePart>(_playerCamera, _interactRange, true);
    }

    // Update is called once per frame
    void Update() {
        bool inputPressed = Input.GetKey(_interactKey);
        InteractionResult<RemovablePart> interactResult = _raycastManager.CheckRaycast(_interactKey, inputPressed);

        RemovablePart toRemove = interactResult.currentInteracted;
        RemovablePart previousRemovable = interactResult.previousInteracted; 
        if (interactResult.canCallBehaviour && toRemove != null && CanBeRemoved(toRemove)) {
            RemovePart(toRemove);
        }


        if (interactResult.enteredRange) {
            //went in range
        } else if (interactResult.exitedRange) {
            //went out of range
        }

        if (interactResult.interactedWithNewTarget) {
            toRemove.RemovalStarted();
        }

        if (interactResult.abandonedPreviousTarget) {
            previousRemovable.RemovalStopped();
        }

        if (interactResult.enteredRange && toRemove != null && CanBeRemoved(toRemove)) {
            ThrowInRangeEvent();
        } else if (interactResult.exitedRange /*&& interactResult.previousInteracted != null && CanBeRemoved(interactResult.previousInteracted)*/) {
            ThrowOutOfRangeEvent();
        }


        /*if (rubResult.didRub) {
            if (previousRemovable != toRemove) {
                if (previousRemovable != null) {
                    previousRemovable.RemovalStopped();
                }
                toRemove.RemovalStarted();
            }
        } else if (previousRemovable != null) {
            previousRemovable.RemovalStopped();
        }*/

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
