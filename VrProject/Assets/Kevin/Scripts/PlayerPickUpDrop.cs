using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private TMP_Text clue;
    [SerializeField] private Hotbar hotbar;

    private float pickupDistance = 5f;
    private Item item;

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit,
                pickupDistance, pickupLayerMask)
            && raycastHit.transform.TryGetComponent(out Item obj)
            && hotbar.activeItem == null)
        {
            clue.text = "Press E to grab\nPress Q to collect";
            if (hotbar.firstEmpty == 6)
            {
                clue.text+="\n\n Release an item to grab or collect another object!";
            }
        }
        else
            clue.text = "";

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hotbar.activeItem == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                        out RaycastHit raycastHit2,
                        pickupDistance, pickupLayerMask)  && hotbar.firstEmpty <= Constants.Capacity)
                {
                    if (raycastHit2.transform.TryGetComponent(out item))
                    {
                        int lastItemIndex = hotbar.Add(item, true);
                        hotbar.activeItem = hotbar.items[lastItemIndex];
                        
                        item.Grab(objectGrabPointTransform);
                        Debug.Log(item+" grabbed");
                    }
                }
            }
            else
            {
                Drop();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                    out RaycastHit raycastHit2,
                    pickupDistance, pickupLayerMask)  && hotbar.firstEmpty <= Constants.Capacity)
            {
                if (raycastHit2.transform.TryGetComponent(out item))
                {
                    hotbar.Add(item, false);
                    Debug.Log(item+" added to the hotbar");
                    
                    item.StartFading();
                }
            }
            else if (hotbar.activeItem)
            {
                Drop();
            }
        }
    }

    public void Drop()
    {
        hotbar.Remove(hotbar.activeItem);
        Debug.Log(item+" removed from the hotbar");
        
        hotbar.activeItem.Drop();
        Debug.Log(item+" dropped");
        
        hotbar.Deselect();
    }
}
