using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Item;

public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private LayerMask pickupLayerMask;
    [SerializeField] private TMP_Text clue;
    [SerializeField] public Hotbar hotbar;
    
    private const float pickupDistance = 5f;
    private Item item;
    public Deposit deposit;
    public ChangeScenario changeScenario;
    
    public Transform objectGrabPointTransform;

    public event Action<Item.ItemName> PickUpEvent;
    public event Action<Item.ItemName> DropEvent;

    // Update is called once per frame
    void Update() {

        if (InputManager.InteractionsAreEnabled()) {

            // ------------- PRESSIONE TASTO E -------------

            if (Input.GetKeyDown(KeyCode.E)) {
                if (hotbar.activeItemObj == null) {
                    if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                            out RaycastHit raycastHit3,
                            pickupDistance, pickupLayerMask)
                        && raycastHit3.transform.TryGetComponent(out item)
                        && item.enabled
                        && !item.isInPlayerHand
                        && (hotbar.firstEmpty < Constants.Capacity || hotbar.Contains(item.itemName))) {

                        if (item.itemName == Item.ItemName.Leaf && (item as AnimalFood).IsPlanted()) {
                            item.GetComponent<BoxCollider>().enabled = false;
                            item.GetComponent<MeshCollider>().enabled = true;
                            StartCoroutine(RegenerateItem(ItemName.Leaf, item.transform, item.transform.parent));
                        }

                        int lastItemIndex = hotbar.Add(item, true);
                        hotbar.Select(lastItemIndex);
                        hotbar.activeItemObj = item;
                        item.Grab(objectGrabPointTransform, true);

                        item.GetComponent<AudioSource>().clip = item.grabSound;
                        item.GetComponent<AudioSource>().Play();

                        Debug.Log(item + " grabbed");

                        if (item.isDeposited) {

                            Item[] itemComponents = item.GetComponents<Item>();
                            foreach (Item itemComponent in itemComponents) {
                                itemComponent.isDeposited = false;
                            }
                            deposit.RemoveItem(item.itemName, true);
                            /*
                            if (--deposit.itemCounters[item.itemName].GetComponent<ItemDepositCounter>().counter != 0) {
                                StartSpawning();
                            }
                            */
                        }
                    }

                    ThrowPickUpEvent(item);
                } else if (hotbar.activeItemObj.itemCategory != Item.ItemCategory.Tool)
                {
                    int slot = hotbar.numSelectedButton;
                    Drop();
                    item = null;
                    if (hotbar.itemWrappers[slot] != null && hotbar.itemWrappers[slot].amount != 0)
                        StartCoroutine(GrabAnother(slot));
                }
            }

            // ------------- PRESSIONE TASTO Q -------------

            else if (Input.GetKeyDown(KeyCode.Q)) {
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward,
                        out RaycastHit raycastHit4, pickupDistance, pickupLayerMask)
                    && raycastHit4.transform.TryGetComponent(out item)
                    && item.enabled
                    & !item.isInPlayerHand
                    && (hotbar.firstEmpty < Constants.Capacity || hotbar.Contains(item.itemName))) {
                    // item.Grab(objectGrabPointTransform, true); // TODO: lascio? Serve a vedere l'effetto di spostamento dell'oggetto mentre lo raccolgo

                    if (item.itemName == Item.ItemName.Leaf && (item as AnimalFood).IsPlanted()) {
                        item.GetComponent<BoxCollider>().enabled = false;
                        item.GetComponent<MeshCollider>().enabled = true;
                        StartCoroutine(RegenerateItem(ItemName.Leaf, item.transform, item.transform.parent));
                    }

                    hotbar.Add(item, false);

                    item.GetComponent<AudioSource>().clip = item.grabSound;
                    item.GetComponent<AudioSource>().Play();
                    item.ThrowGrabEvent(); //aggiunto da Pietro

                    Debug.Log(item + " added to the hotbar");
                    

                    if (item.isDeposited) {

                        Item[] itemComponents = item.GetComponents<Item>();
                        foreach (Item itemComponent in itemComponents) {
                            itemComponent.isDeposited = false;
                        }
                        deposit.RemoveItem(item.itemName, true);
                        /*
                        if (--deposit.itemCounters[item.itemName].GetComponent<ItemDepositCounter>().counter != 0) {
                            GameObject spawnedItem = SpawnItem(item.itemName);
                            foreach(Item itemComponent in spawnedItem.GetComponents<Item>())
                                itemComponent.GetComponent<Item>().isDeposited = true;
                        }
                        */
                    }

                    ThrowPickUpEvent(item);
                    item.StartFading();
                } else if (Vector3.Distance(deposit.transform.position, playerCameraTransform.position) < pickupDistance
                           && hotbar.activeItemObj != null
                           && hotbar.activeItemObj.itemName != Item.ItemName.BucketMilk
                           && hotbar.activeItemObj.itemName != Item.ItemName.OpenPomade) {
                    
                    hotbar.activeItemObj.StartFading();
                    deposit.AddItem(hotbar.activeItemObj.itemName, hotbar.activeItemWrapper.amount);
                    DepositAll();
                    
                    Debug.Log(hotbar.activeItemObj + " released into the deposit");
                }

            }
            
            // ------------- PRESSIONE TASTO H: nascondi o espandi quests list -------------
            
            else if (Input.GetKeyDown(KeyCode.H) && !GameObject.Find("QuestsList").GetComponent<UI_QuestsList>().commandLocked)
            {
                UI_QuestsList questsList = GameObject.Find("QuestsList").GetComponent<UI_QuestsList>();
                if (questsList.isOpen)
                {
                    questsList.commandLocked = true;
                    questsList.Close();
                }
                else
                {
                    questsList.commandLocked = true;
                    questsList.Open();
                }
            }
            
            // ------------- PRESSIONE L: cambio scenario -------------
            
            else if (Input.GetKeyDown(KeyCode.L) 
                && Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit5, pickupDistance, pickupLayerMask)
                && raycastHit5.transform.gameObject.layer == LayerMask.NameToLayer("Pipe") /*&& pickupLayerMask == LayerMask.GetMask("Pipe")*/) {
                if (changeScenario.currentScenario + 1 < changeScenario.scenarios.Count 
                    && changeScenario.scenarios[changeScenario.currentScenario + 1].unlocked)
                {
                    changeScenario.SelectScenario(changeScenario.currentScenario + 1);
                }
                else
                {
                    changeScenario.SelectScenario(0);
                }
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

        ThrowDropEvent(hotbar.activeItemObj);

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
    
    public void DepositAll()
    {
        hotbar.activeItemWrapper.amount = 0;
        hotbar.Remove(hotbar.activeItemWrapper);

        if (hotbar.activeItemObj)
            hotbar.activeItemObj = null;

        hotbar.Deselect();
    }

    public GameObject SpawnItem(Item.ItemName itemName)
    {
        return deposit.SpawnItem(itemName);
        /*
        return Instantiate(deposit.itemAssets[itemName], 
            deposit.itemAssets[itemName].GetComponent<Item>().depositPosition, 
            Quaternion.Euler(deposit.itemAssets[itemName].GetComponent<Item>().depositRotation));
        */
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
        Item[] itemComponents = spawnedItem.GetComponents<Item>();
        foreach (Item itemComponent in itemComponents) {
            itemComponent.GetComponent<Item>().isDeposited = true;
        }
    }
    
    public void ThrowPickUpEvent(Item item) {
        if (item != null) {
            Item[] itemComponents = item.GetComponents<Item>();
            if (item != null && PickUpEvent != null) {
                foreach (Item i in itemComponents) {
                    PickUpEvent(item.itemName);
                }
            }
        }
    }

    public void ThrowDropEvent(Item item) {
        Item[] itemComponents = item.GetComponents<Item>();
        if (item != null && DropEvent != null) {
            foreach (Item i in itemComponents) {
                DropEvent(item.itemName);
            }
        }
    }

    public bool PickUpAndSelect(Item newItem) {
        if (hotbar.firstEmpty < Constants.Capacity) {
            // item.Grab(objectGrabPointTransform, true); // TODO: lascio? Serve a vedere l'effetto di spostamento dell'oggetto mentre lo raccolgo

            int i = hotbar.Add(newItem, false);

            newItem.GetComponent<AudioSource>().clip = newItem.grabSound;
            newItem.GetComponent<AudioSource>().Play();

            Debug.Log(newItem + " added to the hotbar");

            if (newItem.isDeposited) {
                Item[] itemComponents = newItem.GetComponents<Item>();
                foreach (Item itemComponent in itemComponents) {
                    itemComponent.isDeposited = false;
                }

                if (--deposit.itemCounters[newItem.itemName].GetComponent<ItemDepositCounter>().counter != 0) {
                    GameObject spawnedItem = SpawnItem(newItem.itemName);
                    foreach (Item itemComponent in itemComponents) {
                        itemComponent.GetComponent<Item>().isDeposited = true;
                    }
                }
            }

            Destroy(newItem.gameObject);
            if (hotbar.itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<UnityEngine.UI.Image>().sprite) {
                hotbar.InstantiateItem(i);
            }
            ThrowPickUpEvent(newItem);

            return true;
        } else {
            return false;
        }
    }

    IEnumerator RegenerateItem(Item.ItemName name, Transform itemTransform, Transform parent) {
        Vector3 position = itemTransform.position;
        Quaternion rotation = itemTransform.rotation;
        yield return new WaitForSeconds(30);
        Debug.Log("Respawining item");
        GameObject spawned = Instantiate((GameObject)Resources.Load("Prefabs/" + name, typeof(GameObject)));
        spawned.transform.position = position;
        spawned.transform.rotation = rotation;
        spawned.transform.parent = parent;
        if (name == ItemName.Leaf) {
            AnimalFood afComponent = spawned.GetComponent<AnimalFood>();
            if (afComponent != null) {
                afComponent.SetIsPlanted(true);
            }
            spawned.GetComponent<MeshCollider>().enabled = false;
            spawned.GetComponent<BoxCollider>().enabled = true;
        }

    }

    public IEnumerator GrabAnother(int slot)
    {
        yield return new WaitForSeconds(0.5f);
        hotbar.InstantiateItem(slot);
    }

}

public class ItemEventArgs : EventArgs {
    public Item item;
    public ItemEventArgs(Item i) {
        item = i;
    }
}
