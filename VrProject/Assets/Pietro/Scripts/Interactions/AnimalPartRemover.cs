using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimalPartRemover : ItemTool {
    //[SerializeField] protected Camera _playerCamera;
    [SerializeField] protected RemovableType _targetType;
    [SerializeField] protected float _interactRange = 1.5f;
    [SerializeField] private string _clueText;
    [SerializeField] private ClueID _clueID;

    public static event EventHandler<ClueEventArgs> InRemovalRange;
    public static event EventHandler<ClueEventArgs> OutOfRemovalRange;
    public static event EventHandler<ClueEventArgs> RemovalStartedEvent;
    public static event EventHandler<ClueEventArgs> RemovalStoppedEvent;

    public AnimalPartRemover(Item.ItemName itemName) : base(itemName) { }

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
        if (InRemovalRange != null) {
            InRemovalRange(this, new ClueEventArgs(_clueID, _clueText));
        }
    }

    protected void ThrowOutOfRangeEvent() {
        if (OutOfRemovalRange != null) {
            OutOfRemovalRange(this, new ClueEventArgs(_clueID, _clueText));
        }
    }

    protected void ThrowRemStartedEvent() {
        if (RemovalStartedEvent != null) {
            RemovalStartedEvent(this, new ClueEventArgs(_clueID, _clueText));
        }
    }

    protected void ThrowRemStoppedEvent() {
        if (RemovalStoppedEvent != null) {
            RemovalStoppedEvent(this, new ClueEventArgs(_clueID, _clueText));
        }
    }

    private void OnDestroy() {
        ThrowRemStoppedEvent();
        ThrowOutOfRangeEvent();
    }
}
