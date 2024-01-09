using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemManager : MonoBehaviour {
    [SerializeField] Camera _playerCamera;
    private Hotbar _hotbar;
    private PlayerPickUpDrop _playerPickUp;


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
            if (heldItem is AnimalPartRemover) {
                (heldItem as AnimalPartRemover).CheckInteraction(_playerCamera);
            } else if (heldItem is AnimalFood) {
                AnimalFood food = heldItem as AnimalFood;
                if(food.CheckInteraction( _playerCamera)) {
                    _playerPickUp.Drop(false);
                }
            }
        }
        
    }
}
