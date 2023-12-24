using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSpriteManager : SpriteManager {
     protected override void Start() {
        base.Start();
        Petter.StartedPetting += OnPettingStarted;
        Petter.StoppedPetting+= OnPettingStopped;
        Petter.InPetRange += WhenInPetRange;
        Petter.OutOfPetRange+= WhenOutOfPetRange;
    }

    public void OnPettingStarted(object sender, EventArgs args) {
        UpdateCurrentSprite(SpriteType.HAND);
    }

    public void OnPettingStopped(object sender, EventArgs args) {
        UpdateCurrentSprite(SpriteType.DOT);
    }

    public void WhenInPetRange(object sender, EventArgs args) {
        if (_currentSpriteData.Type == SpriteType.DOT) {
            SetCurrentSpriteColor(Color.green);
        }
    }

    public void WhenOutOfPetRange(object sender, EventArgs args) {
        if (_currentSpriteData.Type == SpriteType.DOT) {
            ResetCurrentSpriteColor();
        }
    }

}
