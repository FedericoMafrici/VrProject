using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class QuestObjectActivator : QuestEventReceiver {
    [SerializeField] List<GameObject> _gameObjects = new List<GameObject>();
    [SerializeField] List<GameObject> _toExcludeList = new List<GameObject>();
    [SerializeField] private bool _startInactive = true;
    private HashSet<GameObject> _gameObjectsSet = new HashSet<GameObject>();
    private HashSet<GameObject> _toExcludeSet = new HashSet<GameObject>();
    private bool _objectsActivated = false; //failsafe in case GameObjects get activated twice

    protected override void Awake() {
        base.Awake();

        if (_gameObjects == null) {
            Debug.LogError(transform.name + " QuestObjectActivator: no list of GameObjects");
        } else if (_gameObjects.Count == 0) {
            Debug.LogWarning(transform.name + " QuestObjectActivator: list of objects is empty");
        }

        if (_toExcludeList == null) {
            Debug.LogError(transform.name + " QuestObjectActivator: _toExcludeList is null");
        }

        foreach (GameObject obj in _gameObjects) {
            if (obj != null) {
                _gameObjectsSet.Add(obj);
            }
        }

        _toExcludeSet = _toExcludeList.ToHashSet();

        if (_startInactive) {
            foreach (GameObject gameObject in _gameObjectsSet) {
                gameObject.SetActive(false);
            }
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        //Debug.Log("event received");
        ActivateItems();

        SetEventSubscription(false, quest, eventType);
    }

    private void SetActiveGameObject(bool active, GameObject gameObject, bool childrenToo = false) {
        gameObject.SetActive(active);

        if (childrenToo) {
            Transform gameObjTransform = gameObject.transform;
            if (gameObjTransform != null) {
                int childrenCount = gameObjTransform.childCount;
                for (int i = 0; i < childrenCount; i++) {
                    GameObject child = gameObjTransform.GetChild(i).gameObject;
                    child.SetActive(active);
                }
            }
        }
    }

    private void ActivateItems() {
        if (!_objectsActivated) {
            foreach (GameObject gameObject in _gameObjectsSet) {
                SetActiveGameObject(true, gameObject);
            }

            foreach (GameObject gameObject in _toExcludeList) {
                SetActiveGameObject(false, gameObject);
            }
            _objectsActivated = true;
        }
    }
}
