using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventReceiverF : QuestEventReceiver
{
    protected override void OnEventReceived(Quest quest,EventType eventType)
    {
            if(eventType==EventType.PROGRESS)
            {
               Debug.Log(quest.GetQuestDescription());
            }
            else if(eventType==EventType.COMPLETE)
            {
                //new Information about the animal 
                Debug.Log(quest.GetInfo().newInformation);
                //disicrivi dagli eventi(?) se mi iscrivo o no, la quest
                SetEventSubscription(false,quest,EventType.COMPLETE);
                SetEventSubscription(false,quest,EventType.PROGRESS);
            }
    }
}
