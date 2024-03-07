using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BelowMapBarrier : MonoBehaviour {
    [SerializeField] Transform _respawnPoint;
    private void OnTriggerEnter(Collider other) {
        Debug.LogWarning("Collision detected");
        bool isPlayer = other.gameObject.layer == LayerMask.NameToLayer("Player");
        Transform toReposition;
        toReposition = other.transform;

        if (isPlayer) {
            toReposition.GetComponent<FirstPersonController>().enabled= false;
            //toReposition.GetComponent<StarterAssetsInputs>().enabled = false;
        }

        Debug.LogWarning("Repositioning: " + toReposition);
        

        toReposition.position = _respawnPoint.position;
        Rigidbody rigidbody;

        if (isPlayer) {
            rigidbody = toReposition.GetComponentInChildren<Rigidbody>();
        } else {
            rigidbody = toReposition.GetComponent<Rigidbody>();
        }

        StartCoroutine(DeleteForcesCoroutine(rigidbody));
        if (isPlayer) {
            StartCoroutine(RestartFPC(toReposition.GetComponent<FirstPersonController>()));
            //toReposition.GetComponent<StarterAssetsInputs>().enabled = true;
        }
    }

    IEnumerator DeleteForcesCoroutine(Rigidbody rigidbody) {
        if (rigidbody != null) {
            rigidbody.isKinematic = true;
            yield return null;
            rigidbody.isKinematic = false;
        }
    }

    IEnumerator RestartFPC(FirstPersonController controller) {
        yield return null;
        controller.enabled= true;
    }

}
