using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public enum MovingState {
    PATROL,
    IN_TARGET_RANGE,
    CLOSE_TO_TARGET,
    VERY_CLOSE_TO_TARGET
}
public class NPCMover : MonoBehaviour {

    private MovingState _state = MovingState.PATROL;

    [SerializeField] private Transform _target;
    [SerializeField] private BoxCollider _patrolArea;

    [SerializeField] private float _minPatrolDelay = 5.0f; //minimum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area
    [SerializeField] private float _maxPatrolDelay = 15.0f; //maximum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area

    private float _inRangeRadius = 5.0f;
    private float _closeRadius = 2.0f;
    private float _veryCloseRadius = .5f;
    [SerializeField] private List<TargetType> _interestsList;
    public HashSet<TargetType> InterestsSet = new HashSet<TargetType>();

    [SerializeField] private MovementBehaviour _movementBehaviour;
    private MovementBehaviour _nextBehaviour;

    // Start is called before the first frame update
    void Start() {
        foreach (TargetType type in _interestsList) {
            InterestsSet.Add(type);
        }

        setInterestRadius();
        //adjustInterestRadius();

        if (_movementBehaviour == null) {
            //create default behaviour
            PatrolBehaviour tmpPatrolBehaviour = new PatrolBehaviour(transform, _patrolArea, new Vector2(_minPatrolDelay, _maxPatrolDelay));

            if (tmpPatrolBehaviour.HasValidParameters) {
                _movementBehaviour = tmpPatrolBehaviour;
                _state = MovingState.PATROL;
            } else {
                _movementBehaviour = null;
            }
        } else {
            if(!_movementBehaviour.HasValidParameters) {
                Debug.LogWarning("MovementBehaviour for " + transform.name + " has invalid parameters, disabling behaviour");
                _movementBehaviour = null;
            }
        }

        _nextBehaviour = null;
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

            _movementBehaviour.Move(transform);
        }
    }

    void adjustInterestRadius() {
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

    void setInterestRadius() {
        
        RangeVolume[] rangeVolumes = GetComponentsInChildren<RangeVolume>();
        if (rangeVolumes.Length < 3) {
            Debug.LogWarning(transform.name + ": not enough RangeVolumes associated, using default interest radius values");
            return;
        } else if (rangeVolumes.Length > 3) {
            Debug.LogWarning(transform.name + ": too many RangeVolumes associated, using only first 3 RangeVolumes to define interest radius");
        }

        //get RangeVolumes, sort them and use their radius to define interest radius
        float[] radiuses = { rangeVolumes[0].getRadius(), rangeVolumes[1].getRadius(), rangeVolumes[2].getRadius() };
        System.Array.Sort(radiuses);

        _veryCloseRadius = radiuses[0];
        _closeRadius = radiuses[1];
        _inRangeRadius= radiuses[2];

        /*
        Debug.Log(transform.name + "Very Close Radius: " + _veryCloseRadius);
        Debug.Log(transform.name + "Close Radius: " + _closeRadius);
        Debug.Log(transform.name + "In Range Radius: " + _inRangeRadius);
        */

    }

    public void DestroyTarget(Transform _target) {
        if (_target != null) { 
            Destroy(_target.gameObject);
        }
    }

    public float getInRangeRadius() {
        return _inRangeRadius;
    }

    public float getCloseRadius() {
        return _closeRadius;
    }

    public float getVeryCloseRadius() {
        return _veryCloseRadius;
    }

    public MovingState getState() {
        return _state;
    }

    public void setState(MovingState _newState) {
        _state = _newState;
    }

    public BoxCollider getPatrolArea() {
        return _patrolArea;
    }

    public Vector2 getDelayBounds() {
        return new Vector2(_minPatrolDelay, _maxPatrolDelay);
    }

    public void setNextBehaviour(MovementBehaviour movementBehaviour) {
        if (movementBehaviour.HasValidParameters) {
            _nextBehaviour = movementBehaviour;
        } else {
            _nextBehaviour = null;  
        }
    }

    public void setBehaviour(MovementBehaviour movementBehaviour) {
        if (movementBehaviour.HasValidParameters) {
            if (_movementBehaviour != null) {
                _movementBehaviour.Delete();
            }
            _movementBehaviour = movementBehaviour;
        } else {
            Debug.LogWarning(transform.name + " tried to set behaviour to an invalid one, behaviour was not set");
        }
    }

}
