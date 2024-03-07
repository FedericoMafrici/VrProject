using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using Microsoft.Unity.VisualStudio.Editor;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image; // Usando una direttiva 'using static'
public class TropheyUI : QuestEventReceiver
{
        [Header("Immagini trofei ")]
        public int currTier=0;
        public GameObject Alert;
        [SerializeField] public Sprite[] Tiers =new Sprite[4];
        
       [SerializeField]  public string[] descrizione =new string[4];
        
        public Sprite CurrTropheyTier=null;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        currTier=0;
        CurrTropheyTier=Tiers[currTier];
    }

    // Update is called once per frame
  

    protected override void OnEventReceived(Quest quest,EventType eventType)
    {
            
            if(eventType==EventType.PROGRESS)
            {
            currTier++;
            CurrTropheyTier=Tiers[currTier];
            Image AlertImage= Alert.GetComponent<Image>();
            if(AlertImage!=null){
            AlertImage.sprite=CurrTropheyTier;
            NewTropheyAlert alertmessage= Alert.GetComponent<NewTropheyAlert>();
            if(alertmessage!=null)
            {
            alertmessage.Show();
            }
            }
              if(currTier==3)
              {
                SetEventSubscription(false,quest,EventType.PROGRESS); 
              }
              
            }
            
    }

}
