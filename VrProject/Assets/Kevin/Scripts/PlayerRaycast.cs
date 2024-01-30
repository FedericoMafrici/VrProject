using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//aggiunto da Pietro
public enum ClueID {
    PET,
    FEED,
    CLEAN,
    SHEAR,
    HEAL,
    PLANT,
    WATER
}

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
            
    [SerializeField] private TMP_Text clue;
    [SerializeField] public Hotbar hotbar;
    
    private const float pickupDistance = 5f;
    private Item item;
    private Transform _previousOutlined = null; //aggiunto da Pietro
    public Deposit deposit;

    public Dictionary<ClueID, string> _addedClues = new Dictionary<ClueID, string>(); //aggiunto da Pietro

    private void Start() {
        //petter events
        Petter.InPetRange += OnAddClueEvent;
        Petter.StoppedPetting += OnAddClueEvent;
        Petter.OutOfPetRange += OnRemoveClueEvent;
        Petter.StartedPetting += OnRemoveClueEvent;

        //food events
        AnimalFood.InFeedRange += OnAddClueEvent;
        AnimalFood.OutOfFeedRange += OnRemoveClueEvent;

        //animal remover events
        AnimalPartRemover.InRemovalRange += OnAddClueEvent;
        AnimalPartRemover.RemovalStoppedEvent += OnAddClueEvent;
        AnimalPartRemover.OutOfRemovalRange += OnRemoveClueEvent;
        AnimalPartRemover.RemovalStartedEvent -= OnRemoveClueEvent;

        //planting events
        Seed.InLandRange += OnAddClueEvent;
        Seed.OutOfLandRange += OnRemoveClueEvent;

        //watering events
        WateringCan.InWateringRange += OnAddClueEvent;
        WateringCan.OutOfWateringRange += OnRemoveClueEvent;
    }

    void Update()
    {
        Transform toOutline = null; //aggiunto da Pietro
        RaycastHit raycastHit;
        RaycastHit raycastHit2;

        // ------------- AGGIORNAMENTO INFORMAZIONI TESTUALI SOTTO IL CURSORE -------------

        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out raycastHit,
                pickupDistance, pickupLayerMask)
            && raycastHit.transform.TryGetComponent(out Item item)
            && hotbar.activeItemObj == null)
        {
            clue.text = "Press E to grab\nPress Q to collect";
            if (hotbar.firstEmpty == 6)
            {
                clue.text += "\n\n Release an item to grab or collect another object!";
            }
            toOutline = raycastHit.transform;
        }
        
        else if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out raycastHit2,
                     pickupDistance, pickupLayerMask)
                 && raycastHit2.transform.TryGetComponent(out Item item2)
                 && hotbar.activeItemObj != null
                 && item2 != hotbar.activeItemObj)
        {
            clue.text = "Press Q to collect";
            if (hotbar.firstEmpty == 6)
            {
                clue.text += "\n\n Release an item to collect another object!";
            }
            toOutline = raycastHit2.transform;
        }
        
        else if (Vector3.Distance(deposit.transform.position, playerCameraTransform.position) < 10
                 && hotbar.activeItemObj != null)
        {
            clue.text = "Press Q to release the item";
        }
        
        else
            clue.text = "";

        //aggiunto da Pietro
        foreach(string clueText in _addedClues.Values) {
            clue.text += "\n" + clueText;
        }

        ManageOutline(toOutline);
    }

    //aggiunto da Pietro
    public void OnAddClueEvent(object sender, ClueEventArgs args) {
        AddClue(args.clueID, args.clueText);
    }

    //aggiunto da Pietro
    public void OnRemoveClueEvent(object sender, ClueEventArgs args) {
        RemoveClue(args.clueID);
    }

    //aggiunto da Pietro
    public void AddClue(ClueID id, string clueText) {
        if (!_addedClues.ContainsKey(id)) { 
            _addedClues.Add(id, clueText);
        }
    }

    //aggiunto da Pietro
    public void RemoveClue(ClueID id) {
        _addedClues.Remove(id);
    }

    //aggiunto da Pietro
    private void ManageOutline(Transform toOutline) {

        if (toOutline != _previousOutlined) {
            Outline outline;

            if (_previousOutlined != null) {
                //previous selected no longer selected, disable outline
                outline = _previousOutlined.GetComponent<Outline>();
                if (outline != null) {
                    outline.enabled = false;
                }
            }

            if (toOutline != null) {
                //new item selected, enable outline
                outline = toOutline.GetComponent<Outline>();
                if (outline != null) {
                    //enable outline
                    outline.enabled = true;
                } else {
                    //add outline
                    outline = toOutline.AddComponent<Outline>();
                    outline.OutlineMode = Outline.Mode.OutlineAll;
                    outline.OutlineColor = Color.white;
                    outline.OutlineWidth = 7.0f;
                    outline.enabled = true;
                }
            }

            
        }

        _previousOutlined = toOutline;
    }
}

//aggiunto da Pietro
public class ClueEventArgs : EventArgs {
    public ClueID clueID;
    public string clueText;

    public ClueEventArgs(ClueID id, string cT) {
        clueID = id;
        clueText = cT;
    }
}
