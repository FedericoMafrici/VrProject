using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleMovementBehaviour : MovementBehaviour {
    public override void Move(Transform objectToMove) {
        if (Input.GetKey(KeyCode.W)) {
            objectToMove.Translate(objectToMove.forward * 1 * Time.deltaTime, Space.World);
        }
    }

}
