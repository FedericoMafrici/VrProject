using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachTargetQuest : Quest {
    [SerializeField] NPCMover _npcMover;
    [SerializeField] private string _description;

    protected override void Init() {
        if (_npcMover == null) {
            Debug.LogError(transform.name + " no NPC Mover set for Reach Target Quest");
        }
        base.Init();
    }

    private void TargetReached() {
        if (_state == QuestState.ACTIVE) {
            Progress();
            Complete();

            //unsubscribe from event
            _npcMover.NPCDestinationReachedEvent -= TargetReached;
        }
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        //subscribe to NPC target reached event
        _npcMover.NPCDestinationReachedEvent += TargetReached;

        if (_npcMover.IsTargetReached()) {
            TargetReached();
        }
    }

    public override string GetQuestDescription() {
        return _description;
    }

    public override bool AutoComplete() {
        ForceStart();
        AutoCompletePreCheck();
        TargetReached();
        AutoCompletePostCheck();
        return true;

    }
}
