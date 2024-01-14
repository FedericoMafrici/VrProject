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

        if (_toStartCollection == null) {
            if (_toStartList == null) {
                Debug.LogError(transform.name + ": QuestStarter has no list of quests to start");
            } else if (_toStartList == null) {
                Debug.LogWarning(transform.name + ": QuestStarter: list of quests to start is empty");
            }

            _toStartSet = _toStartList.ToHashSet();

        } else {
            if (_toStartList != null && _toStartList.Count > 0) {
                Debug.LogWarning(transform.name + ": QuestStarter: Quest Set was specified through a collection, but list of quests to start is not empty, the list will be ignored");
            }

            _toStartSet = GetQuestsFromParentTransform(_toStartCollection);
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
