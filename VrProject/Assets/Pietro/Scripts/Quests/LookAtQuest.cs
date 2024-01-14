using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum LookAtID {
    NONE,
    PETTABLE
}

public class LookAtQuest : Quest {

    [SerializeField] private LookAtID _target;
    [SerializeField] private string _description = "";
    protected override void OnQuestStart() {
        SetEventSubscription(true, _target);
        base.OnQuestStart();
    }

    private void OnLookEvent() {
        SetEventSubscription(false, _target);
        Progress();
        Complete();
    }

    private void SetEventSubscription(bool subscribe, LookAtID id) {
        switch(id) {
            case LookAtID.PETTABLE:
                if (subscribe)
                    Petter.LookedAtPettable += OnLookEvent;
                else
                    Petter.LookedAtPettable -= OnLookEvent;
                break;
            default:
                break;
        }
    }

    public override string GetQuestDescription() {
        return _description;
    }
}
