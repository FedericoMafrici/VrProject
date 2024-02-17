using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingObjectsDisabler : MonoBehaviour
{
    [SerializeField] List<GameObject> _toDisable;
    void Awake() {
        foreach (GameObject obj in _toDisable) {
            obj.SetActive(false);
        }
    }
}
