using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpriteManager : SpriteManager {
    private bool _petting = false;
    private bool _inRange = false;
     protected override void Start() {
        base.Start();
        Petter.StartedPetting += OnPettingStarted;
        Petter.StoppedPetting+= OnPettingStopped;
        Petter.InPetRange += WhenInPetRange;
        Petter.OutOfPetRange+= WhenOutOfPetRange;
    }

    public void OnPettingStarted(object sender, EventArgs args) {
        UpdateCurrentSprite(SpriteType.HAND);
        _petting= true;
    }

    public void OnPettingStopped(object sender, EventArgs args) {
        SpriteType st = _inRange ? SpriteType.INTERACT_DOT : SpriteType.DOT;
        UpdateCurrentSprite(st);
        _petting= false;
    }

    public void WhenInPetRange(object sender, EventArgs args) {
        if (!_petting) {
            UpdateCurrentSprite(SpriteType.INTERACT_DOT);
        }
        _inRange= true;
    }

    public void WhenOutOfPetRange(object sender, EventArgs args) {
            UpdateCurrentSprite(SpriteType.DOT);
        _inRange= false;
    }

}
