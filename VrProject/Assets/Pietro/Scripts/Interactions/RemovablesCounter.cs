using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovablesCounter : MonoBehaviour
{
    private int _yetToRemoveNumber;
    void Start() {
        _yetToRemoveNumber = 0;
        for (int i = 0; i < transform.childCount; i++) {
            Transform child = transform.GetChild(i);
            RemovablePart remPart = child.GetComponent<RemovablePart>();
            if (remPart != null ) {
                _yetToRemoveNumber++;
                remPart.PartRemoved += OnPartRemoved;
            }
        }

        if (_yetToRemoveNumber == 0) {
            Debug.LogWarning(transform.name + " has no RemovablePart in its children");
        }
    }

    private void OnPartRemoved() {
        _yetToRemoveNumber--;
        if (_yetToRemoveNumber == 0 ) {
            //Do Something
            Debug.Log("All parts removed");
        }
    }

}
