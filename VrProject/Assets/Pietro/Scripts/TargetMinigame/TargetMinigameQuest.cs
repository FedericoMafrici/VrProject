using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetMinigameQuest : Quest {
    [SerializeField] private string _description;
    [SerializeField] private string _finalDescription;
    [SerializeField] private List<TargetMinigame> _targetMinigameList;
    private HashSet<TargetMinigame> _targetMinigameSet = new HashSet<TargetMinigame>();

    protected override void Init() {
        if (_targetMinigameList == null) {
            Debug.LogError(transform.name + " Target Minigame Quest, no Target Minigame List set");
        } else if (_targetMinigameList.Count == 0) {
            Debug.LogWarning(transform.name + " Target Minigame Quest, Target Minigame list is empty");
        }

        _targetMinigameSet = _targetMinigameList.ToHashSet();

        base.Init();
    }

    private void OnMinigameCompleted() {
        if (_state == QuestState.ACTIVE) {
            foreach (TargetMinigame minigame in _targetMinigameList) {
                minigame.CompleteEvent -= OnMinigameCompleted;
            }
            Progress();
            Complete();
            Debug.LogWarning("Minigame quest completed");
        }
    }

    protected override void OnQuestStart() {
        base.OnQuestStart();
        foreach (TargetMinigame minigame in _targetMinigameList) {
            minigame.CompleteEvent += OnMinigameCompleted;
        }
    }

    public override string GetQuestDescription() {
        if (_state != QuestState.COMPLETED) {
            return _description;
        } else {
            return _finalDescription;
        }
        return _description;
    }

    public override bool AutoComplete() {
        ForceStart();
        AutoCompletePreCheck();
        OnMinigameCompleted();
        AutoCompletePostCheck();
        return true;
    }
}
