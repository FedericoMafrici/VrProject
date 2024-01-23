using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustTriggerQuest : Quest
{
    //A simple "quest" used to trigger some events when it starts

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
