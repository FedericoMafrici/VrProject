using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Unity.VisualScripting.Member;

public abstract class AbstractStructuredQuest : Quest {
    [SerializeField] protected string _questCompletedDescription = "";

    protected List<Quest> _steps = new List<Quest>();
    protected int _curStepIdx = 0;
    protected Quest _currentStep = null;
    public event EventHandler<StepEventArgs> StepCompleted;

    protected override void Init() {
        //DisableCollider();

        if (_steps == null) {
            Debug.LogError(transform.name + " no step list set for StructuredQuest component");
        } else if (_steps.Count == 0) {
            Debug.LogError(transform.name + " step list in StructuredQuest component is empty");
        } else {
            foreach (Quest step in _steps) {
                if (step == null) {
                    Debug.LogError(transform.name + " found a \"null\" step in the step list");
                } else {
                    //step.DisableCollider();
                    step.SetIsStep(true);
                }
            }
            _currentStep = _steps[_curStepIdx];
            SubscribeToCurrStep();
        }
        base.Init();
    }

    protected override void OnTriggerEnter(Collider other) {
       //disable method
    }

    protected override void OnTriggerExit(Collider other) {
        //disable method
    }

    private void OnPlayerEnteredArea(Quest step) {
        //equivalent code of OnTriggerEnter
        PlayerEnteredQuestArea();
    }

    private void OnPlayerExitedArea(Quest step) {
        //equivalent code of OnTriggerExit
        PlayerExitedQuestArea();
    }

    public override void AreaCheck() {
        if (_currentStep != null && !_inArea) {
            _currentStep.AreaCheck();
        }
    }

    public override void Complete() {
        base.Complete();
        Debug.Log("Structured quest completed");
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();
        StartCurrStep();
    }

    public override void Deactivate() {
        base.Deactivate();
        _currentStep.Deactivate();
    }

    protected virtual void OnStepProgressed(Quest step) {
        Progress();
    }

    protected virtual void OnStepCompleted(Quest step) {
        Debug.LogWarning("Step completed: " + step.name);
        if (StepCompleted != null) {
            StepCompleted(this, new StepEventArgs(_currentStep, _curStepIdx));
        }

        if (CurrentStepIsFinal()) {
            Progress();
            Complete();
        } else {
            AdvanceStep();
            Progress();
        }
    }

    private bool CurrentStepIsFinal() {
        return (_curStepIdx+1) == _steps.Count;
    }

    private void AdvanceStep() {
        UnsubscribeFromCurrStep();
        _curStepIdx++;
        _currentStep = _steps[_curStepIdx];
        SubscribeToCurrStep();
        StartCurrStep();
    }

    protected virtual void StartCurrStep() {
        _currentStep.StartQuest();
    }

    public override string GetQuestDescription() {
        //if quest is not completed return intermediate step description
        //otherwise return the given quest completed description
        string result = (_state != QuestState.COMPLETED) ? _currentStep.GetQuestDescription() : _questCompletedDescription;
        return result;
    }

    public override BoxCollider GetQuestArea() {
      return _currentStep.GetQuestArea();
    }

    private void UnsubscribeFromCurrStep() {
        _currentStep.QuestProgressed -= OnStepProgressed;
        _currentStep.QuestCompleted -= OnStepCompleted;
        _currentStep.StepEnteredArea -= OnPlayerEnteredArea;
        _currentStep.StepExitedArea -= OnPlayerExitedArea;
        //_currentStep.DisableCollider();
    }

    private void SubscribeToCurrStep() {
        //_currentStep.EnableCollider();
        _currentStep.QuestProgressed += OnStepProgressed;
        _currentStep.QuestCompleted += OnStepCompleted;
        _currentStep.StepEnteredArea += OnPlayerEnteredArea;
        _currentStep.StepExitedArea += OnPlayerExitedArea;
    }

    public int GetNSteps() {
        return _steps.Count;
    }

    public Quest GetCurrentStep() {
        return _currentStep;
    }

    public override bool AutoComplete() {
        ForceStart();

        bool canComplete = true;
        while (canComplete && _state != QuestState.COMPLETED) {
            Quest _prevStep = _currentStep;

            canComplete = _currentStep.AutoComplete();

            if (!CurrentStepIsFinal() && (_prevStep == _currentStep)) {
                Debug.LogWarning(transform.name + ": " + this + ": quest did not advance step during auto complete");
                canComplete = false;
            }
        }

        return canComplete;
    }

}

public class StepEventArgs : EventArgs {
    public int stepIdx;
    public Quest step;
    public StepEventArgs(Quest s, int sIdx) {
        step = s;
        stepIdx = sIdx;
    }
}