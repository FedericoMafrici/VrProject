using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public enum MovingState {
    PATROL,
    IN_TARGET_RANGE,
    CLOSE_TO_TARGET,
    VERY_CLOSE_TO_TARGET
}

public enum BehaviourID {
    NONE,
    BASIC_FOLLOW_PATH,
    FOLLOW_TARGET,
    REACH_TARGET,
    PATROL,
    INTEREST
}

public class NPCMover : MonoBehaviour {

    private MovingState _state = MovingState.PATROL;

    [SerializeField] private float _inRangeRadius = 10.0f;
    [SerializeField] private float _closeRadius = 5.0f;
    [SerializeField] private float _veryCloseRadius = 2f;
    [SerializeField] private Animator _animator = null;
    private float _toWaitBeforeMoving = 0.0f;
    private Coroutine _waitingCoroutine = null;
    private FoodEater _foodEater;

    [Header("Patrol Behaviour parameters")]
    [SerializeField] private BoxCollider _patrolArea;
    [SerializeField] private float _minPatrolDelay = 5.0f; //minimum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area
    [SerializeField] private float _maxPatrolDelay = 15.0f; //maximum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area

    [Header("Interests")]
    [SerializeField] private List<TargetType> _interestsList;
    public HashSet<TargetType> InterestsSet = new HashSet<TargetType>();
    private MovementBehaviour _currentBeahviour;

    [Header("Behaviour generator parameters")]
    [SerializeField] BehaviourID _startingSpecialBehaviour;
    BehaviourGenerator _behaviourGenerator;

    public event Action NPCDestinationReachedEvent;

    // Start is called before the first frame update
    void Awake() {
        if (_animator == null) {
            _animator = GetComponent<Animator>();
        }

        _foodEater = GetComponent<FoodEater>();
        NavMeshAgent _agent = GetComponent<NavMeshAgent>();
        if (_agent != null ) {
            _agent.avoidancePriority = UnityEngine.Random.Range(0, 32);
        }

        foreach (TargetType type in _interestsList) {
            InterestsSet.Add(type);
        }

        //SetInterestRadius();
        //adjustInterestRadius();

        _behaviourGenerator = GetComponent<BehaviourGenerator>();
        GenerateStartingBehaviour();

        if (_currentBeahviour == null) {
            //create default behaviour
            PatrolBehaviour tmpPatrolBehaviour = new PatrolBehaviour(transform, _patrolArea, new Vector2(_minPatrolDelay, _maxPatrolDelay));

            if (tmpPatrolBehaviour.HasValidParameters) {
                _currentBeahviour = tmpPatrolBehaviour;
                _state = MovingState.PATROL;
            }

        } else if (!_currentBeahviour.HasValidParameters) {
            Debug.LogWarning("MovementBehaviour for " + transform.name + " has invalid parameters, disabling behaviour");
            _currentBeahviour = null;
        }
     

    }

    // Update is called once per frame
    void Update() {
        
        if (_currentBeahviour != null) {
            /*
            if (_nextBehaviour != null) {
                _movementBehaviour.Delete();
                _movementBehaviour = _nextBehaviour;
                _nextBehaviour = null;
               
            }
            */
            _currentBeahviour.Move();
            UpdateAnimations();
        }
    }

    /*
    void AdjustInterestRadius() {
        BoxCollider bbox = transform.GetComponent<BoxCollider>();


        //interest radiuses must always be greater than 0
        if (_veryCloseRadius < 0) {
            _veryCloseRadius = -_veryCloseRadius;
        }

        if (_closeRadius < 0) {
            _closeRadius = -_closeRadius;
        }

        if (_inRangeRadius < 0) {
            _inRangeRadius = -_inRangeRadius;
        }

        //in range radius must be greater than radius covered by bbox
        if (bbox != null) {
            float maxBboxRadius = Mathf.Max(bbox.size.x, bbox.size.z);

            if (_veryCloseRadius < maxBboxRadius) {
                _veryCloseRadius = maxBboxRadius + 1.0f;
            }
        }

        //radiuses must contain each other
        if (_closeRadius < _veryCloseRadius) {
            _closeRadius = _veryCloseRadius + 2.0f;
        }

        if (_closeRadius < _inRangeRadius) {
            _inRangeRadius = _closeRadius + 3.0f;
        }
    }
    */

    void SetInterestRadius() {
        
        RangeVolume[] rangeVolumes = GetComponentsInChildren<RangeVolume>();
        if (rangeVolumes.Length < 3) {
            Debug.LogWarning(transform.name + ": not enough RangeVolumes associated, using default interest radius values");
            return;
        } else if (rangeVolumes.Length > 3) {
            Debug.LogWarning(transform.name + ": too many RangeVolumes associated, using only first 3 RangeVolumes to define interest radius");
        }

        foreach (RangeVolume rangeVolume in rangeVolumes) {
            rangeVolume.Init();
        }

        //get RangeVolumes, sort them and use their radius to define interest radius
        float[] radiuses = { rangeVolumes[0].GetRadius(), rangeVolumes[1].GetRadius(), rangeVolumes[2].GetRadius() };
        System.Array.Sort(radiuses);

        _veryCloseRadius = radiuses[0];
        _closeRadius = radiuses[1];
        _inRangeRadius= radiuses[2];

        //Debug.Log("Very Close Radius: " + _veryCloseRadius);
        //Debug.Log("Close Radius: " + _closeRadius);
        //Debug.Log("In Range Radius: " + _inRangeRadius);

        /*
        Debug.Log(transform.name + "Very Close Radius: " + _veryCloseRadius);
        Debug.Log(transform.name + "Close Radius: " + _closeRadius);
        Debug.Log(transform.name + "In Range Radius: " + _inRangeRadius);
        */

    }

