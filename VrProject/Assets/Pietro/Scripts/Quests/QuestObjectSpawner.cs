using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;



public class QuestObjectSpawner : QuestEventReceiver
{
    
    [SerializeField] private bool _spawnOnce = true; 
    [SerializeField] private GameObject _toSpawn; //reference to prefab to spawn
    [SerializeField] private bool _objectsShouldRegisterToQuest = false; //if true the spawnd object will call the quest "RegisterObject()" methods passing themselves as arguments
    [SerializeField] private List<Transform> _transformsData; //list of transforms to indicate properties of spawned objects
                                                              //will spawn an object for each transform in the list
    private bool _alreadySpawned = false;
    
    protected override void Start() {
        base.Start();

        if (_toSpawn == null) {
            Debug.LogError(transform.name + " QuestGameObjectSpawner has no object to spawn reference");
        }

        if (_transformsData== null) {
            Debug.LogError(transform.name + " QuestGameObjectSpawner has no list of transforms where to spawn objects");
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {

        //only spawn object if quest has already started or is not inactive, also check if objects should be spawned through CanSpawnMoreObjects() method
        if (CanSpawnMoreObjects() && quest.GetState() != QuestState.INACTIVE && quest.GetState() != QuestState.NOT_STARTED) {

            //iterate over transform data in order to determine transform properties of spawned objects
            foreach (Transform tr in _transformsData) {
                GameObject spawned = Spawner.Spawn(_toSpawn, tr.position, tr.rotation);
                if (_objectsShouldRegisterToQuest) {
                    quest.RegisterObject(spawned);
                }
            }

            _alreadySpawned = true;
        }

        //if objects do not need to spawn anymore or quest is completed unsubscribe from event
        if (!CanSpawnMoreObjects() || quest.GetState() == QuestState.COMPLETED) {
            SetEventSubscription(false, quest, eventType);
        }
    }

    private bool CanSpawnMoreObjects() {
        return (!_spawnOnce || !_alreadySpawned);
    }
}
