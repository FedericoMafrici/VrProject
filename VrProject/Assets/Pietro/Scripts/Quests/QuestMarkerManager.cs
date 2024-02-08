using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestMarkerManager : MonoBehaviour
{
    [SerializeField] private Transform _markerPositionTransform; //transform that will be set as parent of the SpriteRenderer instance
    [SerializeField] private GameObject _markerPrefab;
    [SerializeField] private List<QuestID> _associatedQuests;
    private Dictionary<QuestID, AdditionalQuestInfo> _requestsIDs = new Dictionary<QuestID, AdditionalQuestInfo>(); //Set of QuestsIDs of quests that have requested to show the marker
    private Vector3 _markerRelativePosition; //position of indicator relative to this GameObject
    private bool _itemGrabbed = false;
    private Item _itemComponentReference; //needed in order to make indicators disappear when an item is held
    //private int _nShowRequests = 0;
    private GameObject _markerInstance;
    private bool _isCollected = false; //needed to manage items in collecting point, find a better way
    private int _nCollectingQuests = 0; //needed to manage deposited items, find a better way
    private Tween _rotationTween;

    //needed for up and down motion
    private float _upDownAnchorY;
    private float _upDownAmplitude = .1f;
    private float _upDownFrequency = 2f;

    // Start is called before the first frame update
    void Start() {
        if (_markerPrefab == null) {
            Debug.LogError(transform.name + ": indicator Canvas prefab is not set");
        }

        if (_markerPositionTransform == null) {
            Debug.LogError(transform.name + " quest indicator position is null");
        }

        _markerInstance = Instantiate(_markerPrefab, _markerPositionTransform.position, Quaternion.identity);
        _markerRelativePosition = /*_markerInstance.transform.position*/ _markerPositionTransform.position - transform.position;
        _markerInstance.layer = LayerMask.NameToLayer("Ignore Raycast");
        _markerInstance.SetActive(false);
        _upDownAnchorY = _markerInstance.transform.position.y;

        //rotation motion through DOTween
        float rotationDuration = 2;
        _rotationTween = _markerInstance.transform.DORotate(new Vector3(0f, 360f, 0f), rotationDuration, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);


        /*
        //up and down motion through DOTween
        float floatingHeight = 1f;
        float floatingSpeed = 2f;
        _upDownTween = _markerInstance.transform.DOMoveY(_markerInstance.transform.position.y + floatingHeight, floatingSpeed).SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        */

        QuestMarkerDatabase.MarkerShowEvent += OnDatabaseShowRequest;
        QuestMarkerDatabase.MarkerHideEvent += OnDatabaseHideRequest;

        
        _itemComponentReference = GetComponent<Item>();

        if (_itemComponentReference != null) {
            _itemComponentReference.GrabEvent += OnItemGrabbed;
            _itemComponentReference.ReleaseEvent += OnItemReleased;
            if (_itemComponentReference.isInPlayerHand) {
                _itemGrabbed = true;
            }
        }

        foreach (QuestID questId in _associatedQuests) {
            //check if the QuestID is in the marker database
            if (QuestMarkerDatabase.markerDatabase.ContainsKey(questId)) {
                AddShowRequest(questId, QuestMarkerDatabase.markerDatabase[questId]);
            }
        }

        CheckIfShow();

    }

    private void CheckIfShow() {
        if (CanBeShown()) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        if (_markerInstance != null) {
            if (!_rotationTween.IsPlaying())
                _rotationTween.Play();
            _markerInstance.gameObject.SetActive(true);
        }
    }

    private void Hide() {
        if (_markerInstance != null) {
            if (_rotationTween.IsPlaying())
                _rotationTween.Pause();
            _markerInstance.gameObject.SetActive(false);
        }
    }

    public void SetIsCollected(bool isDeposited) {
        _isCollected = isDeposited;
        CheckIfShow();
    }

    public void AddShowRequest(QuestID questId) {
        AddShowRequest(questId, AdditionalQuestInfo.Default());
    }

    public void AddShowRequest(QuestID questId, AdditionalQuestInfo questInfo) {
        if (_associatedQuests.Contains(questId) && !_requestsIDs.ContainsKey(questId)) {
            _requestsIDs.Add(questId, questInfo);
            if (questInfo.isCollectingQuest) {
                _nCollectingQuests++;
            }
        }

        CheckIfShow();
    }

    public void RemoveShowRequest(QuestID questId) {
        if (_requestsIDs.ContainsKey(questId)) {
            if (_requestsIDs[questId].isCollectingQuest) {
                _nCollectingQuests--;
            }

            _requestsIDs.Remove(questId);
        }

        CheckIfShow();
    }

    public void OnDatabaseShowRequest(MarkerEventArgs args) {
        QuestID questId = args.questId;
        AdditionalQuestInfo info = args.questInfo;

        if (_associatedQuests.Contains(questId)) {
            AddShowRequest(questId, info);
        }

    }

    public void OnDatabaseHideRequest(QuestID questId) {
        if (_associatedQuests.Contains(questId)) {
            RemoveShowRequest(questId);
        }
    }

    public void OnItemGrabbed(Grabbable grabbable) {
        _itemGrabbed = true;
        CheckIfShow();
    }

    public void OnItemReleased(Grabbable grabbabble) {
        _itemGrabbed = false;
        CheckIfShow();
    }

    private void OnDestroy() {

        _rotationTween.Kill();

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

    private void OnDisable() {
        CheckIfShow();
    }

    private void OnEnable() {
        CheckIfShow();
    }

    // Update is called once per frame
    void Update() {
        if (_markerInstance != null) {

            //animate in an up and down motion mantaining _upDownAnchorY as the lowest point in the animation
            _upDownAnchorY = /*_markerPositionTransform.position.y*/ transform.position.y + _markerRelativePosition.y;
            float newY = _upDownAnchorY + _upDownAmplitude * Mathf.Sin(_upDownFrequency * Time.time) + _upDownAmplitude;

            //follow this transform's location but not its scale or rotation (thus simple parenting cannot be used)
            _markerInstance.transform.position = new Vector3(/*_markerPositionTransform.position.x*/transform.position.x + _markerRelativePosition.x, newY, /*_markerPositionTransform.position.z*/ transform.position.z + _markerRelativePosition.z);

        }
    }

    private bool CanBeShown() {
        return ((_nCollectingQuests <= 0 || !_isCollected) && !_itemGrabbed && _requestsIDs.Count > 0 && gameObject.activeSelf);
    }

    public void RemoveAssociatedQuest(QuestID id) {
        //used if, no matter what, the marker won't need to be shown again for a given quest
        RemoveShowRequest(id);
        _associatedQuests.Remove(id);
    }
}
