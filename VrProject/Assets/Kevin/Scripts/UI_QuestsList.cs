using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestsList : QuestEventReceiver {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject questsUI;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text hint;

    private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();
    private Dictionary<string, GameObject> elements = new Dictionary<string, GameObject>();

    public bool isOpen;

    protected override void Awake() {
        base.Awake();
        Close();
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        Debug.LogWarning("Received " + eventType + " from " + quest.name);
        if (eventType == EventType.AREA_ENTER) {
            
            allQuests.Add(quest.name, quest);

            if (!elements.ContainsKey(quest.name)) {
                GameObject uiElem = Instantiate((GameObject) Resources.Load("Prefabs/Description"), this.transform);
                elements.Add(quest.name, uiElem);

                uiElem.GetComponent<TMP_Text>().text = quest.GetQuestDescription();
                if (quest.GetState() != QuestState.COMPLETED)
                    uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
                else
                    uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
            }

        } else if (eventType == EventType.COMPLETE) {
            
            if (allQuests.ContainsKey(quest.name))
            {
                allQuests[quest.name] = quest;
            }
            
            if (elements.ContainsKey(quest.name))
            {
                GameObject uiElem = elements[quest.name];
                uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
            }
            
        } else if (eventType == EventType.PROGRESS) {
            
            if (allQuests.ContainsKey(quest.name))
            {
                allQuests[quest.name] = quest;
            }
            
            if (elements.ContainsKey(quest.name))
            {
                GameObject uiElem = elements[quest.name];
                uiElem.GetComponent<TMP_Text>().text = quest.GetQuestDescription();
            }

        } else if (eventType == EventType.DELETED) {
            
            allQuests.Remove(quest.name);
            if (elements.ContainsKey(quest.name))
            {
                Destroy(elements[quest.name]);
                elements.Remove(quest.name);
            }

        } else if (eventType == EventType.AREA_EXIT) {

            if (elements.ContainsKey(quest.name)) {
                Destroy(elements[quest.name]);
                elements.Remove(quest.name);
            }
            
        }
        
        if (elements.Count == 0) {
            Close();
            Hide();
        }
        else if (elements.Count == 1)
        {
            Open();
            Show();
        }

    }

    public void Open() {
        if (!animator.GetBool("IsOpen"))
        {
            animator.SetBool("IsOpen", true); 
            isOpen = true;

            hint.text = "Premi H per nascondere";
        }
        StartCoroutine(ShowText());
    }

    public void Close() {
        if (animator.GetBool("IsOpen"))
        {
            animator.SetBool("IsOpen", false);
            isOpen = false;

            hint.text = "Premi H per espandere";
        }
        StartCoroutine(HideText());
    }

    IEnumerator ShowText() {
        yield return new WaitForSeconds(0.65f);

        title.gameObject.SetActive(true);
        foreach (String name in elements.Keys)
        {
            elements[name].SetActive(true);
        }
    }

    IEnumerator HideText() {
        yield return new WaitForSeconds(0.35f);

        title.gameObject.SetActive(false);
        foreach (String name in elements.Keys)
        {
            elements[name].SetActive(false);
        }
    }

    public void Show() {
        questsUI.gameObject.SetActive(true);
        hint.gameObject.SetActive(true);
    }

    public void Hide() {
        questsUI.gameObject.SetActive(false);
        hint.gameObject.SetActive(false);
    }
}
