using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject quests_UI;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text hint;
    
    private int numOfQuests = 0;
    private GameObject uiElement;
    private Dictionary<int, QuestElement> elements = new Dictionary<int, QuestElement>();
    private Dictionary<int, TMP_Text> descriptions = new Dictionary<int, TMP_Text>();
    private Dictionary<int, Image> checkboxes = new Dictionary<int, Image>();
    private Dictionary<int, Image> checks = new Dictionary<int, Image>();
    
    public bool isOpen;

    protected override void Awake()
    {
        base.Awake();
        Hide();
        uiElement = (GameObject) Resources.Load("Prefabs/Description");
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
            elements.Add(quest.GetInfo().orderNumber, elem);

            GameObject uiElem = Instantiate(uiElement, this.transform);
            descriptions.Add(quest.GetInfo().orderNumber, uiElem.GetComponent<TMP_Text>());
            checkboxes.Add(quest.GetInfo().orderNumber, uiElem.transform.GetChild(0).GetComponent<Image>());
            checks.Add(quest.GetInfo().orderNumber, uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>());
            
            uiElem.GetComponent<TMP_Text>().text = elem.text;
            if(!elem.isCompleted)
                uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(false);

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
        
        else if (eventType == EventType.PROGRESS)
        {
            descriptions[quest.GetInfo().orderNumber].text = quest.GetQuestDescription();
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
                for (int i = 0; i < transform.childCount; i++)
                {
                    if(transform.GetChild(i).tag == "QuestsUI")
                        Destroy(transform.GetChild(i));
                }
                Hide();
            }
        }
    }

    public void Open(bool isWaitingNecessary)
    {
        // if(isWaitingNecessary)
        //     Thread.Sleep(1000);
        
        animator.SetBool("IsOpen", true);
        isOpen = true;
        
        hint.text = "Premi H per nascondere";

        ShowText();
    }
    
    public void Close()
    {
        animator.SetBool("IsOpen", false);
        isOpen = false;

        hint.text = "Premi H per espandere";
        
        HideText();
    }

    void ShowText()
    {
        StartCoroutine(ShowTextCoroutine());
    }

    IEnumerator ShowTextCoroutine()
    {
        yield return StartCoroutine(ShowWait());
        
        title.gameObject.SetActive(true);
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].text = elements[i].text;
            descriptions[i].gameObject.SetActive(true);
            checkboxes[i].gameObject.SetActive(true);
            if(elements[i].isCompleted)
                checks[i].gameObject.SetActive(true);
        }
    }

    IEnumerator ShowWait()
    {
        yield return new WaitForSeconds(0.65f);
    }

    void HideText()
    {
        StartCoroutine(HideTextCoroutine());
    }

    IEnumerator HideTextCoroutine()
    {
        yield return HideWait();
        
        title.gameObject.SetActive(false);
        for (int i = 0; i < numOfQuests; i++)
        {
            descriptions[i].gameObject.SetActive(false);
            checkboxes[i].gameObject.SetActive(false);
            checks[i].gameObject.SetActive(false);
        }
    }
    
    IEnumerator HideWait()
    {
        yield return new WaitForSeconds(0.35f);
    }

    private void Show()
    {
        quests_UI.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        hint.gameObject.SetActive(true);
    }

    private void Hide()
    {
        quests_UI.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        hint.gameObject.SetActive(false);
    }
}
