using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpAchievement : Quest {
    [SerializeField] private string _description;
    [SerializeField] private List<Item.ItemName> _targetItemNames;
    [SerializeField] private PlayerPickUpDrop _playerPickUp;
    [SerializeField] private List<int> _toReachValues = new List<int>();
    private HashSet<Item.ItemName> _alreadyPickedUpList = new HashSet<Item.ItemName>();
    private int _nPickedUp = 0;
    private int _valuesIndex = 0;

    protected override void Init() {
        if (_playerPickUp == null) {
            Debug.LogError(transform.name + ": no PlayerPickUpDrop reference set");
        }
        _playerPickUp.PickUpEvent += OnPickUp;

        _autoStart = true;

        base.Init();
    }

    private void OnPickUp(Item.ItemName itemName) {
        if (_state == QuestState.ACTIVE) {
            if (_targetItemNames.Contains(itemName) && !_alreadyPickedUpList.Contains(itemName)) {
                _alreadyPickedUpList.Add(itemName);
                AdvanceCounter();
                
            }
        }
    }

    private void AdvanceCounter() {
        _nPickedUp++;
        Debug.LogWarning("<color=cyan>" + this + ": progress for achievement, id: " + GetID() + "</color>");
        if (_nPickedUp >= _toReachValues[_valuesIndex]) {
            Progress();
            string color = _valuesIndex == 0 ? "green" : (_valuesIndex == 1 ? "yellow" : "red");
            Debug.LogWarning("<color=" + color + ">" + this + ": achievement unlocked" + " tier : " + _valuesIndex + 1 + ", id: " + GetID() + "</color>");
            if (_valuesIndex < _toReachValues.Count - 1) {
                _valuesIndex++;
            } else {
                Complete();
                _playerPickUp.PickUpEvent -= OnPickUp;
            }
        }
    }

    public override string GetQuestDescription() {
        return _description + " " + _nPickedUp + "/" + _toReachValues[_valuesIndex];
    }

    public override bool AutoComplete() {
        AutoCompletePreCheck();

        while (_state != QuestState.COMPLETED) {
            AdvanceCounter();
        }

        AutoCompletePostCheck();

        return true;
    }
}
