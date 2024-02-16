using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObjectsParams : MonoBehaviour
{
    [SerializeField] ChangeScenario changeScenario;
    [SerializeField] List<Transform> _scenarioSpawnedParents;
    private void Awake() {
        SpawnedObjectsParentGetter.Init(_scenarioSpawnedParents, changeScenario);
    }
}
