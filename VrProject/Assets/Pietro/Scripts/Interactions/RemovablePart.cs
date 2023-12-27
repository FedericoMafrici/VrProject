using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum RemovableType {
    WOOL,
    MUD
}

public abstract class RemovablePart : MonoBehaviour {
    protected bool _isRemoved;
    protected Renderer _renderer;

    [SerializeField] private RemovableType _type;
    [SerializeField] protected NPCMover _npcMover;

    public event Action PartRemoved;

    protected virtual void Start() {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null) {
            Debug.LogError(transform.name + " no renderer assigned");
        }

        _isRemoved= false;
    }
    public virtual void Remove() {
        if (PartRemoved != null) {
            PartRemoved();
        }

        RemovalStopped();
        _isRemoved = true;
    }

    protected IEnumerator FadeOut() {
        float timeDelta = 0.05f;
        float deltaAlpha = timeDelta;

        bool isCompleted = false;

        while (!isCompleted) {
            Color newColor = _renderer.material.color;
            newColor.a -= deltaAlpha;
            
            if (newColor.a <= 0f ) { 
                newColor.a = 0f;
                isCompleted= true;
            }

            _renderer.material.color = newColor;

            yield return new WaitForSeconds(timeDelta);
        }
        Destroy(gameObject);
    }

    public RemovableType GetRemovableType() {
        return _type;
    }

    public bool IsRemoved() {
        return _isRemoved;
    }

    public virtual void RemovalStarted() {}
    public virtual void RemovalStopped() {}

    protected void MakeNPCStartMoving() {
        if (_npcMover != null) {
            _npcMover.StartMoving();
        } else {
            Debug.LogWarning(transform.name + " is a removable that has no reference to an NPCMover");
        }
    }

    protected void MakeNPCStopMoving(float seconds = 0f) {
        if (_npcMover != null) {
            _npcMover.StopMoving(seconds);
        } else {
            Debug.LogWarning(transform.name + " is a removable that has no reference to an NPCMover");
        }
    }

}
