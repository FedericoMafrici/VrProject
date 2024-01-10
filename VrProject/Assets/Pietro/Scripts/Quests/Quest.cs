using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum QuestID {
    TEST_QUEST
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

    public event EventHandler EnteredArea;
    public event EventHandler ExitedArea;
    public event EventHandler QuestStarted;
    public event EventHandler QuestCompleted;

    protected virtual void Start() {
        BoxCollider coll = GetComponent<BoxCollider>();
        if (coll == null) {
            Debug.LogWarning(transform.name + " no BoxCollider component found");
        } else if (!coll.isTrigger) {
            Debug.LogWarning(transform.name + " BoxCollider component is not set as \"is trigger\"");
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
            EnteredArea(this, EventArgs.Empty);
        }
    }

    protected virtual void PlayerExitedQuestArea() {
        if (ExitedArea != null) {
            ExitedArea(this, EventArgs.Empty);
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
        if (!_isStep) {
            _alert.GetComponent<Alert>().Show();
        }
        _state = QuestState.COMPLETED;
        if (QuestCompleted != null) {
            QuestCompleted(this, EventArgs.Empty);
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
                QuestStarted(this, EventArgs.Empty);
            }
            didStart= true;
        }

        return didStart;
    }

    public virtual void Deactivate() {
        if (_state == QuestState.ACTIVE) {
            Debug.Log(transform.name + ": deactivated");
            _state = QuestState.INACTIVE;
        }
    }

    public QuestState GetState() { return _state; }

    public virtual BoxCollider GetQuestArea() {
        return GetComponent<BoxCollider>();
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

    public abstract string GetQuestDescription();
}
