using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovablesCounter : MonoBehaviour
{

    private int _yetToRemove;
    void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            _yetToRemove = 0;
            Transform child = transform.GetChild(i);
            RemovablePart remPart = child.GetComponent<RemovablePart>();
            if (remPart != null ) {
                _yetToRemove++;
                remPart.PartRemoved += OnPartRemoved;
            }
        }

        if (_yetToRemove == 0) {
            Debug.LogWarning(transform.name + " has no RemovablePart in its children");
        }
    }

    private void OnPartRemoved() {
        _yetToRemove--;
        if (_yetToRemove == 0 ) {
            //Do Something
        }
    }

}
