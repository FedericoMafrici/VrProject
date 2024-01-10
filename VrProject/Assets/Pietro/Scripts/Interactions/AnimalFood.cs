using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFood : ItemConsumable {
    [SerializeField] private float _interactRange;
    private RaycastManager<FoodEater> _raycastManager;
    private KeyCode _interactKey = KeyCode.Mouse0;
    // Start is called before the first frame update
    public void Start() {
        base.Start();
        _raycastManager = new RaycastManager<FoodEater>(_interactRange, false);
    }

    public override UseResult Use(PlayerItemManager itemManager) { 
        // get reference to camera in order to determine raycast origin
        Camera playerCamera = itemManager.GetCamera();

        // generate default return value
        UseResult useResult = new UseResult();
        useResult.itemUsed = false;
        useResult.itemConsumed = false;

        //do raycast through RaycastManager
        bool inputPressed = Input.GetKeyDown(_interactKey);
        InteractionResult<FoodEater> interactionResult = _raycastManager.CheckRaycast(playerCamera, _interactKey, inputPressed);

        FoodEater eater = interactionResult.currentInteracted;

        //check result of raycast and determine if food can be given to animal
        //update return value accordingly
        if (interactionResult.canCallBehaviour && eater.FoodInterestsAnimal(this)) {
            eater.ForceEatFood(this);
            useResult.itemUsed = true;
            useResult.itemConsumed = true;
            //RemovePart(result.currentInteracted);
        }

        if (interactionResult.enteredRange && eater.FoodInterestsAnimal(this)) {
            //ThrowInRangeEvent();
        } else if (interactionResult.exitedRange /*&& result.previousInteracted!= null && CanBeRemoved(result.previousInteracted)*/) {
            //ThrowOutOfRangeEvent();
        }

        return useResult;
    }


}
