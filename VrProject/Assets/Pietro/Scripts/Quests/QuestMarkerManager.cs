using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarkerManager : MonoBehaviour
{
    [SerializeField] private Transform _markerPositionTransform; //transform that will be set as parent of the SpriteRenderer instance
    [SerializeField] private Canvas _spriteCanvasPrefab;
    [SerializeField] private List<QuestID> _associatedQuests;
    private Vector3 _markerRelativePosition; //position of indicator relative to this GameObject
    private bool _canBeShown = true;
    private Item _itemComponentReference; //needed in order to make indicators disappear when an item is held
    private int _nShowRequests = 0;
    private Canvas _spriteCanvasInstance; 

    // Start is called before the first frame update
    void Start() {
        if (_spriteCanvasPrefab == null) {
            Debug.LogError(transform.name + ": indicator Canvas prefab is not set");
        }

        if (_markerPositionTransform == null) {
            Debug.LogError(transform.name + " quest indicator position is null");
        }

        _spriteCanvasInstance = Instantiate(_spriteCanvasPrefab, _markerPositionTransform.position, Quaternion.identity);
        _markerRelativePosition = _spriteCanvasInstance.transform.position - transform.position;
        //_spriteCanvasInstance.transform.parent = _indicatorParentTransform;
        

        QuestMarkerDatabase.MarkerShowEvent += OnDatabaseShowRequest;
        QuestMarkerDatabase.MarkerHideEvent += OnDatabaseHideRequest;

        foreach (QuestID questId in _associatedQuests) {
            //first check if the QuestID is in the indicator database
            if(QuestMarkerDatabase.markerDatabase.Contains(questId)) {
                _nShowRequests++;
            }
        }

        _itemComponentReference = GetComponent<Item>();
        if (_itemComponentReference != null) {
            _itemComponentReference.GrabEvent += OnItemGrabbed;
            _itemComponentReference.ReleaseEvent += OnItemReleased;
            if (_itemComponentReference.isInPlayerHand) {
                _canBeShown = false;
            }
        }
        

        _spriteCanvasInstance.gameObject.SetActive(_nShowRequests > 0 && _canBeShown); //enable component if there's at least one show request
    }

    public void AddShowRequest() {
        _nShowRequests++;

        if (_nShowRequests == 1 && _canBeShown) {
            _spriteCanvasInstance.gameObject.SetActive(true);
        }
    }

    public void RemoveShowRequest() {
        _nShowRequests--;

        if (_nShowRequests <= 0) {
            _nShowRequests = 0;
            _spriteCanvasInstance.gameObject.SetActive(false);
        }
    }

    public void OnDatabaseShowRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            AddShowRequest();
        }

    }

    public void OnDatabaseHideRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            RemoveShowRequest();
        }
    }

    public void OnItemGrabbed() {
        _canBeShown = false;
        _spriteCanvasInstance.gameObject.SetActive(false);
    }

    public void OnItemReleased() {
        _canBeShown = true;
        if (_nShowRequests > 0) {
            _spriteCanvasInstance.gameObject.SetActive(true);
        }
    }

    private void OnDestroy() {

        if (_itemComponentReference != null) {
            _itemComponentReference.GrabEvent -= OnItemGrabbed;
            _itemComponentReference.ReleaseEvent -= OnItemReleased;
        }

        QuestMarkerDatabase.MarkerShowEvent -= OnDatabaseShowRequest;
        QuestMarkerDatabase.MarkerHideEvent -= OnDatabaseHideRequest;

        if (_spriteCanvasInstance != null) {
            Destroy(_spriteCanvasInstance.gameObject);
        }
    }

    // Update is called once per frame
    void Update() {
        _spriteCanvasInstance.transform.position = transform.position + _markerRelativePosition;
    }
}
