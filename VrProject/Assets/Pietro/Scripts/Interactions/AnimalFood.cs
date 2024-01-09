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

    public bool CheckInteraction(Camera playerCamera) {
        bool foodConsumed = false;
        bool inputPressed = Input.GetKey(_interactKey);
        InteractionResult<FoodEater> result = _raycastManager.CheckRaycast(playerCamera, _interactKey, inputPressed);

        FoodEater eater = result.currentInteracted;

        if (result.canCallBehaviour && eater.FoodInterestsAnimal(this)) {
            eater.ForceEatFood(this);
            foodConsumed = true;
            //RemovePart(result.currentInteracted);
        }

        if (result.enteredRange && eater.FoodInterestsAnimal(this)) {
            //ThrowInRangeEvent();
        } else if (result.exitedRange /*&& result.previousInteracted!= null && CanBeRemoved(result.previousInteracted)*/) {
            //ThrowOutOfRangeEvent();
        }

        return foodConsumed;
    }


}
