using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.AI;


public enum MovingState {
    PATROL,
    IN_TARGET_RANGE,
    CLOSE_TO_TARGET,
    VERY_CLOSE_TO_TARGET
}

public enum SpecialBehaviourID {
    BASIC_FOLLOW_PATH
}

public class NPCMover : MonoBehaviour {

    private MovingState _state = MovingState.PATROL;

    private float _inRangeRadius = 5.0f;
    private float _closeRadius = 2.0f;
    private float _veryCloseRadius = .5f;
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
    private MovementBehaviour _movementBehaviour;

    [Header("Behaviour generator parameters")]
    [SerializeField] SpecialBehaviourID _startingSpecialBehaviour;
    SpecialBehaviourGenerator _specialBehaviourGenerator;

    // Start is called before the first frame update
    void Start() {
        _foodEater = GetComponent<FoodEater>();

        foreach (TargetType type in _interestsList) {
            InterestsSet.Add(type);
        }

        SetInterestRadius();
        //adjustInterestRadius();

        _specialBehaviourGenerator = GetComponentInChildren<SpecialBehaviourGenerator>();
        GenerateStartingBehaviour();

        if (_movementBehaviour == null) {
            //create default behaviour
            PatrolBehaviour tmpPatrolBehaviour = new PatrolBehaviour(transform, _patrolArea, new Vector2(_minPatrolDelay, _maxPatrolDelay));

            if (tmpPatrolBehaviour.HasValidParameters) {
                _movementBehaviour = tmpPatrolBehaviour;
                _state = MovingState.PATROL;
            }

        } else if (!_movementBehaviour.HasValidParameters) {
            Debug.LogWarning("MovementBehaviour for " + transform.name + " has invalid parameters, disabling behaviour");
            _movementBehaviour = null;
        }
     

    }

    // Update is called once per frame
    void Update() {
        
        if (_movementBehaviour != null) {
            /*
            if (_nextBehaviour != null) {
                _movementBehaviour.Delete();
                _movementBehaviour = _nextBehaviour;
                _nextBehaviour = null;
               
            }
            */

            _movementBehaviour.Move();
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

        Debug.Log("Very Close Radius: " + _veryCloseRadius);
        Debug.Log("Close Radius: " + _closeRadius);
        Debug.Log("In Range Radius: " + _inRangeRadius);

        /*
        Debug.Log(transform.name + "Very Close Radius: " + _veryCloseRadius);
        Debug.Log(transform.name + "Close Radius: " + _closeRadius);
        Debug.Log(transform.name + "In Range Radius: " + _inRangeRadius);
        */

    }

    private void GenerateStartingBehaviour() {
        if (_specialBehaviourGenerator != null) {
            Debug.Log("Generating starting behaviour");
            _movementBehaviour = _specialBehaviourGenerator.GenerateBehaviour(_startingSpecialBehaviour, this);
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

    public void SetBehaviour(MovementBehaviour movementBehaviour) {
        if (movementBehaviour.HasValidParameters) {
            if (_movementBehaviour != null) {
                _movementBehaviour.Delete();
            }
            _movementBehaviour = movementBehaviour;
        } else {
            Debug.LogWarning(transform.name + " tried to set behaviour to an invalid one, behaviour was not set");
        }
    }

    public void StartMoving() {
        if (_movementBehaviour != null)
            _movementBehaviour.Start();
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
        if (_movementBehaviour != null)
            _movementBehaviour.Stop();

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

}
