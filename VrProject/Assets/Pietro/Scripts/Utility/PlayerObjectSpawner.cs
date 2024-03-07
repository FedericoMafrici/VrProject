using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectSpawner : MonoBehaviour {
    [SerializeField] GameObject _toSpawn;
    [SerializeField] Camera _playerCamera;
    private float _spawnDistance = 1f;
    private float forceMagitude = 2f;

    void Start() {
        if (_toSpawn == null) {
            Debug.LogWarning(transform.name + ": no reference to object to spawn");
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Vector3 spawnPosition = _playerCamera.transform.position;
            spawnPosition += _playerCamera.transform.forward * _spawnDistance;
            Vector3 forceDirection = _playerCamera.transform.forward * forceMagitude;
            Spawner.Spawn(_toSpawn, spawnPosition, forceDirection);
        }
    }
}
