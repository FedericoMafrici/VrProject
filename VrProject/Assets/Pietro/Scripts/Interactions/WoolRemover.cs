using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoolRemover : AnimalPartRemover {
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        _targetType = RemovableType.WOOL;
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _interactRange)) {
            Transform hitTransform = hit.transform;
            if (hitTransform != null) {
                RemovablePart toRemove;
                toRemove = hitTransform.GetComponent<RemovablePart>();
                if (toRemove != null && CanBeRemoved(toRemove)) {
                    if (Input.GetMouseButton(0)) {
                        RemovePart(toRemove);
                    }
                }
            }
        }
    }
}
