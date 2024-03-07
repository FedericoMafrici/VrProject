using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpriteManagerWrapper : MonoBehaviour {

    [SerializeField] private SpriteManager _spriteManager;
    private bool _interacting = false;
    private bool _inPetRange = false;
    private bool _inRemovalRange = false;
    private void Start() {
        if (_spriteManager == null) {
            Debug.LogError(transform.name + " no reference to SpriteManager");
        }

        Petter.StartedPetting += OnPettingStarted;
        Petter.StoppedPetting+= OnPettingStopped;
        Petter.InPetRange += WhenInPetRange;
        Petter.OutOfPetRange+= WhenOutOfPetRange;
        AnimalPartRemover.InRemovalRange += WhenInRemovalRange;
        AnimalPartRemover.OutOfRemovalRange += WhenOutOfRemovalRange;
    }

    public void OnPettingStarted(object sender, EventArgs args) {
        _spriteManager.UpdateCurrentSprite(SpriteType.HAND);
        _interacting= true;
    }

    public void OnPettingStopped(object sender, EventArgs args) {
        SpriteType st = _inPetRange ? SpriteType.INTERACT_DOT : SpriteType.DOT;
        _spriteManager.UpdateCurrentSprite(st);
        _interacting= false;
    }

    public void WhenInPetRange(object sender, EventArgs args) {
        if (!_interacting) {
            _spriteManager.UpdateCurrentSprite(SpriteType.INTERACT_DOT);
        }
        _inPetRange= true;
    }

    public void WhenOutOfPetRange(object sender, EventArgs args) {
        if (!_inRemovalRange) {
            _spriteManager.UpdateCurrentSprite(SpriteType.DOT);
        }
        _inPetRange = false;
    }

    public void WhenInRemovalRange(object sender, EventArgs args) {
        if (!_interacting) {
            _spriteManager.UpdateCurrentSprite(SpriteType.INTERACT_DOT);
        }
        _inRemovalRange = true;
    }

    public void WhenOutOfRemovalRange(object sender, EventArgs args) {
        if (!_inPetRange) {
            _spriteManager.UpdateCurrentSprite(SpriteType.DOT);
        }
        _inRemovalRange = false;
    }

}
