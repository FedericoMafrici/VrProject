using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipQuest : Quest {
    [SerializeField] private List<Pettable> _pettables= new List<Pettable>();
    [SerializeField] private float _nToBefriend;
    [SerializeField] private string _description;
    private float _nBefriended = 0;

    protected override void Start() {
        base.Start();

        if (_pettables == null) {
            Debug.LogError(transform.name + ": pettable list is null");
        } else if (_pettables.Count == 0 ) {
            Debug.LogWarning(transform.name + ": pettable list is null");
        }

        if (_nToBefriend < 1) {
            Debug.LogWarning(transform.name + ": number of animals to befriend was lower than 1, setting it to 1");
            _nToBefriend = 1;
        }

        if (_description == null) {
            Debug.LogWarning(transform.name + ": no description");
        }

        if (!_isStep) {
            InitQuest();
        }
    }

    private void OnAnimalBefriended(object sender, EventArgs args) {
        if (_state == QuestState.ACTIVE) {
            Debug.Log("Animal befriended: " + (sender as Pettable).transform.name);
            _nBefriended++;
            Progress();

            if (_nBefriended >= _nToBefriend) {

                // complete quest
                _nBefriended = _nToBefriend;
                Complete();

                // unsubscribe from event
                foreach (Pettable _pettable in _pettables) {
                    _pettable.Befriended -= OnAnimalBefriended;
                }
            }
        }
    }

    protected override void OnQuestStart() {

        //subscribe to pettables
        foreach (Pettable pettable in _pettables) {
            if (_state != QuestState.COMPLETED) { //quest might complete while iterating, check that this is not the case before executing code
                if (pettable != null) {

                    pettable.Befriended += OnAnimalBefriended;

                    //check if pettable was already befriended
                    if (pettable.IsAtMaxFriendship()) {
                        OnAnimalBefriended(pettable, EventArgs.Empty);
                    }
                }
            }
        }
    }

    protected override void PlayerEnteredQuestArea() {
        base.PlayerEnteredQuestArea();
    }

    public override void Complete() {
        base.Complete();
        Debug.Log("Friendship quest completed");
    }

    public override string GetQuestDescription() {
        string result = "";
        if (_description != null) {
            result += _description;
        }

        result += " " + _nBefriended + "/" + _nToBefriend;
        return result;
    }

}
