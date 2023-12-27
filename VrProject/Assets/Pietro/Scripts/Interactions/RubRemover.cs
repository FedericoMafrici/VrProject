using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubRemover : AnimalPartRemover {

    private RubManager<RemovablePart> _rubber;
    protected override void Start() {
        base.Start();
        _rubber = new RubManager<RemovablePart>(_playerCamera, _interactRange);
    }

    // Update is called once per frame
    void Update() {
        RubbingResult<RemovablePart> rubResult = _rubber.CheckRubs(KeyCode.Mouse0);

        RemovablePart toRemove = rubResult.currentRubbed;
        RemovablePart previousRemovable = rubResult.previousRubbed; 
        if (rubResult.canCallBehaviour && toRemove != null && CanBeRemoved(toRemove)) {
            RemovePart(toRemove);
        }


        if (rubResult.enteredRange) {
            //went in range
        } else if (rubResult.exitedRange) {
            //went out of range
        }

        if (rubResult.interactedWithNewTarget) {
            toRemove.RemovalStarted();
        }

        if (rubResult.abandonedPreviousTarget) {
            previousRemovable.RemovalStopped();
        }

        if (rubResult.previousRayDidHit && !rubResult.currentRayDidHit) {

        } else if (rubResult.currentRayDidHit && !rubResult.previousRayDidHit) {

        }


        /*if (rubResult.didRub) {
            if (previousRemovable != toRemove) {
                if (previousRemovable != null) {
                    previousRemovable.RemovalStopped();
                }
                toRemove.RemovalStarted();
            }
        } else if (previousRemovable != null) {
            previousRemovable.RemovalStopped();
        }*/

    }
}
