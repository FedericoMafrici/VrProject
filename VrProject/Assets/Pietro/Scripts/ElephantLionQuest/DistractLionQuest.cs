using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistractLionQuest : Quest
{
    [SerializeField] private string _description;
    [SerializeField] private LionQuestBehaviour _lion;
    [SerializeField] private NPCMover _adultElephant;

    private void Start() {
        Init();
    }

    protected override void Init() {
        if (_lion == null) {
            Debug.LogError(transform.name + " no Lion set for Distract Lion Quest");
        }

        if (_lion == null) {
            Debug.LogError(transform.name + " no Adult Elephant set for Distract Lion Quest");
        }

        base.Init();
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();

        Debug.LogWarning("distract quest started");
        _lion.LionDistractedEvent += OnLionDistracted;
        if (_lion.IsDistracted()) {
            Debug.LogWarning("Lion is already distracted");
            OnLionDistracted();
        } 
        
    }

    private void OnLionDistracted() {
        if (_state == QuestState.ACTIVE) {
            _lion.LionDistractedEvent -= OnLionDistracted;
            //_adultElephant.SetPatrolArea(_finalElephantArea, true);
            Progress();
            Complete();
        }
    }

    public override string GetQuestDescription() {
        return _description;
    }
}
