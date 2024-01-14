using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpQuest : Quest
{
    [SerializeField] private string _description;
    [SerializeField] private int _nToPickUp;
    private int _nPickedUp = 0;
    [SerializeField] private Item.ItemName _targetItemName;
    [SerializeField] private PlayerPickUpDrop _playerPickUp;

    protected override void Init() {
        if (_playerPickUp == null) {
            Debug.LogError(transform.name + ": no PlayerPickUpDrop reference set");
        }

        if (_nToPickUp <= 0) {
            Debug.LogWarning(transform.name + ": number of items to pick up was lower than 1, setting it to 1");
            _nToPickUp = 1;
        }

        base.Init();
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        _playerPickUp.PickUpEvent += OnPickUp;
        _playerPickUp.DropEvent += OnDrop;
    }

    private void OnPickUp(Item.ItemName itemName) {
        if (_state == QuestState.ACTIVE) {
            if (itemName == _targetItemName) {
                _nPickedUp++;
                Progress();
                if (_nPickedUp >= _nToPickUp) {
                    _nPickedUp = _nToPickUp;
                    Complete();
                }
            }
        }
    }

    private void OnDrop(Item.ItemName itemName) {
        if (_state == QuestState.ACTIVE) {
            if (itemName == _targetItemName) {
                if (_nPickedUp > 0) {
                    _nPickedUp--;
                    Progress();
                }
            }
        }
    }

    public override void ShowMarkers() {
        QuestMarkerDatabase.RequestShowMarkers(GetID());
    }

    public override void HideMarkers() {
        QuestMarkerDatabase.RequestHideMarkers(GetID());
    }

    public override string GetQuestDescription() {
        return _description + " " + _nPickedUp + "/" + _nToPickUp;
    }

}
