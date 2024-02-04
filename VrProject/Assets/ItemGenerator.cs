using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private bool _startWithItem;
    [SerializeField] private GameObject _toSpawnPrefab;
    private float _timerSeconds = 30;
    private float _delayVariance = 5;
    private bool _hasItem = false;
    private GameObject _spawned = null;

    void Start() {
        if (_toSpawnPrefab == null) {
            Debug.LogError(transform.name + ": no prefab to spawn");

        } else if (_toSpawnPrefab.GetComponent<Grabbable>() == null) {
            Debug.LogError(transform.name + ": this script only supports Grabbable GameObjects for now");
        }

        if (_startWithItem) {
            StartCoroutine(SpawnCoroutine(Random.Range(0, _delayVariance)));
        } else {
            WaitAndSpawn();
        }
    }

    void SpawnItem() {
        if (!_hasItem) {
            _hasItem = true;
            _spawned = Spawner.Spawn(_toSpawnPrefab, transform.position);
            _spawned.GetComponent<Grabbable>().GrabEvent += OnItemCollected;
        } else {
            Debug.LogWarning(transform.name + ": tried to spawn an item but already has one");
        }
    }

    void WaitAndSpawn() {
        float delay = _timerSeconds + Random.Range(-_delayVariance, _delayVariance);
        StartCoroutine(SpawnCoroutine(delay));
    }

    IEnumerator SpawnCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
        SpawnItem();
        yield return null;
    }

    private void OnItemCollected() {
        if (_hasItem) {
            //Debug.LogWarning(transform.parent.name + " Item collected");
            _hasItem = false;
            if (_spawned != null) {
                _spawned.GetComponent<Grabbable>().GrabEvent -= OnItemCollected;
            }
            _spawned = null;
            WaitAndSpawn();
        } else {
            Debug.LogWarning(transform.name + " item collected called but there should be no item");
        }
    }
}
