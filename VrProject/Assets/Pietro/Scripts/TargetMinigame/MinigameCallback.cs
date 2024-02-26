using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigameCallback : MonoBehaviour {

    public abstract void Init(TargetMinigame minigame);

    public virtual void ProgressCallback() {

    }

    public virtual void BeginCallback() {

    }

    public virtual void EndCallback(bool success) {

    }

    public virtual void SpawnTargetCallback(Target target, int numSpawnedTargets) {

    }

}
