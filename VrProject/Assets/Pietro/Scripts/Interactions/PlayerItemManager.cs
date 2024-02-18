using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

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
        bool itemUsed = false;
        bool itemConsumed = false;
        bool suppressSound = false;

        if ( heldItem != null ) {
            Item[] itemComponents = heldItem.GetComponents<Item>();
            //get all the "Item" components on the object and call their "Use" method one by one
            foreach (Item itemComponent in itemComponents) {
                //Debug.LogWarning("Calling Use of " +  itemComponent);
                UseResult useResult = itemComponent.Use(this);

                if (!itemUsed && useResult.itemUsed) {
                    itemUsed = true;
                }

                if (!itemConsumed && useResult.itemConsumed) {
                    itemConsumed = true;
                }

                if (!suppressSound && useResult.supressSound) {
                    suppressSound = true;
                }
            }

            //this is out of the for loop in order to execute the behaviour following use/consume exactly once
            if (itemUsed) {
                if (!suppressSound) {
                    heldItem.gameObject.GetComponent<AudioSource>().clip = heldItem.usageSound;
                    heldItem.gameObject.GetComponent<AudioSource>().Play();
                }
                //throw used event
                if (ItemUsed != null) {
                    ItemUsed(this, new UsedItemEventArgs(heldItem));
                }
            }

            if (itemConsumed) {

                //consume item
                //remove from hotbar if needed
                _hotbar.activeItemWrapper.amount--;
                _playerPickUp.ThrowDropEvent(_hotbar.activeItemObj);
                _hotbar.Deselect();
                Destroy(heldItem.gameObject);
                if (_hotbar.activeItemObj)
                    _hotbar.activeItemObj = null;
                if (_hotbar.activeItemWrapper.amount == 0) {
                    _hotbar.Remove(_hotbar.activeItemWrapper);
                }
            }
        }
        
    }

    public Camera GetCamera() {
        return _playerCamera;
    }

    public PlayerPickUpDrop GetPlayerPickUpDrop() {
        return _playerPickUp;
    }
}

public class UsedItemEventArgs : EventArgs {
    public Item usedItem;
    public UsedItemEventArgs(Item item) {
        usedItem = item;
    }
}
