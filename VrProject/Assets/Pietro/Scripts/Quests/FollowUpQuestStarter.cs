using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FollowUpQuestStarter : QuestEventReceiver {
    [Header("Quests to start settings")]
    [SerializeField] private Transform _toStartCollection; //a reference to a transform whose children are the quest that need to be subscripted to
    [SerializeField] private List<Quest> _toStartList; //used if _questSetParent is null to determine set of quest to subscribe to
    private HashSet<Quest> _toStartSet = new HashSet<Quest>();
    bool _alreadyStarted = false;

    protected override void Awake() {
        base.Awake();

        if (_toStartCollection != null) {
            GetQuestsFromParentTransform(_toStartCollection, _toStartSet);
        }

        if (_toStartList != null) {
            foreach (Quest quest in _toStartList) {
                foreach (Quest questComponent in quest.transform.GetComponents<Quest>()) {
                    _toStartSet.Add(questComponent);
                }
            }
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        if (!_alreadyStarted) {
            foreach (Quest toStart in _toStartSet) {
                if (toStart.GetState() == QuestState.NOT_STARTED && !toStart.IsStep()) {
                    toStart.StartQuest();
                }
            }

            _alreadyStarted = true;
        }

        SetEventSubscription(false, quest, eventType);

    }
}
