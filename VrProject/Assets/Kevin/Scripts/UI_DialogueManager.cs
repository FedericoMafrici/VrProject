using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences = new Queue<string>();
    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    
    public Animator animator;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;

    [SerializeField] private GameObject knob;
    [SerializeField] private GameObject hotbar;
    [SerializeField] private GameObject _questUI;

    Coroutine _sentenceCoroutine = null; //aggiunto da Pietro
    private bool dialogueOngoing;
    void Awake()
    {
       
    }

    public void EnqueDialogue(Dialogue dialogue) {
        if (!dialogueOngoing) {
            //Debug.LogWarning("Starting dialogue");
            //InputManager.DisableMenu();
            InputManager.DisableInteractions();
            StartDialogue(dialogue);
        } else {
            //Debug.LogWarning("Enqueueing dialogue");
            dialogueQueue.Enqueue(dialogue);
        }
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        dialogueOngoing = true;
        knob.SetActive(false);
        hotbar.SetActive(false);
        if (_questUI != null) {
            _questUI.SetActive(false);
        }
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;
        characterImage.sprite = dialogue.sprite;
        
        
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        if (_sentenceCoroutine != null) {
            StopCoroutine(_sentenceCoroutine);
        }
        _sentenceCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void EndDialogue()
    {
        dialogueOngoing = false;
        if (dialogueQueue.Count == 0) {
            //Debug.LogWarning("Ending dialogue");
            StopCoroutine(_sentenceCoroutine);
            _sentenceCoroutine = null;
            //InputManager.EnableMenu();
            InputManager.EnableInteractions();
            animator.SetBool("IsOpen", false);
            knob.SetActive(true);
            hotbar.SetActive(true);
            if (_questUI != null) {
                _questUI.SetActive(true);
            }
        } else {
            //Debug.LogWarning("Dequeueing and staring dialogue");
            StartDialogue(dialogueQueue.Dequeue());
        }
    }

    public void Update()
    {
    }
}

[System.Serializable]
public class Dialogue {
    public Sprite sprite;
    public string name;

    [TextArea(2, 3)]
    public string[] sentences;

}
