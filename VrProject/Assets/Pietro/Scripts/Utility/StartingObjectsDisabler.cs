using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingObjectsDisabler : MonoBehaviour
{
    [SerializeField] List<GameObject> _toDisable;
    void Start() {
        StartCoroutine(DisableCoroutine());
    }

    IEnumerator DisableCoroutine() {
        yield return null;
        foreach (GameObject obj in _toDisable) {
            obj.SetActive(false);
        }
    }
}


