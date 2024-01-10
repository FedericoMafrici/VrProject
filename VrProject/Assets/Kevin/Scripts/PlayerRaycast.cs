using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
            
    [SerializeField] private TMP_Text clue;
    [SerializeField] public Hotbar hotbar;
    
    private const float pickupDistance = 5f;
    private Item item;
    public Deposit deposit;

    void Update()
    {
        
        // ------------- AGGIORNAMENTO INFORMAZIONI TESTUALI SOTTO IL CURSORE -------------
        
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
        
        else if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit2,
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
        }
        
        else if (Vector3.Distance(deposit.transform.position, playerCameraTransform.position) < 10
                 && hotbar.activeItemObj != null)
        {
            clue.text = "Press Q to release the item";
        }
        
        else
            clue.text = "";
    }
}
