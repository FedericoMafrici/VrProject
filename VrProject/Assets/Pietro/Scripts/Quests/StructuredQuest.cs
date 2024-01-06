using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class StructuredQuest : AreaQuest {
    [SerializeField] private List<Quest> _steps;
    private int _curStepIdx = 0;
    private Quest _currentStep = null;

    protected override void Start() {
        base.Start();

        DisableCollider();

        if (_steps == null) {
            Debug.LogError(transform.name + " no step list set for StructuredQuest component");
        } else if (_steps.Count == 0) {
            Debug.LogError(transform.name + " step list in StructuredQuest component is empty");
        } else {
            foreach(Quest step in _steps) {
                if (step == null) {
                    Debug.LogError(transform.name + " found a \"null\" step in the step list");
                } else {
                   step.DisableCollider();
                }
            }
            _currentStep = _steps[_curStepIdx];
            SubscribeToCurrStep();

            if (!_isStep) {
                InitQuest();
            }
        }

    }

    protected override void OnTriggerEnter(Collider other) {
       //disable method
    }

    protected override void OnTriggerExit(Collider other) {
        //disable method
    }

    private void OnPlayerEnteredArea(object sender, EventArgs args) {
        //equivalent code of OnTriggerEnter
        PlayerEnteredQuestArea();
    }

    private void OnPlayerExitedArea(object sender, EventArgs args) {
        //equivalent code of OnTriggerExit
        PlayerExitedQuestArea();
    }

    public override void InitQuest() {
        _currentStep.InitQuest();
    }

    public override void Complete() {
        base.Complete();
        Debug.Log("Structured quest completed");
    }

    public override bool StartQuest() {
        bool didStart = base.StartQuest();

        if (didStart) {
            _currentStep.StartQuest();
        }

        return didStart;
    }

    public override void Deactivate() {
        base.Deactivate();
        _currentStep.Deactivate();
    }

    private void OnStepCompleted(object sender, EventArgs args) {
        if (CurrentStepIsFinal()) {
            Complete();
        } else {
            ProgressStep();
        }
    }

    private bool CurrentStepIsFinal() {
        return (_curStepIdx+1) == _steps.Count;
    }

    private void ProgressStep() {
        UnsubscribeFromCurrStep();
        _curStepIdx++;
        _currentStep = _steps[_curStepIdx];
        SubscribeToCurrStep();
        StartCurrStep();
    }

    private void StartCurrStep() {
        _currentStep.StartQuest();
    }

    public override string GetQuestDescription() {
        return _currentStep.GetQuestDescription();
    }

    public override BoxCollider GetQuestArea() {
      return _currentStep.GetQuestArea();
    }

    private void UnsubscribeFromCurrStep() {
        _currentStep.QuestCompleted -= OnStepCompleted;
        _currentStep.EnteredArea -= OnPlayerEnteredArea;
        _currentStep.ExitedArea -= OnPlayerExitedArea;
        _currentStep.DisableCollider();
    }

    private void SubscribeToCurrStep() {
        _currentStep.EnableCollider();
        _currentStep.QuestCompleted += OnStepCompleted;
        _currentStep.EnteredArea += OnPlayerEnteredArea;
        _currentStep.ExitedArea += OnPlayerExitedArea;
    }

}