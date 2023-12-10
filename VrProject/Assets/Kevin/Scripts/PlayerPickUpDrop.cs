using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private TMP_Text clue;

    private float pickupDistance = 5f;
    private Grabbable objGrabbable;
    
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit,
                pickupDistance, pickupLayerMask)
            && raycastHit.transform.TryGetComponent(out Grabbable obj)
            && objGrabbable == null)
        {
            clue.text = "Press E to grab";
        }
        else
            clue.text = "";

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objGrabbable == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                        out RaycastHit raycastHit2,
                        pickupDistance, pickupLayerMask))
                {
                    Debug.Log(raycastHit2.transform);
                    if (raycastHit2.transform.TryGetComponent(out objGrabbable))
                    {
                        objGrabbable.Grab(objectGrabPointTransform);
                        Debug.Log(objGrabbable);
                    }
                }
            }
            else
            {
                objGrabbable.Drop();
                objGrabbable = null;
            }
        }
    }
}
