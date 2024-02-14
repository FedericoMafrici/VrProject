using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletionCounter : Quest {

    [SerializeField] List<Transform> _questParentsList= new List<Transform>();
    private HashSet<Quest> _quests = new HashSet<Quest>();
    private int _nToComplete;
    private int _nCompleted = 0;

    protected override void Init() {
        if (_questParentsList != null) {
            foreach (Transform parent in _questParentsList) {
                GetQuestsFromParentTransform(parent, _quests);
            }
        }

        _nToComplete = _quests.Count;

        foreach (Quest q in _quests) {
            if (q.GetState() == QuestState.COMPLETED) {
                AdvanceCounter();
            } else {
                q.QuestCompleted += OnQuestCompleteEvent;
            }
        }
        base.Init();
    }

    private void OnQuestCompleteEvent(Quest quest) {
        Debug.LogWarning("Event received from " + quest + " current state is: " + _state);
        if (_state == QuestState.ACTIVE) {
            quest.QuestCompleted -= OnQuestCompleteEvent;
            AdvanceCounter();
        }
    }

    private void AdvanceCounter() {
        if (_state == QuestState.ACTIVE) {
            _nCompleted++;
            Progress();
            if (_nCompleted >= _nToComplete) {
                Complete();
            }
        }
    }


    public override string GetQuestDescription() {
        return "";
    }

    protected void GetQuestsFromParentTransform(Transform collection, HashSet<Quest> _questSet) {
        //HashSet<Quest> quests = new HashSet<Quest>();
        int nChildren = collection.childCount;

        for (int i = 0; i < nChildren; i++) {
            Transform child = collection.GetChild(i);
            Quest[] toAddArray = child.GetComponents<Quest>();
            foreach (Quest q in toAddArray) {
                _questSet.Add(q);
            }
        }

    }

}
