using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class growthInteractionManager : MonoBehaviour
{
  
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private float _interactionDistance;
    private CharacterController _character;
    private FarmingLand _pointingAtLand=null;

  //  private Interactable _pointingInteractable;
    private Vector3 _rayOrigin;
    
    public  Seed seed;
    
    public Item tool;
    public Item heldItem;
    public Hotbar _hotbar;
    public PlayerPickUpDrop _playerPickUp;
    // Start is called before the first frame update
    void Start()
    {
        _character=GetComponent<CharacterController>();
        _playerPickUp=GetComponent<PlayerPickUpDrop>();
       if(_playerPickUp!=null)
       {
        _hotbar=_playerPickUp.hotbar;
        if(_hotbar!=null)
        {
        Debug.Log(_hotbar.numSelectedButton);
        }
       }
    }

    // Update is called once per frame
    void Update()
    {
        _rayOrigin=_fpsCameraT.position+_character.radius*_fpsCameraT.forward;
        // this will be inside an if keep object
         CheckInteraction();
         Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
    }



private void CheckInteraction()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;
        heldItem=_hotbar.activeItemObj;
        if(heldItem!=null){
     //   Debug.Log(heldItem.itemName);
        }
        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            
           // Check if pointing to a land 
            _pointingAtLand = hit.transform.GetComponent<FarmingLand>();
            
            if(heldItem!=null && heldItem is Seed )
            {
                seed=(Seed) heldItem;
               // Debug.Log(seed.debug);
            }
            //uso la classe interactable perchè associo a questi oggetti un metodo interact che definisceun comportamento associato all'interazione 
            //interactable è infatti una classe astratta che associa un metodo interact che verrà implementato da tutti gli oggetti che ereditano interactable
            //
            if (_pointingAtLand && Input.GetMouseButtonDown(0) && seed!=null && _pointingAtLand.crop==null && _pointingAtLand.tree==false) // se l'oggetto interactable è diverso da null  
            { 
                
                    _pointingAtLand.Interact(seed);
                    _hotbar.Remove(_hotbar.activeItemWrapper);
                    _hotbar.Deselect();
                    Destroy(_hotbar.activeItemObj.gameObject);
            }
                //  _pointingAtLand.Interact(seed);
                if(heldItem!=null && heldItem is ItemTool)
                {        
                    tool=(Item) heldItem;
                }

             if (_pointingAtLand && Input.GetMouseButtonDown(0) ) // se l'oggetto interactable è diverso da null  
            { 
               if(tool!=null && _pointingAtLand.crop!=null)    
               {
                if(tool.itemName==Item.ItemName.WateringCan)
                        { 
                        _pointingAtLand.Interact(null);
                        }
               }
            }
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingAtLand= null;
            seed=null;
            tool=null;

        }
    }
    
}