using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum QuestID {
    TEST_QUEST
}

public class Quest : MonoBehaviour {
    private bool completed = false;
    [SerializeField] private QuestID _id;
    private BoxCollider _area;
    [SerializeField] private string _description;
    protected bool _hasStarted = false; //indicates wether the quest has been started or not
    protected bool _playerIsInArea = false; //indicates wether the player is in the quest area or not

    protected virtual void Start() {
        _area = GetComponent<BoxCollider>();

        if (_area == null) {
            Debug.LogWarning(transform.name + ": no BoxCollider component associated");
        } else if (!_area.isTrigger) {
            Debug.LogWarning(transform.name + " quest area is not set as \"is trigger\"");
        }

        if (_description == null) {
            Debug.LogWarning(transform.name + ": no description");
        }
    }

    protected virtual void Update() {

    }

    public virtual void Complete() {
        completed = true;
    }

    protected virtual void OnTriggerEnter(Collider other) {
        if ((LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            if (!_playerIsInArea) {
                Debug.Log(transform.name + ": quest activated, description: " + GetQuestDescription());
                _playerIsInArea= true;
                if (!_hasStarted) {
                    _hasStarted= true;
                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        if ((LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0) {
            if (_hasStarted && _playerIsInArea) {
                Debug.Log(transform.name + ": quest deactivated");
                _playerIsInArea= false;
            }
        }
    }

    public virtual string GetQuestDescription() {
        string result = "";
        if (_description != null) {
            result += _description;
        }

        return result;
    }
}
