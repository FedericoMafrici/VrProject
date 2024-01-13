using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerItemManager : MonoBehaviour {
    [SerializeField] Camera _playerCamera;
    private Hotbar _hotbar;
    private PlayerPickUpDrop _playerPickUp;

    public event EventHandler<UsedItemEventArgs> ItemUsed;

    // Start is called before the first frame update
    void Start() {
        if (_playerCamera == null) {
            Debug.LogError("no player camera set for " + transform.name);
        }

        _playerPickUp = GetComponent<PlayerPickUpDrop>();
        if (_playerPickUp != null) {
            _hotbar = _playerPickUp.hotbar;
        } else {
            Debug.LogError(transform.name + " no PlayerPickUpDrop component");
        }
    }

    // Update is called once per frame
    void Update() {
        Item heldItem = _hotbar.activeItemObj;

        if ( heldItem != null ) {
            UseResult useResult = heldItem.Use(this);
            if (useResult.itemUsed) {
                //throw used event
                if (ItemUsed != null) {
                    ItemUsed(this, new UsedItemEventArgs(heldItem));
                }
            }


            if (useResult.itemConsumed) {

                //consume item
                //remove from hotbar if needed
                _hotbar.activeItemWrapper.amount--;
                _playerPickUp.ThrowDropEvent(_hotbar.activeItemObj);
                if (_hotbar.activeItemWrapper.amount == 0) {
                    _hotbar.Remove(_hotbar.activeItemWrapper);
                    _hotbar.Deselect();
                    Destroy(heldItem.gameObject);

                    if (_hotbar.activeItemObj)
                        _hotbar.activeItemObj = null;

                    
                }
            }
        }
        
    }

    public Camera GetCamera() {
        return _playerCamera;
    }
}

public class UsedItemEventArgs : EventArgs {
    public Item usedItem;
    public UsedItemEventArgs(Item item) {
        usedItem = item;
    }
}
