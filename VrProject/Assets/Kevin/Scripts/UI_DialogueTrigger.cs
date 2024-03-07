using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueManager dialogueManager;
    private bool dialogueOpen = false;
    bool checkInputs = false;
    Coroutine _activateInputsCoroutine = null;

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
        if (checkInputs && dialogueManager.animator.GetBool("IsOpen") && Input.GetKeyDown(KeyCode.E) && InputManager.DialoguesAreEnabled()) {
            TriggerDialogueManager();
        } else if (!checkInputs && dialogueManager.animator.GetBool("IsOpen")) {
            if (_activateInputsCoroutine == null) {
                _activateInputsCoroutine = StartCoroutine(ActivateDialogueInputs());
            }
        } else if (checkInputs && !dialogueManager.animator.GetBool("IsOpen")) {
            checkInputs = false;
        }
    }

    IEnumerator ActivateDialogueInputs() {
        yield return null;
        yield return null;
        checkInputs= true;
        _activateInputsCoroutine = null;
    }

    /*
    public void ForceEndCurrentDialogue() {
        dialogueManager.EndDialogue();
    }
    */
}
