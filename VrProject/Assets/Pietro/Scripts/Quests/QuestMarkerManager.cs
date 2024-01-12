using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerManager : MonoBehaviour
{
    [SerializeField] private Transform _markerPositionTransform; //transform that will be set as parent of the SpriteRenderer instance
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private List<QuestID> _associatedQuests;
    private HashSet<QuestID> _requestsIDs = new HashSet<QuestID>(); //Set of QuestsIDs of quests that have requested to show the marker
    private Vector3 _markerRelativePosition; //position of indicator relative to this GameObject
    private bool _canBeShown = true;
    private Item _itemComponentReference; //needed in order to make indicators disappear when an item is held
    //private int _nShowRequests = 0;
    private GameObject _markerInstance;
    private bool _isDeposited = false; //needed to manage deposited items, find a better way

    // Start is called before the first frame update
    void Start() {
        if (_markerPrefab == null) {
            Debug.LogError(transform.name + ": indicator Canvas prefab is not set");
        }

        if (_markerPositionTransform == null) {
            Debug.LogError(transform.name + " quest indicator position is null");
        }

        _markerInstance = Instantiate(_markerPrefab, _markerPositionTransform.position, Quaternion.identity);
        _markerRelativePosition = _markerInstance.transform.position - transform.position;
        //_spriteCanvasInstance.transform.parent = _indicatorParentTransform;
        

        QuestMarkerDatabase.MarkerShowEvent += OnDatabaseShowRequest;
        QuestMarkerDatabase.MarkerHideEvent += OnDatabaseHideRequest;

        foreach (QuestID questId in _associatedQuests) {
            //check if the QuestID is in the marker database
            if(QuestMarkerDatabase.markerDatabase.Contains(questId)) {
                _requestsIDs.Add(questId);
            }
        }

        _itemComponentReference = GetComponent<Item>();

        if (_itemComponentReference != null) {
            _itemComponentReference.GrabEvent += OnItemGrabbed;
            _itemComponentReference.ReleaseEvent += OnItemReleased;
            if (_itemComponentReference.isInPlayerHand || _itemComponentReference.isCollected) {
                _canBeShown = false;
            }
        }
        

        _markerInstance.gameObject.SetActive(_requestsIDs.Count > 0 && _canBeShown); //enable component if there's at least one show request
    }

    private void CheckIfShow() {
        if (!_isDeposited && _canBeShown && _requestsIDs.Count > 0) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        if (!_markerInstance.gameObject.activeSelf) {
            _markerInstance.gameObject.SetActive(true);
        }
    }

    private void Hide() {
        if (_markerInstance.gameObject.activeSelf) {
            _markerInstance.gameObject.SetActive(false);
        }
    }

    public void SetIsDeposited(bool isDeposited) {
        _isDeposited = isDeposited;
        CheckIfShow();
    }

    public void AddShowRequest(QuestID questId) {
        _requestsIDs.Add(questId);

        if (_requestsIDs.Count == 1) {
            CheckIfShow();
        }
    }

    public void RemoveShowRequest(QuestID questId) {
        _requestsIDs.Remove(questId);

        if (_requestsIDs.Count == 0) {
            //no request to show marker, hide marker
            CheckIfShow();
        }
    }

    public void OnDatabaseShowRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            AddShowRequest(questId);
        }

    }

    public void OnDatabaseHideRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            RemoveShowRequest(questId);
        }
    }

    public void OnItemGrabbed() {
        _canBeShown = false;
        _markerInstance.gameObject.SetActive(false);
    }

    public void OnItemReleased() {
        _canBeShown = true;
        if (_requestsIDs.Count > 0) {
            _markerInstance.gameObject.SetActive(true);
        }
    }

    private void OnDestroy() {

        if (_itemComponentReference != null) {
            _itemComponentReference.GrabEvent -= OnItemGrabbed;
            _itemComponentReference.ReleaseEvent -= OnItemReleased;
        }

        QuestMarkerDatabase.MarkerShowEvent -= OnDatabaseShowRequest;
        QuestMarkerDatabase.MarkerHideEvent -= OnDatabaseHideRequest;

        if (_markerInstance != null) {
            Destroy(_markerInstance.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        if (_markerInstance != null) {
            _markerInstance.transform.position = transform.position + _markerRelativePosition;
        }
    }
}
