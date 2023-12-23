using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalPartRemover : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] RemovableType _targetType;
    [SerializeField] private float _interactDistance = 2f;

    private void Start() {
        if (_playerCamera == null) { 
            Debug.LogError("no player camera set for " + transform.name);
        }
    }
    // Update is called once per frame
    void Update() {
        RaycastHit hit;

        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _interactDistance)) {
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

    protected virtual void RemovePart(RemovablePart toRemove) {
       toRemove.Remove();
    }

    protected virtual bool CanBeRemoved(RemovablePart toRemove) {
        return (!toRemove.IsRemoved()) && (toRemove.GetRemovableType() == _targetType);
    }
}
