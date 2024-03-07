using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectSpawner : MinigameCallback
{
    [SerializeField] private GameObject toSpawn;
    private Vector3 spawnPos;
    private int _numTotalTargets;
    private int _numSpawnedTargets = 0;
    public override void Init(TargetMinigame minigame) {
        _numTotalTargets = minigame.GetNumTotalTargets();
    }

    public override void ProgressCallback() {
        if (_numSpawnedTargets < _numTotalTargets) {
            Instantiate(toSpawn, spawnPos, Quaternion.identity);
        }
    }

    public override void SpawnTargetCallback(Target target, int numSpawnedTargets) {
        spawnPos = target.transform.position;
        _numSpawnedTargets = numSpawnedTargets;
    }

}
