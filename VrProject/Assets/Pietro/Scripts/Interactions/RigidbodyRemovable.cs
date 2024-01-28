using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyRemovable : RemovablePart {
    private Rigidbody _rigidbody;
    private bool _isItem;
    private Item _itemComponent;
    [SerializeField] private bool _startRemoved = true;

    protected override void Start() {
        base.Start();
        if (!_startRemoved) {
            _itemComponent = GetComponent<Item>();
            _isItem = _itemComponent != null;

            if (_isItem && !_isRemoved) {
                _itemComponent.enabled = false;
            }

            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null) {
                Debug.LogError(transform.name + ": no Rigidbody assigned");
            }
            _rigidbody.isKinematic = true;
        } else {
            _isRemoved = true;
            this.enabled = false;
        }
    }

    public override void Remove() {
        if (!_isRemoved) {
            base.Remove();
            MakeNPCStopMoving(2.0f);
            _isRemoved = true;
            _rigidbody.isKinematic = false;
            _rigidbody.AddForce(transform.forward * 1f, ForceMode.Impulse);
            _rigidbody.AddForce(transform.up * .5f, ForceMode.Impulse);
            if (_isItem) {
                _itemComponent.enabled = true;
                enabled = false;
            } else {
                StartCoroutine(WaitBeforeDestroying());
            }
        }
    }

    private IEnumerator WaitBeforeDestroying() {
        float toWait = 0.0f;
        yield return new WaitForSeconds(toWait);
        yield return FadeOut();
    }
}
