using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[Serializable] public struct QuestElement
{
    public int orderNumber;
    public string text;
    public bool isCompleted;
}

public class QuestsList : QuestEventReceiver
{
    static public int maxQuests = 5;
    private int numOfQuests = 0;
    
    public Animator animator;
    public List<QuestElement> elements;
    
    public List<TMP_Text> descriptions;
    public List<Image> checkboxes;
    public List<Image> checks;

    void Start()
    {
        for(int i = 0; i < maxQuests; i++){
            GameObject description = gameObject.transform.Find("Description" + i).gameObject;
            description.SetActive(false);
            descriptions.Insert(i, description.GetComponent<TMP_Text>());
            
            GameObject checkbox = gameObject.transform.Find("CheckBox" + i).gameObject;
            checkbox.SetActive(false);
            checkboxes.Insert(i, checkbox.GetComponent<Image>());
            
            GameObject check = gameObject.transform.Find("Checks" + i).gameObject;
            check.SetActive(false);
            checks.Insert(i, check.GetComponent<Image>());
        }
    }
    
    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.AREA_ENTER)
        {
            numOfQuests++;
            
            QuestElement elem;
            elem.orderNumber = quest.GetInfo().orderNumber;
            elem.text = quest.GetQuestDescription();
            elem.isCompleted = (quest.GetState() == QuestState.COMPLETED);
            elements.Insert(quest.GetInfo().orderNumber, elem);

            if (elem.orderNumber == 0)
            {
                Thread.Sleep(1000);
                Show();
            }
        }

        else if (eventType == EventType.COMPLETE)
        {
            QuestElement elem = elements[quest.GetInfo().orderNumber];
            elem.isCompleted = true;
            elements[quest.GetInfo().orderNumber] = elem;
            
            checks[quest.GetInfo().orderNumber].gameObject.SetActive(true);
        }
        
        else if (eventType == EventType.AREA_EXIT)
        {
            if (quest.GetInfo().orderNumber == 0)
            {
                Hide();
                elements.Clear();
            }
        }
    }

    void Show()
    {
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].text = elements[i].text;
            descriptions[i].gameObject.SetActive(true);
            
            checkboxes[i].gameObject.SetActive(true);
            
            if(elements[i].isCompleted)
                checks[i].gameObject.SetActive(true);
        }

        animator.SetBool("IsOpen", true);

    }
    
    void Hide()
    {
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].text = "";
            descriptions[i].gameObject.SetActive(false);
            
            checkboxes[i].gameObject.SetActive(false);
            
            if(elements[i].isCompleted)
                checks[i].gameObject.SetActive(false);
        }
        numOfQuests = 0;
        
        animator.SetBool("IsOpen", true);
    }
    
}
