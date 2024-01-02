using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipQuest : Quest {
    [SerializeField] private List<Pettable> _pettables= new List<Pettable>();
    [SerializeField] private float _nToBefriend;
    private float _nBefriended = 0;

    protected override void Start() {
        base.Start();
        if (_pettables.Count == 0 ) {
            Debug.LogWarning(transform.name + ": no pettables to keep track of");
        }

        foreach(Pettable _pettable in _pettables) {
            _pettable.Befriended += OnAnimalBefriended;
        }

        if (_nToBefriend < 1) {
            Debug.LogWarning(transform.name + ": number of animals to befriend was lower than 1, setting it to 1");
            _nToBefriend = 1;
        }
    }

    private void OnAnimalBefriended(object sender, EventArgs args) {
        if (_hasStarted) {
            Debug.Log("Animal befriended: " + (sender as Pettable).transform.name);
            _nBefriended++;
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

    public override void Complete() {
        base.Complete();
        Debug.Log("Friendship quest completed");
    }

    public override string GetQuestDescription() {
        string result =  base.GetQuestDescription();
        result += " " + _nBefriended + "/" + _nToBefriend;
        return result;
    }

}
