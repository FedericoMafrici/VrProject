using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalFood : ItemConsumable {
    [SerializeField] private string _clueText = "Premi [CLICK SINISTRO] per dare da mangiare";
    [SerializeField] private float _interactRange = .5f;
    private RaycastManager<FoodEater> _raycastManager;
    private KeyCode _interactKey = KeyCode.Mouse0;

    public static event EventHandler<ClueEventArgs> InFeedRange;
    public static event EventHandler<ClueEventArgs> OutOfFeedRange;

    // Start is called before the first frame update
    public void Start() {
        base.Start();
        InitRaycastManager();
    }


    void InitRaycastManager() {
        _raycastManager = new RaycastManager<FoodEater>(_interactRange, false);
    }

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
        bool inputPressed = InputManager.InteractionsAreEnabled() ? Input.GetMouseButtonDown(0) : false;
        InteractionResult<FoodEater> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, FoodInterestsEater, LayerMask.GetMask("Animals"));

        FoodEater eater = interactionResult.currentInteracted;

        //check result of raycast and determine if food can be given to animal
        //update return value accordingly
        if (interactionResult.canCallBehaviour) {
            eater.ForceEatFood(this);
            useResult.itemUsed = true;
            useResult.itemConsumed = true;
            //RemovePart(result.currentInteracted);
        }

        if (interactionResult.enteredRange) {
            if (InFeedRange != null) {
                InFeedRange(this, new ClueEventArgs(ClueID.FEED, _clueText));
            }
        } else if (interactionResult.exitedRange) {
            if (OutOfFeedRange != null) {
                OutOfFeedRange(this, new ClueEventArgs(ClueID.FEED, _clueText));
            }
        }

        return useResult;
    }

    private bool FoodInterestsEater(FoodEater eater) {
        return eater.FoodInterestsAnimal(this);
    }

    void OnDestroy() {
        if (OutOfFeedRange != null) {
            OutOfFeedRange(this, new ClueEventArgs(ClueID.FEED, _clueText));
        }
    }

}
