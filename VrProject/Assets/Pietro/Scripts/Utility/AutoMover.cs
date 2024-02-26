using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMover : MonoBehaviour
{
    [SerializeField] Vector3 _movementDirection = Vector3.zero;
    [SerializeField] float _speed = 2f;
    [SerializeField] float _maxTravelDistance = 10f;
    [SerializeField] bool distancWrtObject;
    [SerializeField] Transform distanceReference;
    private Vector3 _startPosition;

    void Start()
    {

        if (_movementDirection != Vector3.zero) {
            _movementDirection = _movementDirection.normalized;
        }

        _startPosition = transform.position;

        if (distancWrtObject) {
            _maxTravelDistance = Mathf.Abs(Vector3.Distance(distanceReference.position, _startPosition));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object in the specified direction at the specified speed
        transform.Translate(_movementDirection * _speed * Time.deltaTime);

        // Check if the object has traveled the maximum distance
        if (Mathf.Abs(Vector3.Distance(_startPosition, transform.position)) >= _maxTravelDistance) {
            // Destroy the object
            Destroy(transform.gameObject);
            Destroy(gameObject);
        }
    }
}
