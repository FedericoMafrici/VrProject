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

public class UI_QuestsList : QuestEventReceiver
{
    private int numOfQuests = 0;
    [SerializeField] private Animator animator;
    private GameObject uiElement;
    private List<QuestElement> elements;
    private List<TMP_Text> descriptions;
    private List<Image> checkboxes;
    private List<Image> checks;
    
    public bool isOpen;

    protected override void Awake()
    {
        base.Awake();
        Hide();
        uiElement = (GameObject)Resources.Load("Prefabs/Description");
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

            GameObject uiElem = Instantiate<GameObject>(uiElement, this.transform);
            descriptions.Insert(quest.GetInfo().orderNumber, transform.Find("Description").GetComponent<TMP_Text>());
            checkboxes.Insert(quest.GetInfo().orderNumber, transform.Find("Checkbox").GetComponent<Image>());
            checks.Insert(quest.GetInfo().orderNumber, transform.Find("Check").GetComponent<Image>());
            
            uiElem.GetComponent<TMP_Text>().text = elem.text;
            if(!elem.isCompleted)
                transform.Find("Check").gameObject.SetActive(false);

            Show();
            if (elem.orderNumber == 0)
                Open(true);
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
                Close();
                numOfQuests = 0;
                elements.Clear();
                descriptions.Clear();
                checkboxes.Clear();
                checks.Clear();
                Hide();
            }
        }
    }

    public void Open(bool isWaitingNecessary)
    {
        if(isWaitingNecessary)
            Thread.Sleep(1000);
        
        animator.SetBool("IsOpen", true);
        isOpen = true;
        
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].text = elements[i].text;
            descriptions[i].gameObject.SetActive(true);
            
            checkboxes[i].gameObject.SetActive(true);
            
            if(elements[i].isCompleted)
                checks[i].gameObject.SetActive(true);
        }

    }
    
    public void Close()
    {
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].gameObject.SetActive(false);
            checkboxes[i].gameObject.SetActive(false);
            checks[i].gameObject.SetActive(false);
        }
        
        animator.SetBool("IsOpen", false);
        isOpen = false;
    }

    private void Show()
    {
        GameObject.Find("Quests_UI").gameObject.SetActive(true);
    }

    private void Hide()
    {
        GameObject.Find("Quests_UI").gameObject.SetActive(false);
    }
}
