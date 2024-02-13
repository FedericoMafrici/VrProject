using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTriggerQuest : Quest
{
    [SerializeField] bool _startOnInit = false;

    protected override void Init() {
        base.Init();
        if (_startOnInit) {
            StartQuest();
        }
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        Progress();
        Complete();
        Destroy(gameObject);
    }

    public override string GetQuestDescription() {
        return "You should not be seeing this";
    }
}
