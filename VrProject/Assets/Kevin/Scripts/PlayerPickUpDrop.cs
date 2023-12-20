using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private TMP_Text clue;
    [SerializeField] private Deposit deposit;
    [SerializeField] private Hotbar hotbar;
    
    private float pickupDistance = 5f;
    private Item item;
    
    public Transform objectGrabPointTransform;

    public void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit,
                pickupDistance, pickupLayerMask)
            && raycastHit.transform.TryGetComponent(out Item item)
            && hotbar.activeItemObj == null)
        {
            clue.text = "Press E to grab\nPress Q to collect";
            if (hotbar.firstEmpty == 6)
            {
                clue.text += "\n\n Release an item to grab or collect another object!";
            }
        }
        else
            clue.text = "";

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hotbar.activeItemObj == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                        out RaycastHit raycastHit2,
                        pickupDistance, pickupLayerMask) && hotbar.firstEmpty < Constants.Capacity)
                {
                    if (raycastHit2.transform.TryGetComponent(out item))
                    {
                        int lastItemIndex = hotbar.Add(item, true);
                        hotbar.Select(lastItemIndex);
                        hotbar.activeItemObj = item;
                        item.Grab(objectGrabPointTransform, true);
                        Debug.Log(item + " grabbed");
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
                    pickupDistance, pickupLayerMask) && hotbar.firstEmpty < Constants.Capacity)
            {
                if (raycastHit2.transform.TryGetComponent(out item))
                {
                    // item.Grab(objectGrabPointTransform, true); // TO DO: lasscio? Serve a vedere l'effetto di spostamento dell'oggetto mentre lo raccolgo

                    hotbar.Add(item, false);
                    Debug.Log(item + " added to the hotbar");

                    item.StartFading();
                }
            }
        }
    }

    public void Drop()
    {
        if (hotbar.activeItemObj.itemCategory == Item.ItemCategory.Tool)
        {
            hotbar.activeItemObj.StartFading();
            Instantiate(deposit.itemAssets[hotbar.activeItemObj.itemName], 
                deposit.itemAssets[hotbar.activeItemObj.itemName].GetComponent<ItemTool>().depositPosition, 
                Quaternion.Euler(0,0,0));
        }
        
        hotbar.activeItemObj.Drop();
        Debug.Log(item+" dropped");

        hotbar.activeItemWrapper.amount--;
        if (hotbar.activeItemWrapper.amount == 0)
        {
            hotbar.Remove(hotbar.activeItemWrapper);
            Debug.Log(item+" removed from the hotbar");
        }
        
        if (hotbar.activeItemObj)
            hotbar.activeItemObj = null;

        hotbar.Deselect();
    }
}
