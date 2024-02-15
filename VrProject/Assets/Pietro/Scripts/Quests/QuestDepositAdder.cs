using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

[Serializable]
struct ItemEntry {
    public Item.ItemName itemName;
    public int amount;
}

public class QuestDepositAdder : QuestEventReceiver {
    [SerializeField] private Deposit _deposit;
    [SerializeField] private List<ItemEntry> _itemsToAdd;
    [SerializeField] private bool _addOnce = true;
    [SerializeField] private bool _spreadOnUpdates = false;
    private HashSet<ItemEntry> _itemsToAddSet;
    private bool _alreadyAdded = false;
    private int _currentToSpawn = 0;

    protected override void Awake() {
        if (_deposit == null) {
            Debug.LogError(transform.name + ": QuestDepositAdder has no Deposit reference");
        }

        if (_itemsToAdd == null) {
            Debug.LogError(transform.name + ": QuestDepositAdder has no Item names list");
        } else if (_itemsToAdd.Count == 0) {
            Debug.LogError(transform.name + ": QuestDepositAdder Item names list is empty");
        }

        _itemsToAddSet = new HashSet<ItemEntry>();
        foreach (ItemEntry entry in _itemsToAdd) {
            _itemsToAddSet.Add(entry);
        }


        base.Awake();
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        Debug.LogWarning("Complete event received");
        if (!_addOnce || !_alreadyAdded) {

            if (_spreadOnUpdates) {
                foreach (ItemEntry entry in _itemsToAddSet) {
                    _deposit.AddItem(entry.itemName, entry.amount);
                }
            } else {
                StartCoroutine(AddItemCoroutine());
            }
            _alreadyAdded = true;
        }

        if (_addOnce && _alreadyAdded) {
            SetEventSubscription(false, quest, eventType);
        }
    }

    IEnumerator AddItemCoroutine() {
        foreach (ItemEntry entry in _itemsToAddSet) {
            _deposit.AddItem(entry.itemName, entry.amount);
            yield return null;
        }
    }

}
