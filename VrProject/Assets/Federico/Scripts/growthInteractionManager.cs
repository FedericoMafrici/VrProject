using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class growthInteractionManager : MonoBehaviour
{
    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private float _interactionDistance;
    private CharacterController _character;
    private Land _pointingAtLand=null;
  //  private Interactable _pointingInteractable;
    private Vector3 _rayOrigin;

    // Start is called before the first frame update
    void Start()
    {
        _character=GetComponent<CharacterController>();
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

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
         
           // Check if pointing to a land 
            _pointingAtLand = hit.transform.GetComponent<Land>();
           
            //uso la classe interactable perchè associo a questi oggetti un metodo interact che definisceun comportamento associato all'interazione 
            //interactable è infatti una classe astratta che associa un metodo interact che verrà implementato da tutti gli oggetti che ereditano interactable
            //
            if (_pointingAtLand && Input.GetMouseButtonDown(0)) // se l'oggetto interactable è diverso da null  
            { 
                   
                    _pointingAtLand.Interact();
            }

         
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingAtLand= null;
        
        }
    }
}