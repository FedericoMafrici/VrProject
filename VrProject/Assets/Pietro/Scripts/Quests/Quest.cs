using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;

public enum QuestID {
    TEST_QUEST,
    TUTORIAL,
    TUTORIAL_PICK_UP_FOOD,
    TUTORIAL_PICK_UP_EGG,
    TUTORIAL_FIRST_EATING_STEP,
    TUTORIAL_SECOND_EATING_STEP,
    TUTORIAL_FIRST_COLLECTING_STEP,
    TUTORIAL_SECOND_COLLECTING_STEP,
    PETTABLE_TUTORIAL,
    PETTABLE_TUTORIAL_LOOKAT_STEP,
    CLEANING_TEST,
    ELEPHANT_LION_STRUCTURED,
    ELEPHANT_LION_ESCORT,
    ELEPHANT_LION_REACH_TARGET,
    ELEPHANT_LION_DISTRACT,
    HORSE_FRIENDSHIP,
    HORSE_EATING,
    HORSE_IRONING,
    COW_FRIENDSHIP,
    COW_EATING,
    COW_MILKING,
    COW_COLLECT_MILK,
    CHICKEN_FRIENDSHIP,
    CHICKEN_EATING,
    PIG_FRIENDSHIP,
    PIG_EATING,
    PIG_CLEANING,
    SHEEP_FRIENDSHIP,
    SHEEP_EATING,
    SHEEP_SHAVING,
    SHEEP_COLLECT_WOOL
}

public enum QuestState {
    NOT_STARTED, //yet to be started
    ACTIVE, //started but not completed yet
    COMPLETED, //completed
    INACTIVE //not necesserily used, may be useful, signals a quest that has started, not completed but cannot be considered "active"
}

[Serializable] public struct JournalInformation
{
    public Animal.AnimalName animalName;
    public string newInformation;
    public int orderNumber;
}

public abstract class Quest : MonoBehaviour {
    [SerializeField] private QuestID _id;
    [SerializeField] private bool _startOnEnter = false;
    [SerializeField] private bool _showMarkersOnEnter = false;
    [SerializeField] private JournalInformation _info;
    protected  QuestState _state = QuestState.NOT_STARTED;
    protected bool _isStep = false; //should be false if the true is a step in a StructuredQuest, false otherwise
    private bool _inited = false;

    public event Action<Quest> EnteredArea;
    public event Action<Quest> ExitedArea;
    public event Action<Quest> QuestStarted;
    public event Action<Quest> QuestProgressed;
    public event Action<Quest> QuestCompleted;

    private void Start() {
        CheckInit();
        if (!_isStep) {
            AreaCheck();
        }
    }

    protected void CheckInit() {
        if (!_inited) {
            Init();
        }
    }

    protected virtual void Init() {
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll == null) {
            Debug.LogWarning(transform.name + " no BoxCollider component found");
        } else if (!coll.isTrigger) {
            Debug.LogWarning(transform.name + " BoxCollider component is not set as \"is trigger\"");
        }

        _inited= true;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if ((_state != QuestState.COMPLETED) && (LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            PlayerEnteredQuestArea();
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if  ((_state != QuestState.COMPLETED) && (LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            PlayerExitedQuestArea();
        }
    }

    protected virtual void PlayerEnteredQuestArea() {
        Debug.LogWarning(transform.name + ": player entered area");
        if (!_isStep && _startOnEnter) {
            StartQuest();
        }

        if (_state == QuestState.ACTIVE && _showMarkersOnEnter) {
            ShowMarkers();
        }

        if (EnteredArea != null) {
            Debug.LogWarning(transform.name + ": area entered event sent");
            EnteredArea(this);
        }
    }

    protected virtual void PlayerExitedQuestArea() {
        Debug.LogWarning(transform.name + ": player exited area");
        if (ExitedArea != null) {
            Debug.LogWarning(transform.name + ": area exited event sent");
            ExitedArea(this);
        }

        if (_state == QuestState.ACTIVE && _showMarkersOnEnter) {
            HideMarkers();
        }
    }

    protected virtual void Progress() {
        if (QuestProgressed != null) {
            QuestProgressed(this);
        }
    }

    public virtual void AreaCheck() {
        if (PlayerIsInQuestArea()) {
            PlayerEnteredQuestArea();
        }
    }

    public bool PlayerIsInQuestArea() {
        //checks if player is inside the quest area
        BoxCollider area = GetQuestArea();
        if (area != null && area.enabled) {
            Bounds bounds = GetQuestArea().bounds;
            Collider[] colliders = Physics.OverlapBox(bounds.center, bounds.extents, Quaternion.identity, LayerMask.GetMask("Player"));
            return colliders.Length > 0;
        } else {
            return false;
        }
    }

    public virtual void Complete() {
        _state = QuestState.COMPLETED;
        HideMarkers();
        if (QuestCompleted != null) {
            QuestCompleted(this);
        }
    }

    public virtual bool StartQuest() { 
        //starts quest if it has not started yet
        //returns true if quest gets started
        CheckInit();

        bool didStart = false;
        if (_state == QuestState.NOT_STARTED) {
            //Debug.Log(transform.name + ": started, description: " + GetQuestDescription());
            _state = QuestState.ACTIVE;
            if (QuestStarted != null) {
                QuestStarted(this);
            }
            didStart = true;
            OnQuestStart();
        }

        return didStart;
    }

    protected virtual void OnQuestStart() {
        Debug.Log(transform.name + " quest started");
        if (!_startOnEnter) {
            AreaCheck();
        }
    }

    public virtual void Deactivate() {
        if (_state == QuestState.ACTIVE) {
            //Debug.Log(transform.name + ": deactivated");
            _state = QuestState.INACTIVE;
        }
    }

    public QuestState GetState() { return _state; }

    public QuestID GetID() { return _id; }

    public bool IsStep() { return _isStep; }

    public virtual BoxCollider GetQuestArea() {
        return GetComponent<BoxCollider>();
    }

    public virtual void ShowMarkers() {

    }

    public virtual void HideMarkers() {
        
    }

    //allows to derived classes to accept objects to register in its "objectives"
    public void RegisterObject(GameObject obj) {

    }

    public abstract string GetQuestDescription();

    public JournalInformation GetInfo()
    {
        return _info;
    }

    public void SetIsStep(bool value) {
        _isStep = value;
    }
}
