using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetObjectSpawner : MinigameCallback
{
    [SerializeField] private GameObject toSpawn;
    private Vector3 spawnPos;

    public override void Init(TargetMinigame minigame) {

    }

    public override void ProgressCallback() {
        Instantiate(toSpawn, spawnPos, Quaternion.identity);
    }

    public override void SpawnTargetCallback(Target target, int numSpawnedTargets) {
        spawnPos = target.transform.position;
    }

}
