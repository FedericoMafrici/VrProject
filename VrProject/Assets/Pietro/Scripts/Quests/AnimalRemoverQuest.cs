using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class AnimalRemoverQuest : Quest {

    [SerializeField] private string _description;
    [SerializeField] private RemovableType _removableType;
    [SerializeField] int _totalAnimals; //number of animals that will need their removable parts removed in order to complete the quest
    [SerializeField] private List<Transform> _animalTransformsList;
    private HashSet<RemovablesCounter> _removablesCounterSet = new HashSet<RemovablesCounter>();
    private int _nCompletedAnimals = 0;

    protected override void Init() {

        if (_animalTransformsList == null) {
            Debug.LogError(transform.name + ": AnimalRemoverQuest has no list of Removable Counters");
        } else if (_animalTransformsList.Count == 0) {
            Debug.LogWarning(transform.name + ": AnimalRemoverQuest: list of Removable Counters is empty");
        }

        InitRemovablesSet(_animalTransformsList);

        if (_totalAnimals <= 0) {
            Debug.LogWarning(transform.name + ": AnimalRemoverQuest, amount of animals participating in the quest is lower than 1, setting it to 1");
            _totalAnimals = 1;
        }
        
        base.Init();
    }

    private void OnAnimalCleared(RemovablesCounter rc) {
        if (_state == QuestState.ACTIVE) {
            _nCompletedAnimals++;
            Progress();
            if (rc != null) {
                RemoveMarker(rc.transform);
                rc.EverythingRemovedEvent -= OnAnimalCleared;
            }
            if (_nCompletedAnimals >= _totalAnimals) {
                _totalAnimals = _nCompletedAnimals;
                SetAllSubscriptions(false);
                Complete();
            }
        }
        
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        //register to events
        foreach(RemovablesCounter rc in _removablesCounterSet) {

            //quest might complete during this phase, manage this case
            if (_state == QuestState.COMPLETED)
                break;
            if (rc.IsEverythingRemoved()) {
                OnAnimalCleared(rc);
            } else {
                rc.EverythingRemovedEvent += OnAnimalCleared;
            }
        }
    }

    private void SetAllSubscriptions(bool subscribe) {
        //if used to subscribe this does not detect if the animal has already been cleared
        foreach (RemovablesCounter rc in _removablesCounterSet) {
            if (subscribe)
                rc.EverythingRemovedEvent += OnAnimalCleared;
            else
                rc.EverythingRemovedEvent -= OnAnimalCleared;
        }
    }

    public override string GetQuestDescription() {
        return _description + " " + _nCompletedAnimals + "/" + _totalAnimals;
    }

    private void InitRemovablesSet(List<Transform> animals) {
        foreach (Transform animalTransform in animals) {
            RemovablesCounter rc = animalTransform.GetComponent<RemovablesCounter>();
            if (rc != null) {
                _removablesCounterSet.Add(rc);
            } else {
                Debug.LogWarning(transform.name + " AnimalRemoverQuest, RemovableCounter not found in " + animalTransform.name);
            }
        }
    }

    public override void ShowMarkers() {
        base.ShowMarkers();
        foreach (RemovablesCounter rc in _removablesCounterSet) {
            QuestMarkerManager markerManager = rc.transform.GetComponent<QuestMarkerManager>();
            if (markerManager != null) {
                markerManager.AddShowRequest(GetID());
            }
        }
    }

    public override void HideMarkers() {
        base.HideMarkers();
        foreach (RemovablesCounter rc in _removablesCounterSet) {
            QuestMarkerManager markerManager = rc.transform.GetComponent<QuestMarkerManager>();
            if (markerManager != null) {
                markerManager.RemoveShowRequest(GetID());
            }
        }
    }

    private void RemoveMarker(Transform animalTransform) {
        QuestMarkerManager markerManager = animalTransform.GetComponent<QuestMarkerManager>();
        if (markerManager != null) {
            markerManager.RemoveAssociatedQuest(GetID());
        }
    }
    public override bool AutoComplete() {
        ForceStart();

        while(_nCompletedAnimals < _totalAnimals) {
            OnAnimalCleared(null);
        }

        return true;
    }
}