    private void GenerateStartingBehaviour() {
        if (_behaviourGenerator != null) {
            //Debug.Log("Generating starting behaviour");
            SetBehaviour(_startingSpecialBehaviour);
        }
    }

    public void DestroyTarget(Transform _target) {
        if (_target != null) { 
            Destroy(_target.gameObject);
        }
    }

    public float GetInRangeRadius() {
        return _inRangeRadius;
    }

    public float GetCloseRadius() {
        return _closeRadius;
    }

    public float GetVeryCloseRadius() {
        return _veryCloseRadius;
    }

    public MovingState GetState() {
        return _state;
    }

    public void SetState(MovingState _newState) {
        _state = _newState;
    }

    public BoxCollider GetPatrolArea() {
        return _patrolArea;
    }

    public Vector2 GetDelayBounds() {
        return new Vector2(_minPatrolDelay, _maxPatrolDelay);
    }

    public FoodEater GetFoodEaterReference() {
        return _foodEater;
    }

    public MovementBehaviour GetMovementBehaviour() {
        return _currentBeahviour;
    }

    public void SetBehaviour(BehaviourID id) {
        MovementBehaviour newBehaviour;

        switch (id) {
            case BehaviourID.PATROL:
                newBehaviour = new PatrolBehaviour(transform, _patrolArea, new Vector2(_minPatrolDelay, _maxPatrolDelay));
                break;

            case BehaviourID.INTEREST:
                newBehaviour = null;
                break;

            default:
                if (_behaviourGenerator != null) {
                    newBehaviour = _behaviourGenerator.GenerateBehaviour(id, this);
                } else {
                    newBehaviour = null;
                    Debug.LogWarning(transform.name + " tried to set a MovementBehaviour through Behaviour Generator but Bhevaiour Generator is null");
                }
                break;
        }

        SetBehaviour(newBehaviour);
    }

    public void SetBehaviour(MovementBehaviour movementBehaviour) {
        if (_currentBeahviour != null) {
            _currentBeahviour.Delete();
        }

        if (movementBehaviour != null && movementBehaviour.HasValidParameters) {

            _currentBeahviour = movementBehaviour;
        } else {
            if (movementBehaviour != null) {
                movementBehaviour.Delete();
            }

            _currentBeahviour = null;
        }
    }

    public void StartMoving() {
        if (_currentBeahviour != null)
            _currentBeahviour.Start();
    }

    public void StartMovingDelayed(float seconds) {

        //Start moving after given delay
        if (_toWaitBeforeMoving < seconds) {
            _toWaitBeforeMoving = seconds;
        }

        if (_waitingCoroutine == null) {
            _waitingCoroutine = StartCoroutine(WaitBeforeMoving());
        }

    }

    public void StopMoving(float seconds = 0f) {
        if (_currentBeahviour != null)
            _currentBeahviour.Stop();

        if (seconds > 0f) {
            //Only stop for the given amount of time, then start moving again
            StartMovingDelayed(seconds);

        } else {
            //ensure that the NPC stops indefinetely
            if (_waitingCoroutine != null) {
                StopCoroutine(_waitingCoroutine);
                _waitingCoroutine = null;
            }
        }
    }

    private IEnumerator WaitBeforeMoving() {

        //waits until _toWaitBeforeMoving reaches 0, then starts moving again
        while (_toWaitBeforeMoving > 0f) {
            float waited = _toWaitBeforeMoving;
            _toWaitBeforeMoving -= waited;  
            yield return new WaitForSeconds(waited);
        }

        _waitingCoroutine = null;
        StartMoving();
    }

    public void SetPatrolArea(BoxCollider newArea, bool destroyOld = false, bool changeBehaviour = true) {
        if (newArea != null) { 
            if (destroyOld) {
                Destroy(_patrolArea.gameObject);
            }
            _patrolArea = newArea;
            if (changeBehaviour) {
                SetBehaviour(BehaviourID.PATROL);
            }
        }
    }

    //needed for some escort quests
    public void OnDestinationReached(object sender, EventArgs args) {
        (sender as TargetBehaviour).TargetReachedEvent -= OnDestinationReached;
        if (NPCDestinationReachedEvent != null) {
            NPCDestinationReachedEvent();
        }
    }

    public bool IsTargetReached() {
        if (_currentBeahviour is TargetBehaviour) {
            return (_currentBeahviour as TargetBehaviour).IsTargetReached();
        }

        return false;
    }

    private void UpdateAnimations() {
        if (_animator != null) {
            _animator.SetFloat("speed", _currentBeahviour.GetCurSpeed());
        }
    }

}
