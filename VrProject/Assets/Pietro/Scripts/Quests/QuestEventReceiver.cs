using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Searcher.Searcher.AnalyticsEvent;

public abstract class QuestEventReceiver : MonoBehaviour {
    protected enum EventType {
        NONE, //a fake event type that causes the adoption of a default behaviour
        START,
        COMPLETE,
        PROGRESS,
        AREA_ENTER,
        AREA_EXIT
    }

    [Header("Quests to listen settings")]
    [SerializeField] private Transform _questCollection; //a reference to a transform whose children are the quest that need to be subscripted to
    [SerializeField] private List<Quest> _targetQuestList; //used if _questSetParent is null to determine set of quest to subscribe to
    [Header("Events to subscribe to")]
    [SerializeField] private List<EventType> _eventList;
    public HashSet<Quest> _targetQuestSet = new HashSet<Quest>();
    private HashSet<EventType> _eventSet;
    // Start is called before the first frame update
    protected virtual void Awake() {

        if (_questCollection != null) {
            _targetQuestSet = GetQuestsFromParentTransform(_questCollection);
        }

        if (_targetQuestList != null) {
            foreach (Quest quest in _targetQuestList) {
                _targetQuestSet.Add(quest);
            }
            //Debug.LogWarning(transform.name + ": QuestEventReceiver: Quest Set was specified through a transform, but target quest list is not empty, target quest list will be ignored");
        }

        /*
        if (_targetQuestList == null) {
            Debug.LogError(transform.name + ": QuestEventReceiver has no list of quests to keep track of");
        } else if (_targetQuestList == null) {
            Debug.LogWarning(transform.name + ": QuestEventReceiver: list of quests is empty");
        }
        */

        //_targetQuestSet = _targetQuestList.ToHashSet();

        if (_targetQuestSet.Count == 0) {
            Debug.LogWarning(transform.name + ": QuestEventReceiver has no reference to any quest");
        }

        _eventSet = _eventList.ToHashSet();

        //subscribe to events for every quest
        foreach(Quest quest in _targetQuestSet) {
            foreach(EventType ev in _eventSet) {
                SetEventSubscription(true, quest, ev);
            }
        }
    }

    protected abstract void OnEventReceived(Quest quest, EventType eventType);

    private void OnQuestStarted(Quest quest) {
        OnEventReceived(quest, EventType.START);
    }

    protected virtual void OnQuestProgressed(Quest quest) {
        OnEventReceived(quest, EventType.PROGRESS);
    }

    protected virtual void OnQuestCompleted(Quest quest) {
        OnEventReceived(quest, EventType.COMPLETE);
    }

    protected virtual void OnEnteredQuestArea(Quest quest) {
        OnEventReceived(quest, EventType.AREA_ENTER);
    }

    protected virtual void OnExitedQuestArea(Quest quest) {
        OnEventReceived(quest, EventType.AREA_EXIT);
    }

    protected void SetEventSubscription(bool subscribe, Quest quest, EventType eventType) {
        switch (eventType) {
            case EventType.START:
                if (subscribe) {
                    quest.QuestStarted += OnQuestStarted;
                } else {
                    quest.QuestStarted -= OnQuestStarted;
                }
                break;
            case EventType.COMPLETE:
                if (subscribe) {
                    quest.QuestCompleted += OnQuestCompleted;
                } else {
                    quest.QuestCompleted -= OnQuestCompleted;
                }
                break;
            case EventType.PROGRESS:
                if (subscribe) {
                    quest.QuestProgressed += OnQuestProgressed;
                } else {
                    quest.QuestProgressed -= OnQuestProgressed;
                }
                break;
            case EventType.AREA_ENTER:
                if (subscribe) {
                    quest.EnteredArea += OnEnteredQuestArea;
                } else {
                    quest.EnteredArea -= OnEnteredQuestArea;
                }
                break;
            case EventType.AREA_EXIT:
                if (subscribe) {
                    quest.ExitedArea += OnExitedQuestArea;
                } else {
                    quest.ExitedArea -= OnExitedQuestArea;
                }
                break;
            default:
                Debug.LogWarning(transform.name + " QuestGameObjectSpawner: unkown event specified in subscribe function");
                break;
        }
    }

    private void OnDestroy() {
        foreach (Quest quest in _targetQuestSet) {
            foreach (EventType ev in _eventSet) {
                SetEventSubscription(false, quest, ev);
            }
        }
    }

    protected HashSet<Quest> GetQuestsFromParentTransform(Transform collection) {
        HashSet<Quest> quests = new HashSet<Quest>();
        int nChildren = collection.childCount;

        for (int i = 0; i < nChildren; i++) {
            Transform child = collection.GetChild(i);
            Quest[] toAddArray = child.GetComponents<Quest>();
            foreach (Quest q in toAddArray) {
                    quests.Add(q);
            }
        }

        return quests;
    }
}
