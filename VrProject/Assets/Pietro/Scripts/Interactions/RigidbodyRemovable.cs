using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRemovable : RemovablePart {
    private Rigidbody _rigidbody;

    protected override void Start() {
        base.Start();
        _rigidbody= GetComponent<Rigidbody>();
        if (_rigidbody == null) {
            Debug.LogError(transform.name + ": no Rigidbody assigned");
        }
        _rigidbody.isKinematic = true;
    }
    public override void Remove() {
        base.Remove();
        _rigidbody.isKinematic= false;
        StartCoroutine(WaitBeforeDestroying());
    }

    private IEnumerator WaitBeforeDestroying() {
        float toWait = 2.0f;
        yield return new WaitForSeconds(toWait);
        yield return FadeOut();
    }
}
