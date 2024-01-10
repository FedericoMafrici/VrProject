using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    [SerializeField] public Hotbar hotbar;
    
    private const float pickupDistance = 5f;
    private Item item;
    public Deposit deposit;
    
    public Transform objectGrabPointTransform;

    // Update is called once per frame
    void Update()
    {

        // ------------- PRESSIONE TASTO E -------------
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hotbar.activeItemObj == null)
            {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                        out RaycastHit raycastHit3,
                        pickupDistance, pickupLayerMask) 
                    && raycastHit3.transform.TryGetComponent(out item)
                    && hotbar.firstEmpty < Constants.Capacity)
                {
                    int lastItemIndex = hotbar.Add(item, true);
                    hotbar.Select(lastItemIndex);
                    hotbar.activeItemObj = item;
                    item.Grab(objectGrabPointTransform, true);
                    Debug.Log(item + " grabbed");
                    
                    if (item.isDeposited)
                    {
                        item.isDeposited = false;
                        if (--deposit.itemCounters[item.itemName].GetComponent<ItemDepositCounter>().counter != 0)
                        {
                            StartSpawning();
                        }
                    }
                }
            }
            
            else if(hotbar.activeItemObj.itemCategory != Item.ItemCategory.Tool)
            {
                Drop();
            }
        }
        
        // ------------- PRESSIONE TASTO Q -------------
        
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                    out RaycastHit raycastHit4, pickupDistance, pickupLayerMask) 
                && raycastHit4.transform.TryGetComponent(out item)
                && hotbar.firstEmpty < Constants.Capacity)
            {
                // item.Grab(objectGrabPointTransform, true); // TODO: lascio? Serve a vedere l'effetto di spostamento dell'oggetto mentre lo raccolgo

                hotbar.Add(item, false);
                Debug.Log(item + " added to the hotbar");

                if (item.isDeposited)
                {
                    item.isDeposited = false;
                    if (--deposit.itemCounters[item.itemName].GetComponent<ItemDepositCounter>().counter != 0)
                    {
                        GameObject spawnedItem = SpawnItem(item.itemName);
                        spawnedItem.GetComponent<Item>().isDeposited = true;
                    }
                }
                
                item.StartFading();
            }
            
            else if (Vector3.Distance(deposit.transform.position, playerCameraTransform.position) < 10
                     && hotbar.activeItemObj != null)
            {
                hotbar.activeItemObj.StartFading();
                if (deposit.itemCounters[hotbar.activeItemObj.itemName].GetComponent<ItemDepositCounter>().counter == 0)
                {
                    GameObject spawnedItem = SpawnItem(hotbar.activeItemObj.itemName);
                    spawnedItem.GetComponent<Item>().isDeposited = true;
                }
                deposit.itemCounters[hotbar.activeItemObj.itemName].GetComponent<ItemDepositCounter>().counter++;
                Debug.Log(hotbar.activeItemObj + " released into the deposit");
                Drop(false);
            }
            
        }
    }

    /// <summary>
    /// Rimuove l'oggetto attivo dalla mano, lo rimuove dalla hotbar se necessario e ne deseleziona il bottone corrispondente.
    /// </summary>
    /// <param name="isFalling">
    /// Serve a distinguere se sto droppando l'oggetto dalla mano oppure se lo sto mettendo solo nel deposito.
    /// Se true (default), invoco Drop() di Grabbable.cs e vedo l'oggetto cadere; se false, lo vedo solo sparire.
    /// </param>
    public void Drop(bool isFalling = true)
    {
        if (isFalling)
        {
            hotbar.activeItemObj.Drop();
            Debug.Log(item+" dropped");
        }

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

    public GameObject SpawnItem(Item.ItemName itemName)
    {
        return Instantiate(deposit.itemAssets[itemName], 
            deposit.itemAssets[itemName].GetComponent<Item>().depositPosition, 
            Quaternion.Euler(0,0,0));
    }

    /// <summary>
    /// Invoca una Coroutine che attende 0.3 secondi prima di istanziare un nuovo Item nel deposito.
    /// </summary>
    private void StartSpawning()
    {
        StartCoroutine(Spawn());
    }
    
    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(0.1F);
        GameObject spawnedItem = SpawnItem(hotbar.activeItemObj.itemName);
        spawnedItem.GetComponent<Item>().isDeposited = true;
    }
    

}
