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
    public string name;
    public string text;
    public bool isCompleted;
}

public class UI_QuestsList : QuestEventReceiver {
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject quests_UI;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text hint;

    private int numOfQuests = 0;
    private GameObject uiElement;
    private Dictionary<string, QuestElement> elements = new Dictionary<string, QuestElement>();
    private Dictionary<string, TMP_Text> descriptions = new Dictionary<string, TMP_Text>();
    private Dictionary<string, Image> checkboxes = new Dictionary<string, Image>();
    private Dictionary<string, Image> checks = new Dictionary<string, Image>();

    public bool isOpen;

    protected override void Awake() {
        base.Awake();
        Hide();
        uiElement = (GameObject)Resources.Load("Prefabs/Description");
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        if (eventType == EventType.AREA_ENTER) {
            numOfQuests++;

            QuestElement elem;
            elem.name = quest.name;
            elem.text = quest.GetQuestDescription();
            elem.isCompleted = (quest.GetState() == QuestState.COMPLETED);
            elements.Add(elem.name, elem);

            GameObject uiElem = Instantiate(uiElement, this.transform);
            descriptions.Add(elem.name, uiElem.GetComponent<TMP_Text>());
            checkboxes.Add(elem.name, uiElem.transform.GetChild(0).GetComponent<Image>());
            checks.Add(elem.name, uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>());

            uiElem.GetComponent<TMP_Text>().text = elem.text;
            
            if (!elem.isCompleted)
                uiElem.transform.GetChild(0).GetChild(0).GetComponent<Image>().gameObject.SetActive(false);

            Show();
            if (numOfQuests == 1)
                Open(true);
        } else if (eventType == EventType.COMPLETE) {
            QuestElement elem = elements[quest.name];
            elem.isCompleted = true;
            elements[quest.name] = elem;
            checks[elem.name].gameObject.SetActive(true);

        } else if (eventType == EventType.PROGRESS) {

            descriptions[quest.name].text = quest.GetQuestDescription();

        } else if (eventType == EventType.AREA_EXIT)
        {
            numOfQuests--;
            if (numOfQuests == 0) {
                Close();
                numOfQuests = 0;
                elements.Clear();
                descriptions.Clear();
                checkboxes.Clear();
                checks.Clear();
                
                for (int i = 0; i < transform.childCount; i++) {
                    if (transform.GetChild(i).tag == "QuestsUI") {
                        transform.GetChild(i).gameObject.SetActive(false);
                        Destroy(transform.GetChild(i).gameObject);

                    }
                }
                Hide();
            }
        }
    }

    public void Open(bool isWaitingNecessary) {
        // if(isWaitingNecessary)
        //     Thread.Sleep(1000);

        animator.SetBool("IsOpen", true);
        isOpen = true;

        hint.text = "Premi H per nascondere";

        ShowText();
    }

    public void Close() {
        animator.SetBool("IsOpen", false);
        isOpen = false;

        hint.text = "Premi H per espandere";

        HideText();
    }

    void ShowText() {
        StartCoroutine(ShowTextCoroutine());
    }

    IEnumerator ShowTextCoroutine() {
        yield return new WaitForSeconds(0.65f);

        title.gameObject.SetActive(true);
        foreach (String str in elements.Keys)
        {
            descriptions[str].text = elements[str].text;
            descriptions[str].gameObject.SetActive(true);

            checkboxes[str].gameObject.SetActive(true);


            if (elements[str].isCompleted)
                checks[str].gameObject.SetActive(true);
        }
    }

    void HideText() {
        StartCoroutine(HideTextCoroutine());
    }

    IEnumerator HideTextCoroutine() {
        yield return new WaitForSeconds(0.35f);

        title.gameObject.SetActive(false);
        foreach (String str in elements.Keys)
        {
            descriptions[str].gameObject.SetActive(false);
            checkboxes[str].gameObject.SetActive(false);
            checks[str].gameObject.SetActive(false);
        }
    }

    private void Show() {
        quests_UI.gameObject.SetActive(true);
        title.gameObject.SetActive(true);
        hint.gameObject.SetActive(true);
    }

    private void Hide() {
        quests_UI.gameObject.SetActive(false);
        title.gameObject.SetActive(false);
        hint.gameObject.SetActive(false);
    }
}
