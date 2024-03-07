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
    public int index=3;
    
    public bool imageChanged=false;
    // immagine da visualizzare
    public Sprite AnimalTitleImage;
    public Sprite AnimalSprite;
    public Sprite currAnimalImage;
    public   HashSet<Quest> quests = new HashSet<Quest>();
    
     protected override void OnEventReceived(Quest quest,EventType eventType)
    {
            if(eventType==EventType.AREA_ENTER)
            {
               currAnimalImage=AnimalSprite;
               imageChanged=true;
              
            }
            else if(eventType==EventType.PROGRESS)
            {
               Debug.Log(quest.GetQuestDescription());
               //per aggiornare l'immagine
               if(!imageChanged)
               {
                currAnimalImage=AnimalSprite;
                imageChanged=true;
               }
              
            }
            else if(eventType==EventType.COMPLETE)
            {
                //new Information about the animal 
                Debug.Log(quest.GetQuestDescription());
                if(index<3)
                {
                    index++;
                    //per aggiornare l'immagine
                    if(!imageChanged)
                    {
                        currAnimalImage=AnimalSprite;
                        imageChanged=true;
                    }
                }
                //disicrivi dagli eventi(?) se mi iscrivo o no, la quest
                SetEventSubscription(false,quest,EventType.AREA_ENTER);
                SetEventSubscription(false,quest,EventType.COMPLETE);
                SetEventSubscription(false,quest,EventType.PROGRESS);
            }
    }

}
