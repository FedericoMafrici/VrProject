using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LookAtQuest : Quest {

    [SerializeField] private ClueID _target;
    [SerializeField] private string _description = "";

    protected override void OnQuestStart() {
        SetEventSubscription(true, _target);
        base.OnQuestStart();
    }

    private void OnLookEvent(object sender, ClueEventArgs args) {
        if (args.clueID == _target &&
            (!(sender is TargetMinigameActivator) || !(sender as TargetMinigameActivator).NeedsAdditionalItem())) {
            SetEventSubscription(false, _target);
            Progress();
            Complete();
        }
    }


    private void SetEventSubscription(bool subscribe, ClueID id) {
        switch(id) {
            case ClueID.PET:
                if (subscribe)
                    Petter.InPetRange += OnLookEvent;
                else
                    Petter.InPetRange -= OnLookEvent;
                break;
            case ClueID.WATER:
                if (subscribe)
                    WateringCan.InWateringRange += OnLookEvent;
                else
                    WateringCan.InWateringRange -= OnLookEvent;
                break;
            case ClueID.TARGET_MINIGAME:
                if (subscribe)
                    TargetMinigameActivator.InMinigameRange += OnLookEvent;
                else
                    TargetMinigameActivator.InMinigameRange -= OnLookEvent;
                break;
            case ClueID.HEAL:
            case ClueID.CLEAN:
                if (subscribe)
                    RubRemover.InRemovalRange += OnLookEvent;
                else
                    RubRemover.InRemovalRange -= OnLookEvent;
                break;
            case ClueID.SHEAR:
                if (subscribe)
                    ClickRemover.InRemovalRange += OnLookEvent;
                else 
                    ClickRemover.InRemovalRange -= OnLookEvent;
                break;
            case ClueID.FEED:
                if (subscribe)
                    AnimalFood.InFeedRange += OnLookEvent;
                else
                    AnimalFood.InFeedRange -= OnLookEvent;
                break;
            case ClueID.PLANT:
                if (subscribe)
                    Seed.InLandRange += OnLookEvent;
                else
                    Seed.InLandRange -= OnLookEvent;
                break;
            default:
                break;
        }
    }

    public override string GetQuestDescription() {
        return _description;
    }

    public override bool AutoComplete() {
        ForceStart();

        SetEventSubscription(false, _target);
        Progress();
        Complete();
        return true;
    }
}
