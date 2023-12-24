using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolRemover : AnimalPartRemover {
    RemovablePart _previousRemoved = null;
    protected override void Start() {
        base.Start();
        _targetType = RemovableType.WOOL;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        RemovablePart toRemove = null;
        bool didRemove = false;

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _interactRange)) {
            Transform hitTransform = hit.transform;
            if (hitTransform != null) {
                toRemove = hitTransform.GetComponent<RemovablePart>();
                if (toRemove != null && CanBeRemoved(toRemove)) {
                    if (Input.GetMouseButton(0)) {
                        didRemove= true;
                        RemovePart(toRemove);
                    } else {
                        toRemove = null;
                    }
                }
            }
        }

        if (toRemove != _previousRemoved) {
            if (_previousRemoved != null) {
                _previousRemoved.RemovalStopped();
            }

            if (toRemove != null) {
                toRemove.RemovalStarted();
            }

        }
        _previousRemoved = toRemove;
    }
}
