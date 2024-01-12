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

    [SerializeField] protected List<Quest> _targetQuestList;
    [SerializeField] private List<EventType> _eventList;
    private HashSet<Quest> _targetQuestSet = new HashSet<Quest>();
    private HashSet<EventType> _eventSet;
    // Start is called before the first frame update
    protected virtual void Start() {
        if (_targetQuestList == null) {
            Debug.LogError(transform.name + ": QuestGameObjectSpawner has no list of quests to keep track of");
        } else if (_targetQuestList == null) {
            Debug.LogError(transform.name + ": QuestGameObjectSpawner: list of quests is empty");
        }

        if (_eventList == null) {
            Debug.LogError(transform.name + ": QuestGameObjectSpawner has no event list to subscribe to");
        } else if (_eventList.Count == 0) {
            Debug.LogWarning(transform.name + "; QuestGameObjectSpawner: event list is empty");
        }

        _targetQuestSet = _targetQuestSet.ToHashSet();
        _eventSet = _eventList.ToHashSet();

        foreach(Quest quest in _targetQuestSet) {
            foreach(EventType ev in _eventSet) {
                SetEventSubscription(true, quest, ev);
            }
        }
    }

    protected virtual void OnQuestStarted(Quest quest) {
        //default behaviour unsubscribes from event
        //this is done for two reasons:
        // 1) Since this method has not been overriden by a subclass we assume that the event registration was a mistake in the first place, and thus we need to unsubscribe
        // 2) It provides an easy way for the subclass to unsubscribe from an event without knowing which EventType the event is associated to
        SetEventSubscription(false, quest, EventType.START);
    }

    protected virtual void OnQuestProgressed(Quest quest) {
        //default behaviour unsubscribes from event
        //this is done for two reasons:
        // 1) Since this method has not been overriden by a subclass we assume that the event registration was a mistake in the first place, and thus we need to unsubscribe
        // 2) It provides an easy way for the subclass to unsubscribe from an event without knowing which EventType the event is associated to
        SetEventSubscription(false, quest, EventType.PROGRESS);
    }

    protected virtual void OnQuestCompleted(Quest quest) {
        //default behaviour unsubscribes from event
        //this is done for two reasons:
        // 1) Since this method has not been overriden by a subclass we assume that the event registration was a mistake in the first place, and thus we need to unsubscribe
        // 2) It provides an easy way for the subclass to unsubscribe from an event without knowing which EventType the event is associated to
        SetEventSubscription(false, quest, EventType.COMPLETE);
    }

    protected virtual void OnEnteredQuestArea(Quest quest) {
        //default behaviour unsubscribes from event
        //this is done for two reasons:
        // 1) Since this method has not been overriden by a subclass we assume that the event registration was a mistake in the first place, and thus we need to unsubscribe
        // 2) It provides an easy way for the subclass to unsubscribe from an event without knowing which EventType the event is associated to
        SetEventSubscription(false, quest, EventType.AREA_ENTER);
    }

    protected virtual void OnExitedQuestArea(Quest quest) {
        //default behaviour unsubscribes from event
        //this is done for two reasons:
        // 1) Since this method has not been overriden by a subclass we assume that the event registration was a mistake in the first place, and thus we need to unsubscribe
        // 2) It provides an easy way for the subclass to unsubscribe from an event without knowing which EventType the event is associated to
        SetEventSubscription(false, quest, EventType.AREA_EXIT);
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
}
