using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingQuest : Quest {
    [SerializeField] private string _description;
    [SerializeField] private CollectingPoint _collectingPoint;
    [SerializeField] private Item.ItemName _targetItem;
    [SerializeField] private int _nToCollect;
    private int _nCollected = 0;

    protected override void Init() {
        if (_collectingPoint == null) {
            Debug.LogError(transform.name + ": no Collecting Point set for Collecting Quest");
        }
        if (_nToCollect <= 0) {
            Debug.LogWarning(transform.name + ": count of items to deposit was lower than 1, setting it to 1");
            _nToCollect = 1;
        }

        base.Init();
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        //subscribe to events
        _collectingPoint.ItemInCollectingPoint += OnItemCollected;
        _collectingPoint.ItemOutOfCollectingPoint += OnItemRemoved;

        //check if collecting point already contains some items of the requested type
        if (_collectingPoint.collectedItems.ContainsKey(_targetItem)) {
            int alreadyCollectedCount = _collectingPoint.collectedItems[_targetItem];
            AddItem(alreadyCollectedCount);
        }
    }

    private void AddItem(int count) {
        if (_state == QuestState.ACTIVE) {
            _nCollected += count;
            Progress();

            if (_nCollected >= _nToCollect) {

                //total amount reached
                _nCollected = _nToCollect;
                Complete();

                _collectingPoint.ItemInCollectingPoint -= OnItemCollected;
                _collectingPoint.ItemOutOfCollectingPoint -= OnItemRemoved;
            }
        }
    }

    private void OnItemCollected(Item item) {
        if (item.itemName == _targetItem) {
            AddItem(1);
        }
    }

    private void OnItemRemoved(Item item) {
        if (item.itemName == _targetItem) {
            Debug.Log(transform.name + ": removed 1 " + _targetItem);
            _nCollected--;
            Progress();
            if (_nCollected < 0) {
                _nCollected = 0;
            }
        }
    }

    public override string GetQuestDescription() {
        string result = "";
        if (_description != null) {
            result += _description;
        }

        result += " " + _nCollected + "/" + _nToCollect;
        return result;
    }

    public override void ShowMarkers() {
        AdditionalQuestInfo info;
        info.isCollectingQuest = true;
        QuestMarkerDatabase.RequestShowMarkers(GetID(), info);
        QuestMarkerManager markerManager = _collectingPoint.GetComponent<QuestMarkerManager>();
        if (markerManager != null) {
            markerManager.AddShowRequest(GetID(), info);
        }
    }

    public override void HideMarkers() {
        QuestMarkerDatabase.RequestHideMarkers(GetID());
        QuestMarkerManager markerManager = _collectingPoint.GetComponent<QuestMarkerManager>();
        if (markerManager != null) {
            markerManager.RemoveShowRequest(GetID());
        }
    }

    public override bool AutoComplete() {
        ForceStart();
        AutoCompletePreCheck();
        AddItem(_nToCollect - _nCollected);
        AutoCompletePostCheck();
        return true;
    }
}
