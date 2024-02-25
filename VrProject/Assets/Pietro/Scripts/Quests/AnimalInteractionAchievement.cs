using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum InteractionType {
    NONE,
    EAT,
    PET
}

public class AnimalInteractionAchievement : Quest
{
    [SerializeField] private string _description;
    [SerializeField] List<int> _toReachValues = new List<int>();
    [SerializeField] private InteractionType _interactionType;
    [SerializeField] private bool _displayProgress = true;
    private int _valuesIndex = 0;

    //private int _nToComplete;
    private int _nInteracted = 0;

    protected override void Init() {

        switch(_interactionType) {
            case InteractionType.EAT:
                FoodEater.StaticEatEvent += OnEatEvent;
                break;
                case InteractionType.PET:
                Pettable.StaticBefriended += OnAnimalBefriended;
                break;
                default: break;
        }


        _toReachValues.Sort();

        _autoStart = true;
        base.Init();
    }

    private void OnAnimalBefriended(object sender, EventArgs args) {
        AdvanceCounter();
    }

    private void OnEatEvent(object sender, EatEventArgs args) {
        AdvanceCounter();
    }

    private void OnQuestCompleteEvent(Quest quest) {
        if (_state == QuestState.ACTIVE) {
            quest.QuestCompleted -= OnQuestCompleteEvent;
            AdvanceCounter();
        }
    }

    private void AdvanceCounter() {
        if (_state == QuestState.ACTIVE) {
            _nInteracted++;
            Debug.LogWarning("<color=cyan>" + this + ": progress for achievement, id: " + GetID() + "</color>");
            if (_nInteracted >= _toReachValues[_valuesIndex]) {
                Progress();
                string color = _valuesIndex == 0 ? "green" : (_valuesIndex == 1 ? "yellow" : "red");
                Debug.LogWarning("<color=" + color + ">" + this + ": achievement unlocked" + " tier : " + _valuesIndex + 1 + ", id: " + GetID() + "</color>");
                if (_valuesIndex < _toReachValues.Count - 1) {
                    _valuesIndex++;
                } else {
                    Complete();
                }
            }
        }
    }

    public override string GetQuestDescription() {
        if (_displayProgress) {
            return _description + " " + _nInteracted + "/" + _toReachValues[_valuesIndex];
        } else {
            return _description;
        }
    }
}
