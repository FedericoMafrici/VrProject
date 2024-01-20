using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovablesCounter : MonoBehaviour {
    private int _yetToRemoveNumber;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _toSpawn;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _removablesParent;
    private bool _everythingRemoved = false;

    public event Action<RemovablesCounter> EverythingRemovedEvent;

    void Start() {
        if (_toSpawn != null && _player== null) {
            Debug.LogError(transform.name + " has no reference to the player");
        }

        if (_removablesParent == null) {
            _removablesParent = transform;
        }

        SubscribeToRemovables(_removablesParent);

        if (_spawnPoint == null) {
            _spawnPoint = transform;
        }

    }

    private void OnPartRemoved(object sender, PartEventArgs args) {
        _yetToRemoveNumber--;
        if (_yetToRemoveNumber == 0 ) {
            AllPartsRemoved();
        }
    }

    protected virtual void AllPartsRemoved() {
        _everythingRemoved= true;
        Vector3 forceDirection = (_player.position - _spawnPoint.position).normalized;
        forceDirection.x *= 4f;
        forceDirection.y = Vector3.up.y * 2f; 
        forceDirection.z *= 4f;
        Spawner.Spawn(_toSpawn, _spawnPoint.position, forceDirection);
        if (EverythingRemovedEvent!= null) {
            EverythingRemovedEvent(this);
        }
    }

    public bool IsEverythingRemoved() { return _everythingRemoved; }

    private void SubscribeToRemovables(Transform parentTransform) {
        _yetToRemoveNumber = 0;
        for (int i = 0; i < parentTransform.childCount; i++) {
            Transform child = parentTransform.GetChild(i);
            RemovablePart remPart = child.GetComponent<RemovablePart>();
            if (remPart != null) {
                _yetToRemoveNumber++;
                remPart.PartRemoved += OnPartRemoved;
            }
        }

        if (_yetToRemoveNumber == 0) {
            Debug.LogWarning(parentTransform.name + " has no RemovablePart in its children");
        }
    }

}
