using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : ItemConsumable
{
    [SerializeField] private float _interactDistance; //aggiunto da Pietro
    private RaycastManager<FarmingLand> _raycastManager; //aggiunto da Pietro
    private KeyCode _interactKey = KeyCode.Mouse0; //aggiunto da Pietro
   //GameObject to be shown in the scene
    //The crop the seed will yield
    public GameObject seed;
    //Debugging purpose
    public int debug=1;

    void Start() {
        //corpo del metodo aggiunto da Pietro

        base.Start();
        if (_raycastManager == null) { 
            InitRaycastManager();
        }

    }

    void InitRaycastManager() {
        _raycastManager = new RaycastManager<FarmingLand>(_interactDistance, false);
    }

    //aggiunto da Pietro:
    public override UseResult Use(PlayerItemManager itemManager) {
        // get reference to camera in order to determine raycast origin
        Camera playerCamera = itemManager.GetCamera();
        if (_raycastManager == null) {
            InitRaycastManager();
        }

        // generate default return value
        UseResult useResult = new UseResult();
        useResult.itemUsed = false;
        useResult.itemConsumed = false;

        //do raycast through RaycastManager
        bool inputPressed = InputManager.InputsAreEnabled() ? Input.GetKeyDown(_interactKey) : false;
        InteractionResult<FarmingLand> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, CanBePlanted);

        FarmingLand farmingLand = interactionResult.currentInteracted;

        if (interactionResult.canCallBehaviour) {
            farmingLand.Interact(this);
            useResult.itemUsed = true;
            useResult.itemConsumed= true;
        }

        if (interactionResult.enteredRange) {
            //ThrowInRangeEvent();
        } else if (interactionResult.exitedRange) {
            //ThrowOutOfRangeEvent();
        }


        return useResult;
    }

    private bool CanBePlanted(FarmingLand land) {
        return (land.crop == null && land.tree == false);
    }

   
}
