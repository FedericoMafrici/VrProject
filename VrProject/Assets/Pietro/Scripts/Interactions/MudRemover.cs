using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudRemover : AnimalPartRemover {

    private RubManager<RemovablePart> _rubber;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        _rubber = new RubManager<RemovablePart>(_playerCamera, _interactRange);
        _targetType = RemovableType.MUD;
    }

    // Update is called once per frame
    void Update() {
        RubbingResult<RemovablePart> rubResult = _rubber.CheckRubs(KeyCode.Mouse0);

        RemovablePart toRemove = rubResult.currentRubbed;
        RemovablePart previousRemovable = rubResult.previousRubbed; 
        if (rubResult.canCallBehaviour && toRemove != null && CanBeRemoved(toRemove)) {
            RemovePart(toRemove);
        }


        if (rubResult.previousRayDidHit && !rubResult.currentRayDidHit) {
            //went out of range
        } else if (rubResult.currentRayDidHit && !rubResult.previousRayDidHit) {
            //went in range
        }

        if (rubResult.didRub) {
            if (previousRemovable != toRemove) {
                if (previousRemovable != null) {
                    previousRemovable.RemovalStopped();
                }
                toRemove.RemovalStarted();
            }
        } else if (previousRemovable != null) {
            previousRemovable.RemovalStopped();
        }

    }
}
