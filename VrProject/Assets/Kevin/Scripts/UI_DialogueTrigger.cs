using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    private bool dialogueOpen = false;

    public void TriggerDialogueManager(Dialogue dialogue = null)
    {
        if (dialogue != null) {
            dialogueManager.EnqueDialogue(dialogue);
        }
        else {
            dialogueManager.DisplayNextSentence();
        }
    }

    private void Update() {
        if (dialogueManager.animator.GetBool("IsOpen") && Input.GetKeyDown(KeyCode.P)) {
            TriggerDialogueManager();
        }
    }

    /*
    public void ForceEndCurrentDialogue() {
        dialogueManager.EndDialogue();
    }
    */
}
