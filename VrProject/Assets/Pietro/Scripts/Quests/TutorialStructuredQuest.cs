using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
struct TutorialStep {
    public bool ShowIndicatorsOnStart;
    public Quest quest;
    public Dialogue endStepDialogue;
}

public class TutorialStructuredQuest : AbstractStructuredQuest {
    [SerializeField] private Dialogue _startingDialogue;
    [SerializeField] private List<TutorialStep> _tutorialSteps;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private Dialogue _latestDialogue = null;

    protected override void Init() {
        if (_startingDialogue == null) {
            Debug.LogWarning(transform.name + ": no starting dialogue");
        }

        if (_tutorialSteps == null) {
            Debug.LogError(transform.name + ": no dialogue list set for tutorial ");
        } else {
            int nullDialogues = 0;
            foreach (TutorialStep step in _tutorialSteps) {
                if (step.quest == null) {
                    Debug.LogError(transform.name + " found null quest in step list");
                } else {
                    if (step.endStepDialogue == null) {
                        nullDialogues++;
                    }
                    _steps.Add(step.quest);
                }
            }
            if (nullDialogues > 0) {
                Debug.LogWarning(transform.name + " found " + nullDialogues + " null dialogues in step list");
            }
            //_tutorial.QuestStarted += OnTutorialStarted;
            //_tutorial.StepCompleted += OnStepCompleted;
        }

        base.Init();
    }

    /*
    void Update() {
        
            if (_latestDialogue != null) {
                //check if current dialogue is still active
                if (_dialogueTrigger.dialogueManager.animator.GetBool("IsOpen") == false) {

                    //if latest dialogue is not active anymore set its reference to null
                    _latestDialogue = null;

                } else if (Input.GetKeyDown(KeyCode.P)) {
                    //if current dialogue is still active and player presses input then advance the dialogue 
                    _dialogueTrigger.TriggerDialogueManager();
                }
            }
        
    }
    */

    private void UpdateActiveDialogue(Dialogue newDialogue) {
        if ((newDialogue != null) && (newDialogue != _latestDialogue) && newDialogue.sentences.Count() > 0) {
            /*
            if (_currentDialogue != null) {
                _dialogueTrigger.ForceEndCurrentDialogue();
            }
            */
            _latestDialogue = newDialogue;
            _dialogueTrigger.TriggerDialogueManager(_latestDialogue);
        }
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();
        Debug.Log(transform.name + ": quest started");
        //when the tutorial starts trigger the tutorial's starting dialogue
        UpdateActiveDialogue(_startingDialogue);
    }

    protected override void StartCurrStep() {
        base.StartCurrStep();
        if (_tutorialSteps[_curStepIdx].ShowIndicatorsOnStart) {
            _currentStep.ShowMarkers();
        }
    }

    protected override void OnStepCompleted(Quest step) {

        //whenever a step is completed trigger its corresponding end step dialogue
        UpdateActiveDialogue(_tutorialSteps[_curStepIdx].endStepDialogue);
        base.OnStepCompleted(step);

    }
}
