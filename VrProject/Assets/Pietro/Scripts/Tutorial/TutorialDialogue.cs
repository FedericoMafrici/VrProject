using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialDialogue : MonoBehaviour {
    [SerializeField] private StructuredQuest _tutorial;
    [SerializeField] private Dialogue _startingDialogue;
    [SerializeField] private List<Dialogue> _endStepDialogues;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private Dialogue _currentDialogue = null;

    void Start() {
        if (_tutorial == null) {
            Debug.LogError(transform.name + ": no StructuredQuest set as tutorial ");
        }

        if (_startingDialogue== null) {
            Debug.LogWarning(transform.name + ": no starting dialogue");
        }

        if (_endStepDialogues == null) {
            Debug.LogError(transform.name + ": no dialogue list set for tutorial ");
        } else {
            if (_endStepDialogues.Count != _tutorial.GetNSteps() ) {
                Debug.LogWarning(transform.name + ": mismatch between number of dialogues for each step and number of steps in StructuredQuest");
            }
        }

        _tutorial.QuestStarted += OnTutorialStarted;
        _tutorial.StepCompleted += OnStepCompleted;
    }

    private void Update() {
        

        if (_currentDialogue != null) {
            //check if current dialogue is still active
            if (_dialogueTrigger.dialogueManager.animator.GetBool("IsOpen") == false) {
                //if current dialogue is not active anymore set its reference to null
                _currentDialogue = null;
            } else if (Input.GetKeyDown(KeyCode.P)) {
                //if current dialogue is still active and player presses input then advance the dialogue 
                _dialogueTrigger.TriggerDialogueManager(_currentDialogue);
            }
        }
    }

    private void UpdateActiveDialogue(Dialogue newDialogue) {
        if (newDialogue != _currentDialogue) {
            _dialogueTrigger.ForceEndCurrentDialogue();
            _currentDialogue = newDialogue;
            _dialogueTrigger.TriggerDialogueManager(_currentDialogue);
        }
    }

    private void OnTutorialStarted(object sender, EventArgs args) {
        //when the tutorial starts trigger the tutorial's starting dialogue

        Dialogue dialogue = _startingDialogue;
        if (dialogue != null) {
            UpdateActiveDialogue(dialogue);
        }
    }

    private void OnStepCompleted(object sender, StepEventArgs args) {
        //whenever a step is completed trigger its corresponding end step dialogue

        //first check if the list can access the given step position (avoids crashing if for some reason an invalid list index is given)
        if (_endStepDialogues.Count > args.stepIdx) {
            Dialogue dialogue = _endStepDialogues[args.stepIdx];

            //then check if the dialogue reference is not null
            if (dialogue != null) {
                //if everything's ok trigger the dialogue
                UpdateActiveDialogue(dialogue);
            }
        }
    }
}
