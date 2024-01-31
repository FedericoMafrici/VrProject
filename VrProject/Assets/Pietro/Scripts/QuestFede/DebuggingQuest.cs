using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggingQuest : Quest
{
    [SerializeField] private string description;
    [SerializeField] private int _nInputsBeforeComplete = 1;
    [SerializeField] private KeyCode _inputKey;

    private int _registeredInputs = 0;

    protected override void OnQuestStart() {
        base.OnQuestStart();
        Debug.Log(transform.name + " debugging quest started");
    }

    // Update is called once per frame
    void Update() {
        if (_state == QuestState.ACTIVE) {
            if (Input.GetKeyDown(_inputKey)) { 
                _registeredInputs++;
                Debug.Log(transform.name + " debugging quest input detected");
                Progress();
                if (_registeredInputs >= _nInputsBeforeComplete) {
                    _registeredInputs = _nInputsBeforeComplete;
                    Debug.Log(transform.name + " debugging quest completed");
                    Complete();
                }
            }
        }
        
    }

    public override string GetQuestDescription() {
        return description + " " + _registeredInputs + "/" + _nInputsBeforeComplete;
    }
}
