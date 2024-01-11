using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : ItemTool {
    private RaycastManager<FarmingLand> _raycastManager;
    [SerializeField] private float _interactDistance;
    private KeyCode _interactKey = KeyCode.Mouse0;

    WateringCan() : base(ItemName.WateringCan) {
        _raycastManager = new RaycastManager<FarmingLand>(_interactDistance, false);
    }

    // Start is called before the first frame update
    void Start() {
        if (itemName != ItemName.WateringCan) {
            Debug.LogWarning(transform.name + ": itemName should be " + ItemName.WateringCan + " but is not");
        }
        _raycastManager = new RaycastManager<FarmingLand>(_interactDistance, false);
    }

    public override UseResult Use(PlayerItemManager itemManager) {
        // get reference to camera in order to determine raycast origin
        Camera playerCamera = itemManager.GetCamera();

        // generate default return value
        UseResult useResult = new UseResult();
        useResult.itemUsed = false;
        useResult.itemConsumed = false;

        //do raycast through RaycastManager
        bool inputPressed = InputManager.InputsAreEnabled() ? Input.GetKeyDown(_interactKey) : false;
        InteractionResult<FarmingLand> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, LandCanBeWatered);

        FarmingLand farmingLand = interactionResult.currentInteracted;

        if (interactionResult.canCallBehaviour) {
            farmingLand.Interact(null);
            useResult.itemUsed = true;
        }

        if (interactionResult.enteredRange) {
            //ThrowInRangeEvent();
        } else if (interactionResult.exitedRange) {
            //ThrowOutOfRangeEvent();
        }

        return useResult;
    }

    private bool LandCanBeWatered(FarmingLand land) {
        return (land.crop != null);
    }
}
