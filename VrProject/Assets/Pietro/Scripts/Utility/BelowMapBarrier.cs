using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowMapBarrier : MonoBehaviour {
    [SerializeField] Transform _respawnPoint;
    private void OnTriggerEnter(Collider other) {
        other.transform.position = _respawnPoint.position;
        DeleteForcesCoroutine(other.GetComponent<Rigidbody>());
    }

    IEnumerator DeleteForcesCoroutine(Rigidbody rigidbody) {
        if (rigidbody != null) {
            rigidbody.isKinematic = true;
            yield return null;
            rigidbody.isKinematic = false;
        }
    }

}
