using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class QuestListD : MonoBehaviour
{
    
    private GameObject uiElement;
    // Start is called before the first frame update
    void Start()
    {
        uiElement = (GameObject) Resources.Load("Prefabs/Description");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddMission(Quest quest)
    {
           GameObject uiElem=Instantiate(uiElement,this.transform);
       //missionsList.Add(uiElem);
        uiElem.GetComponent<TMP_Text>().text = quest.GetQuestDescription();
       if(!(quest.GetState()==QuestState.COMPLETED)) {
            uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
       }
    }

public void deleteMissions()
{
      for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).tag == "QuestsUI")
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                        Destroy(transform.GetChild(i).gameObject);
                        
                    }
                }
                
}

}