using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletionAchievement : Quest {
    [SerializeField] List<Transform> _questParentsList = new List<Transform>();
    [SerializeField] List<Quest> _targetQuestList = new List<Quest>();
    [SerializeField] private string _description;
    [SerializeField] List<int> _toReachValues = new List<int>();
    [SerializeField] private bool _displayProgress = true;
    private HashSet<Quest> _quests = new HashSet<Quest>();
    private int _valuesIndex = 0;

    //private int _nToComplete;
    private int _nCompleted = 0;

    protected override void Init() {
        if (_questParentsList != null) {
            foreach (Transform parent in _questParentsList) {
                GetQuestsFromParentTransform(parent, _quests);
            }
        }

        foreach (Quest q in _targetQuestList) {

            _quests.Add(q);
        }


        //_nToComplete = _quests.Count;

        foreach (Quest q in _quests) {
            if (q.GetState() == QuestState.COMPLETED) {
                AdvanceCounter();
            } else {
                q.QuestCompleted += OnQuestCompleteEvent;
            }
        }

        _toReachValues.Sort();

        _autoStart = true;
        base.Init();
    }

    private void OnQuestCompleteEvent(Quest quest) {
        if (_state == QuestState.ACTIVE) {
            quest.QuestCompleted -= OnQuestCompleteEvent;
            AdvanceCounter();
        }
    }

    private void AdvanceCounter() {
        if (_state == QuestState.ACTIVE) {
            _nCompleted++;
            Debug.LogWarning("<color=cyan>" + this + ": progress for achievement, id: " + GetID() + "</color>");
            if (_nCompleted >= _toReachValues[_valuesIndex]) {
                Progress();
                string color = _valuesIndex == 0 ? "green" : (_valuesIndex == 1 ? "yellow" : "red");
                Debug.Log("<color=" + color + ">" + this + ": achievement unlocked" + " tier : " + _valuesIndex + 1 + ", id: " + GetID() + "</color>");
                if (_valuesIndex < _toReachValues.Count - 1) {
                    _valuesIndex++;
                } else {
                    Complete();
                }
            }
        }
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

    public override string GetQuestDescription() {
        if (_displayProgress) {
            return _description + " " + _nCompleted + "/" + _toReachValues[_valuesIndex];
        } else {
            return _description;
        }
    }
}
