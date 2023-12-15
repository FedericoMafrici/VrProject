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
    [SerializeField] public Transform objectGrabPointTransform;
    [SerializeField] private TMP_Text clue;
    [SerializeField] private Deposit deposit;
    [SerializeField] private Hotbar hotbar;

    private float pickupDistance = 5f;
    private Item item;

    public void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit,
                pickupDistance, pickupLayerMask)
            && raycastHit.transform.TryGetComponent(out Item item)
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
                        pickupDistance, pickupLayerMask)  && hotbar.firstEmpty < Constants.Capacity)
                {
                    if (raycastHit2.transform.TryGetComponent(out item))
                    {
                        int lastItemIndex = hotbar.Add(item, true);
                        hotbar.Select(lastItemIndex);
                        
                        item.Grab(objectGrabPointTransform, true);
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
                    pickupDistance, pickupLayerMask)  && hotbar.firstEmpty < Constants.Capacity)
            {
                if (raycastHit2.transform.TryGetComponent(out item))
                {
                    // item.Grab(objectGrabPointTransform, true); // serve a vedere l'effetto di spostamento dell'oggetto emntre lo raccolgo
                    
                    hotbar.Add(item, false);
                    Debug.Log(item+" added to the hotbar");
                    
                    item.StartFading();
                }
            }
        }
    }

    public void Drop()
    {
        if (hotbar.activeItem.isTool)
        {
            hotbar.activeItem.StartFading();
        }
        
        hotbar.activeItem.Drop();
        Debug.Log(item+" dropped");

        hotbar.activeItem.amount--;
        if (hotbar.activeItem.amount == 0)
        {
            hotbar.Remove(hotbar.activeItem);
            Debug.Log(item+" removed from the hotbar");
        }
        
        if (hotbar.activeItem)
            hotbar.activeItem = null;

        hotbar.Deselect();
    }
}
