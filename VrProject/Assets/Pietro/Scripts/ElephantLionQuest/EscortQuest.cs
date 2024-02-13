using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscortQuest : Quest {
    [SerializeField] private NPCMover _toEscort;
    [SerializeField] private Transform _destination;
    [SerializeField] private float _requiredDistance = 5f; //when object to escort reaches this distance from the destination the quest completes
    [SerializeField] private string _activeDescription;
    [SerializeField] private string _completeDescription;

    private Action _updateFunction = () => { } ;

    protected override void Init() {
        if (_toEscort == null) {
            Debug.LogError(transform.name + ": no NPC to escort was set for Escort Quest");
        }

        if (_destination == null) {
            Debug.LogError(transform.name + ": no destination was set for Escort Quest");
        }

        base.Init();
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        //change behaviour of update method
        _updateFunction = () => {
            if (Vector3.Distance(_toEscort.transform.position, _destination.position) <= _requiredDistance) {
                DestinationReached();
            }
        };

    }

    private void Update() {
        _updateFunction();
    }

    private void DestinationReached() {

        if (_state == QuestState.ACTIVE) {
            //reset behaviour of update method
            _updateFunction = () => { };

            Progress();
            Complete();
        }
    }

    public override string GetQuestDescription() {
        if (_state == QuestState.COMPLETED) {
            return _completeDescription;
        } else {
            return _activeDescription;
        }
    }

    public override bool AutoComplete() {
        ForceStart();
        DestinationReached();
        return true;
    }
}
