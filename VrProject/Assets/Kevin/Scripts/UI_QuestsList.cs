using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.Search;
#endif
using UnityEngine;
using UnityEngine.UI;

public class UI_QuestsList : QuestEventReceiver {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject questsUI;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text hint;
    [SerializeField] private Transform _layoutGroupTransform;

    private Dictionary<string, Quest> allQuests = new Dictionary<string, Quest>();
    private Dictionary<string, GameObject> elements = new Dictionary<string, GameObject>();
    
    public bool commandLocked;
    public bool isOpen;

    private bool _isInactive = false; //aggiunto da pietro;

    protected override void Awake() {
        base.Awake();
        Close();
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        int prevElementsCount = elements.Count;
        //Debug.LogWarning("Received " + eventType + " from " + quest.name);
        if (eventType == EventType.AREA_ENTER) {

            if (!allQuests.ContainsKey(quest.name)) {
                allQuests.Add(quest.name, quest);
            }

            if (!elements.ContainsKey(quest.name)) {
                GameObject uiElem = Instantiate((GameObject) Resources.Load("Prefabs/Description"), this.transform);

                if (_layoutGroupTransform != null) {
                    uiElem.transform.SetParent(_layoutGroupTransform, false);
                }

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
        
        if (prevElementsCount > 0 && elements.Count == 0) {
            Close();
            Hide();
        }
        else if (prevElementsCount == 0 && elements.Count > 0)
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
        yield return new WaitForSeconds(0.5f);
        if (_isInactive || !isOpen) {
            commandLocked = false;
            yield break;
        }

        if (elements.Keys.Count > 0) {
            title.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(0.2f);
        if (_isInactive || !isOpen) {
            commandLocked = false;
            yield break;
        }
        foreach (String name in elements.Keys)
        {
            elements[name].SetActive(true);
            yield return new WaitForSeconds(0.1f);
            if (_isInactive || !isOpen) {
                commandLocked = false;
                yield break;
            }
        }
        commandLocked = false;
    }

    IEnumerator HideText() {
        yield return new WaitForSeconds(0.2f);
        if (_isInactive || isOpen) {
            if (elements.Keys.Count == 0) {
                title.gameObject.SetActive(false);
            }
            commandLocked = false;
            yield break;
        }

        foreach (String name in elements.Keys.Reverse())
        {
            elements[name].SetActive(false);
            yield return new WaitForSeconds(0.1f);
            if (_isInactive || isOpen) {
                if (elements.Keys.Count == 0) {
                    title.gameObject.SetActive(false);
                }
                commandLocked = false;
                yield break;
            }
        }
        yield return new WaitForSeconds(0.1f);
        if (_isInactive || isOpen) {
            if (elements.Keys.Count== 0) {
                title.gameObject.SetActive(false);
            }
            commandLocked = false;
            yield break;
        }
        title.gameObject.SetActive(false);
        commandLocked = false;
    }

    public void Show() {
        _isInactive = false;
        questsUI.gameObject.SetActive(true);
        hint.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _isInactive = true;
        StartCoroutine(HideAfterAWhile());
    }
    
    IEnumerator HideAfterAWhile()
    {
        yield return new WaitForSeconds(1.5f);
        if (_isInactive) {
            questsUI.gameObject.SetActive(false);
            hint.gameObject.SetActive(false);
        }
    }
}
