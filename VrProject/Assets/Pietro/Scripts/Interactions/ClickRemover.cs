using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickRemover : AnimalPartRemover {
    private RaycastManager<RemovablePart> _raycastManager;
    private const KeyCode _interactKey = KeyCode.Mouse0;
    protected override void Start() {
        base.Start();
        _raycastManager = new RaycastManager<RemovablePart>(_playerCamera, _interactRange, false);
    }

    // Update is called once per frame
    void Update() {
        //bool didRemove = false;

        InteractionResult<RemovablePart> result = _raycastManager.CheckRaycast(_interactKey);

        /*
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _interactRange)) {
            Transform hitTransform = hit.transform;
            if (hitTransform != null) {
                toRemove = hitTransform.GetComponent<RemovablePart>();
                if (toRemove != null && CanBeRemoved(toRemove)) {
                    if (Input.GetMouseButton(0)) {
                        //didRemove= true;
                        RemovePart(toRemove);
                    } else {
                        toRemove = null;
                    }
                }
            }
        }
        */
        RemovablePart toRemove = result.currentInteracted;

        if (result.canCallBehaviour && toRemove != null && CanBeRemoved(toRemove)) {
            RemovePart(result.currentInteracted);
        }

        if (result.interactedWithNewTarget) {
            toRemove.RemovalStarted();
        }

        if (result.abandonedPreviousTarget) {
            result.previousInteracted.RemovalStopped();
        }

        if (result.enteredRange && toRemove != null && CanBeRemoved(toRemove)) {
            ThrowInRangeEvent();
        } else if (result.exitedRange /*&& result.previousInteracted!= null && CanBeRemoved(result.previousInteracted)*/) {
            ThrowOutOfRangeEvent();
        }

        /*
        if (toRemove != _previousRemoved) {
            if (_previousRemoved != null) {
                _previousRemoved.RemovalStopped();
            }

            if (toRemove != null) {
                toRemove.RemovalStarted();
            }

        }
        */
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
