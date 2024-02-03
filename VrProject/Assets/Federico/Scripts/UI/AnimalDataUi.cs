using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalDataUi : QuestEventReceiver
{
    [Header("The Animal which the quest is related to")]
  [SerializeField] public  Animal.AnimalName _animal;
/*
    [Header("Missions Related to the animals")]
    [SerializeField] private List<Quest> AnimalsQuest; 
*/
    // ENUM ANIMAL 
    [Header("Description of the Animals (divided in sentences)")]
    // Descrizione degli animali    
    public string[] description = new string[3];
    //Descrizione delle missioni degli animali 
    public int index=0;
    // immagine da visualizzare
    public Sprite AnimalSprite;
    public   HashSet<Quest> quests = new HashSet<Quest>();
    
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
                if(index<3)
                {
                    index++;
                }
                //disicrivi dagli eventi(?) se mi iscrivo o no, la quest
                SetEventSubscription(false,quest,EventType.COMPLETE);
                SetEventSubscription(false,quest,EventType.PROGRESS);
            }
    }

}