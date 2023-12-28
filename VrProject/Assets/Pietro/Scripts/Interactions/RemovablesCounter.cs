using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovablesCounter : MonoBehaviour {
    private int _yetToRemoveNumber;
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _toSpawn;
    [SerializeField] private Transform _spawnPoint;

    void Start() {
        if (_player== null) {
            Debug.LogError(transform.name + " has no reference to the player");
        }

        if (_spawnPoint == null) {
            _spawnPoint = transform;
        }

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

    private void OnPartRemoved(object sender, PartEventArgs args) {
        _yetToRemoveNumber--;
        if (_yetToRemoveNumber == 0 ) {
            AllPartsRemoved();
        }
    }

    protected virtual void AllPartsRemoved() {
        if (_toSpawn != null) {
            GameObject spawned = Instantiate(_toSpawn, _spawnPoint.position, Quaternion.identity);
            Rigidbody spawnedRigidbody = spawned.transform.GetComponent<Rigidbody>();
            if (spawnedRigidbody != null) {
                Vector3 directionToPlayer = (_player.position - spawned.transform.position).normalized;
                spawnedRigidbody.AddForce(new Vector3(directionToPlayer.x * 5f, Vector3.up.y * 2f, directionToPlayer.z * 5f), ForceMode.Impulse);
            }
        }
    }

}
