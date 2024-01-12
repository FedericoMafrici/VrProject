using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingQuest : Quest {
    [SerializeField] private List<FoodEater> _foodEaters= new List<FoodEater>();
    [SerializeField] private Item.ItemName _targetFood;
    [SerializeField] private int _nToEat;
    [SerializeField] private string _description;
    private int _nEaten = 0;

    protected override void Start() {

        if (_foodEaters == null) {
            Debug.LogWarning(transform.name + " food eaters list is null");
        } else if (_foodEaters.Count == 0) {
            Debug.LogWarning(transform.name + " food eaters list is empty");
        }

        if (_nToEat < 1) {
            Debug.LogWarning(transform.name + ": count of food to eat was lower than 1, setting it to 1");
            _nToEat = 1;
        }

        if (_description == null) {
            Debug.LogWarning(transform.name + ": no description");
        }

        base.Start();
    }

    private void OnFoodEaten(object sender, EatEventArgs args) {
        if (_state == QuestState.ACTIVE && FoodIsOk(args.eaten)) {
            _nEaten++;
            Progress();

            if (_nEaten >= _nToEat) {

                // complete quest
                _nEaten = _nToEat;
                Complete();

                // unsubscribe from event
                foreach (FoodEater fe in _foodEaters) {
                    fe.EatEvent -= OnFoodEaten;
                }
            }
        }
    }

    protected override void OnQuestStart() {

        foreach (FoodEater fe in _foodEaters) {
            if (fe != null) {
                fe.EatEvent += OnFoodEaten;
            }
        }
    }

    public override void Complete() {
        base.Complete();
        foreach (FoodEater fe in _foodEaters) {
            if (fe != null) {
                fe.EatEvent -= OnFoodEaten;
            }
        }
        Debug.Log("Eating quest completed");
    }

    public override string GetQuestDescription() {
        string result = "";
        if (_description != null) {
            result += _description;
        }

        result += " " + _nEaten + "/" + _nToEat;
        return result;
    }

    public override void ShowMarkers() {

        foreach (FoodEater fe in _foodEaters) {
            QuestMarkerManager indicator = fe.GetComponent<QuestMarkerManager>();
            if (indicator != null) {
                indicator.AddShowRequest(GetID());
            }
        }

    }

    public override void HideMarkers() {
        foreach (FoodEater fe in _foodEaters) {
            QuestMarkerManager indicator = fe.GetComponent<QuestMarkerManager>();
            if (indicator != null) {
                indicator.RemoveShowRequest(GetID());
            }
        }

    }

    private bool FoodIsOk(ItemConsumable eaten) {
        return eaten != null && eaten.itemName == _targetFood;
    }
}
