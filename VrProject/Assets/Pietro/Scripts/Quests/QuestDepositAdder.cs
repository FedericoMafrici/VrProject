using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
struct ItemEntry {
    public Item.ItemName itemName;
    public int amount;
}

public class QuestDepositAdder : QuestEventReceiver {
    [SerializeField] private Deposit _deposit;
    [SerializeField] private List<ItemEntry> _itemsToAdd;
    [SerializeField] private bool _addOnce = true;
    private HashSet<ItemEntry> _itemsToAddSet;
    private bool _alreadyAdded = false;
    protected override void Start() {
        if (_deposit == null) {
            Debug.LogError(transform.name + ": QuestDepositAdder has no Deposit reference");
        }

        if (_itemsToAdd == null) {
            Debug.LogError(transform.name + ": QuestDepositAdder has no Item names list");
        } else if (_itemsToAdd.Count == 0) {
            Debug.LogError(transform.name + ": QuestDepositAdder Item names list is empty");
        }

        _itemsToAddSet = _itemsToAdd.ToHashSet();

        base.Start();
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        if (!_addOnce || !_alreadyAdded) {
            foreach (ItemEntry entry in _itemsToAddSet) {
                _deposit.AddItem(entry.itemName, entry.amount);
            }
            _alreadyAdded= true;
        }

        if (_addOnce) {
            SetEventSubscription(false, quest, eventType);
        }
    }

}
