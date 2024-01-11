using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour
{
    [SerializeField] private Transform _indicatorParentTransform; //transform that will be set as parent of the SpriteRenderer instance
    [SerializeField] private Canvas _spriteCanvasPrefab;
    [SerializeField] private List<QuestID> _associatedQuests;
    private Vector3 _indicatorRelativePosition; //position of indicator relative to this GameObject
    private bool _canBeShown = true;
    private Item _itemComponentReference; //needed in order to make indicators disappear when an item is held
    private int _nShowRequests = 0;
    private Canvas _spriteCanvasInstance; 

    // Start is called before the first frame update
    void Start() {
        if (_spriteCanvasPrefab == null) {
            Debug.LogError(transform.name + ": indicator Canvas prefab is not set");
        }

        if (_indicatorParentTransform == null) {
            Debug.LogError(transform.name + " quest indicator position is null");
        }

        _spriteCanvasInstance = Instantiate(_spriteCanvasPrefab, _indicatorParentTransform.position, Quaternion.identity);
        _indicatorRelativePosition = _spriteCanvasInstance.transform.position - transform.position;
        //_spriteCanvasInstance.transform.parent = _indicatorParentTransform;
        

        QuestIndicatorDatabase.IndicatorShowEvent += OnShowRequest;
        QuestIndicatorDatabase.IndicatorHideEvent += OnHideRequest;

        foreach (QuestID questId in _associatedQuests) {
            //first check if the QuestID is in the indicator database
            if(QuestIndicatorDatabase.indicatorDatabase.Contains(questId)) {
                _nShowRequests++;
            }
        }

        _itemComponentReference = GetComponent<Item>();
        _itemComponentReference.GrabEvent += OnItemGrabbed;
        _itemComponentReference.ReleaseEvent += OnItemReleased;
        if (_itemComponentReference != null && _itemComponentReference.isInPlayerHand) {
            _canBeShown = false;
        }

        _spriteCanvasInstance.gameObject.SetActive(_nShowRequests > 0 && _canBeShown); //enable component if there's at least one show request
    }

    public void OnShowRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            _nShowRequests++;
        }
        if (_nShowRequests == 1 && _canBeShown) {
            _spriteCanvasInstance.gameObject.SetActive(true);
        }
    }

    public void OnHideRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            _nShowRequests--;
        }

        if (_nShowRequests <= 0) {
            _nShowRequests = 0;
            _spriteCanvasInstance.gameObject.SetActive(false);
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

        QuestIndicatorDatabase.IndicatorShowEvent -= OnShowRequest;
        QuestIndicatorDatabase.IndicatorHideEvent -= OnHideRequest;
        Destroy(_spriteCanvasInstance.gameObject);
    }

    // Update is called once per frame
    void Update() {
        _spriteCanvasInstance.transform.position = transform.position + _indicatorRelativePosition;
    }
}
