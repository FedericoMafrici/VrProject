using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour {

    [SerializeField] private Transform _target;
    [SerializeField] private BoxCollider _patrolArea;
    [SerializeField] private float _minPatrolSecondsDelay = 1.0f; //minimum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area
    [SerializeField] private float _maxPatrolSecondsDelay = 10.0f; //maximum amount of time (in seconds) that needs to pass before generating a new target when patrolling an area

    private MovementBehaviour _movementBehaviour;


    // Start is called before the first frame update
    void Start() {

        PatrolBehaviour tmpPatrolBehaviour = new PatrolBehaviour(transform, _patrolArea, new Vector2(_minPatrolSecondsDelay, _maxPatrolSecondsDelay));
        if (tmpPatrolBehaviour.HasValidParameters) {
            StartCoroutine(tmpPatrolBehaviour.updateTarget());
            _movementBehaviour = tmpPatrolBehaviour;
        } else {
            _movementBehaviour = null;
        }
    }

    // Update is called once per frame
    void Update() {
        if (_movementBehaviour != null) {
            _movementBehaviour.Move(transform);
        }
    }

  
}
