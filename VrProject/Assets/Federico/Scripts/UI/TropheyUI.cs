using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropheyUI : QuestEventReceiver
{
        [Header("Immagini trofei ")]
        private int currTier=0;
        [SerializeField] public Sprite Tier0; 
        [SerializeField] public Sprite Tier1;
        [SerializeField] public Sprite Tier2;
        [SerializeField] public Sprite Tier3;

        public Sprite CurrTropheyTier=null;
    // Start is called before the first frame update
    void Start()
    {
        currTier=0;
        CurrTropheyTier=Tier0;
    }

    // Update is called once per frame
  

    protected override void OnEventReceived(Quest quest,EventType eventType)
    {
            
            if(eventType==EventType.PROGRESS)
            {
              if(currTier==3)
              {
                SetEventSubscription(false,quest,EventType.PROGRESS); 
              }
              
            }
            
    }

}
