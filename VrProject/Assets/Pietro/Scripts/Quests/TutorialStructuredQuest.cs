using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
struct TutorialStep {
    public Quest quest;
    public Dialogue endStepDialogue;
}

public class TutorialStructuredQuest : AbstractStructuredQuest {
    [SerializeField] private Dialogue _startingDialogue;
    [SerializeField] private List<TutorialStep> _tutorialSteps;
    [SerializeField] private DialogueTrigger _dialogueTrigger;
    private Dialogue _currentDialogue = null;
    private bool _checkDialogueBox = false;

    protected override void Start() {

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

        base.Start();
    }

    void Update() {
        //if (_checkDialogueBox) {
            if (_currentDialogue != null) {
                //check if current dialogue is still active
                if (_dialogueTrigger.dialogueManager.animator.GetBool("IsOpen") == false) {

                    //if current dialogue is not active anymore set its reference to null
                    //_currentDialogue = null;
                    if (_state == QuestState.COMPLETED) {
                        _checkDialogueBox = false;
                    }
                } else if (Input.GetKeyDown(KeyCode.P)) {
                    //if current dialogue is still active and player presses input then advance the dialogue 
                    _dialogueTrigger.TriggerDialogueManager(_currentDialogue);
                }
            }
        //}
    }

    private void UpdateActiveDialogue(Dialogue newDialogue) {
        if ((newDialogue != null) && (newDialogue != _currentDialogue)) {
            if (_currentDialogue != null) {
                _dialogueTrigger.ForceEndCurrentDialogue();
            }
            _currentDialogue = newDialogue;
            _dialogueTrigger.TriggerDialogueManager(_currentDialogue);
        }
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();
       

        //when the tutorial starts trigger the tutorial's starting dialogue
        _checkDialogueBox = true;
        UpdateActiveDialogue(_startingDialogue);
    }

    protected override void StartCurrStep() {
        base.StartCurrStep();
        if (_currentStep.GetID() == QuestID.TUTORIAL_PICK_UP_STEP) {
            _currentStep.ShowIndicators();
        }
    }

    protected override void OnStepCompleted(object sender, EventArgs args) {

        //whenever a step is completed trigger its corresponding end step dialogue
        UpdateActiveDialogue(_tutorialSteps[_curStepIdx].endStepDialogue);
        base.OnStepCompleted(sender, args);

    }
}
