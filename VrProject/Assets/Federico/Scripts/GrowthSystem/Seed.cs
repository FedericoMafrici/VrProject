using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Seed : ItemTool
{
    [SerializeField] private string _clueText = "Premi [CLICK SINISTRO] per piantare"; //aggiunto da Pietro
    [SerializeField] private float _interactDistance; //aggiunto da Pietro
    private RaycastManager<FarmingLand> _raycastManager; //aggiunto da Pietro
    private KeyCode _interactKey = KeyCode.Mouse0; //aggiunto da Pietro
   //GameObject to be shown in the scene
    //The crop the seed will yield
    public GameObject crop;
    public float offset=0.89f;
     public Vector3 coordinate;
    //Debugging purpose
    public int debug=1;

    public static event EventHandler<ClueEventArgs> InLandRange; //aggiutno da Pietro
    public static event EventHandler<ClueEventArgs> OutOfLandRange; //aggiunto da Pietro


    Seed(Item.ItemName name) : base(name) { }

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
        bool inputPressed = InputManager.InteractionsAreEnabled() ? Input.GetMouseButtonDown(0) : false;
        InteractionResult<FarmingLand> interactionResult = _raycastManager.CheckRaycast(playerCamera, inputPressed, CanBePlanted);

        FarmingLand farmingLand = interactionResult.currentInteracted;

        if (interactionResult.canCallBehaviour) {
            farmingLand.Interact(this);
            useResult.itemUsed = true;
            useResult.itemConsumed= false;
        }

        if (interactionResult.enteredRange) {
            if (InLandRange != null) {
                InLandRange(this, new ClueEventArgs(ClueID.PLANT, _clueText));
            }
        } else if (interactionResult.exitedRange) {
            if (OutOfLandRange != null) {
                OutOfLandRange(this, new ClueEventArgs(ClueID.PLANT, _clueText));
            }
        }

        
        return useResult;
        
    }

    private bool CanBePlanted(FarmingLand land) {
        return (land.crop == null && land.tree == false);
    }

    private void OnDestroy() {
        if (OutOfLandRange != null) {
            OutOfLandRange(this, new ClueEventArgs(ClueID.PLANT, _clueText));
        }
    }
}
