using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    MovementBehaviour movementBehaviour;
    // Start is called before the first frame update

    void Start() {
        movementBehaviour = new SampleMovementBehaviour();
    }

    // Update is called once per frame
    void Update() {
        movementBehaviour.Move(transform);
    }
}
