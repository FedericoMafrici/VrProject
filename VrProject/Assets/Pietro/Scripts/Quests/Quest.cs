using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum QuestID {
    TEST_QUEST,
    TUTORIAL,
    TUTORIAL_PICK_UP_FOOD,
    TUTORIAL_PICK_UP_EGG,
    TUTORIAL_FIRST_EATING_STEP,
    TUTORIAL_SECOND_EATING_STEP,
    TUTORIAL_FIRST_COLLECTING_STEP,
    TUTORIAL_SECOND_COLLECTING_STEP
}

public enum QuestState {
    NOT_STARTED, //yet to be started
    ACTIVE, //started but not completed yet
    COMPLETED, //completed
    INACTIVE //not necesserily used, may be useful, signals a quest that has started, not completed but cannot be considered "active"
}

public abstract class Quest : MonoBehaviour {
    [SerializeField] private QuestID _id;
    [SerializeField] protected bool _isStep; //should be false if the true is a step in a StructuredQuest, false otherwise
    [SerializeField] private GameObject _alert;
    protected  QuestState _state = QuestState.NOT_STARTED;

    public event Action<Quest> EnteredArea;
    public event Action<Quest> ExitedArea;
    public event Action<Quest> QuestStarted;
    public event Action<Quest> QuestProgressed;
    public event Action<Quest> QuestCompleted;

    protected virtual void Start() {
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll == null) {
            Debug.LogWarning(transform.name + " no BoxCollider component found");
        } else if (!coll.isTrigger) {
            Debug.LogWarning(transform.name + " BoxCollider component is not set as \"is trigger\"");
        }

        if (!_isStep) {
            InitQuest();
        }
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if ((LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            PlayerEnteredQuestArea();
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if  ((LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            PlayerExitedQuestArea();
        }
    }

    protected virtual void PlayerEnteredQuestArea() {
        if (!_isStep) {
            StartQuest();
        }

        if (EnteredArea != null) {
            EnteredArea(this);
        }
    }

    protected virtual void PlayerExitedQuestArea() {
        if (ExitedArea != null) {
            ExitedArea(this);
        }
    }

    protected virtual void Progress() {
        if (QuestProgressed != null) {
            QuestProgressed(this);
        }
    }

    public virtual void InitQuest() {
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
        if (!_isStep && _alert != null) {
            _alert.GetComponent<Alert>().Show();
        }
        _state = QuestState.COMPLETED;
        HideMarkers();
        if (QuestCompleted != null) {
            QuestCompleted(this);
        }
    }

    public virtual bool StartQuest() { 
        //starts quest if it has not started yet
        //returns true if quest gets started

        bool didStart = false;
        if (_state == QuestState.NOT_STARTED) {
            Debug.Log(transform.name + ": started, description: " + GetQuestDescription());
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
       
    }

    public virtual void Deactivate() {
        if (_state == QuestState.ACTIVE) {
            Debug.Log(transform.name + ": deactivated");
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

    public void EnableCollider() {
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll != null) {
            coll.enabled = true;
        }
    }

    public void DisableCollider() {
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll != null) {
            coll.enabled = false;
        }
    }

    //allows to derived classes to accept objects to register in its "objectives"
    public void RegisterObject(GameObject obj) {

    }

    public abstract string GetQuestDescription();
}
