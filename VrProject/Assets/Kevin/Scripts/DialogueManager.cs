using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Dialogue
{
    public Sprite sprite;
    public string name;
    
    [TextArea(2,3)]
    public string[] sentences;

}

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    
    public Animator animator;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public Image characterImage;

    [SerializeField] private GameObject knob;
    [SerializeField] private GameObject hotbar;
    

    void Start()
    {
        sentences = new Queue<string>();
    }
    
    public void StartDialogue(Dialogue dialogue)
    {
        InputManager.DisableInputs();
        knob.SetActive(false);
        hotbar.SetActive(false);
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
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
        InputManager.EnableInputs();
        animator.SetBool("IsOpen", false);
        knob.SetActive(true);
        hotbar.SetActive(true);

    }

    public void Update()
    {
    }
}
