using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public void TriggerDialogueManager(Dialogue dialogue)
    {
        if (dialogueManager.animator.GetBool("IsOpen") == false)
        {
            dialogueManager.StartDialogue(dialogue);
        }
        else
        {
            dialogueManager.DisplayNextSentence();
        }
    }
}
