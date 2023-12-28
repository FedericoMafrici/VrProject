using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalPartRemover : MonoBehaviour
{
    [SerializeField] protected Camera _playerCamera;
    [SerializeField] protected RemovableType _targetType;
    [SerializeField] protected float _interactRange = 1.5f;

    public static event EventHandler InRange;
    public static event EventHandler OutOfRange;

    protected virtual void Start() {
        if (_playerCamera == null) { 
            Debug.LogError("no player camera set for " + transform.name);
        }
    }

    protected virtual void RemovePart(RemovablePart toRemove) {
       toRemove.Remove();
    }

    protected virtual bool CanBeRemoved(RemovablePart toRemove) {
        return (!toRemove.IsRemoved()) && (toRemove.GetRemovableType() == _targetType);
    }

    public virtual string GetActionText() {
        return ("");
    }

    protected void ThrowInRangeEvent() {
        if (InRange != null) {
            InRange(this, EventArgs.Empty);
        }
    }

    protected void ThrowOutOfRangeEvent() {
        if (OutOfRange != null) {
            OutOfRange(this, EventArgs.Empty);
        }
    }
}
