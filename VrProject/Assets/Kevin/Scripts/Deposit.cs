using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif
using Quaternion = UnityEngine.Quaternion;

public class Deposit : MonoBehaviour
{
    public PlayerPickUpDrop player;
    [SerializeField] private bool _startEmpty = false; //aggiunto da pietro, per controllare alcune cose del tutorial 
    [SerializeField] private bool _addAllItems = false; //aggiunto da pietro
    public Dictionary<Item.ItemName, GameObject> itemAssets = new Dictionary<Item.ItemName, GameObject>();
    public Dictionary<Item.ItemName, GameObject> itemCounters = new Dictionary<Item.ItemName, GameObject>();
    private Dictionary<Item.ItemName, GameObject> _spawnedItems = new Dictionary<Item.ItemName, GameObject>();

    public void Awake()
    {
        Array itemNames = Enum.GetValues(typeof(Item.ItemName));
        foreach(Item.ItemName itemName in itemNames)
        {
            if (itemName != Item.ItemName.NoItem) {
                itemAssets.Add(itemName, (GameObject)Resources.Load("Prefabs/" + itemName, typeof(GameObject)));

                if (itemName != Item.ItemName.BucketMilk && itemName != Item.ItemName.OpenPomade) {
                    itemCounters.Add(itemName, GameObject.Find("ItemDepositCounter" + itemName));
                    if (itemCounters[itemName]) {
                        itemCounters[itemName].GetComponent<ItemDepositCounter>().player = player;
                        itemCounters[itemName].GetComponent<ItemDepositCounter>().category = itemAssets[itemName].GetComponent<Item>().itemCategory;
                        //itemCounters[itemName].GetComponent<ItemDepositCounter>().transform.parent = this.transform;
                        if (_startEmpty || _addAllItems) {
                            itemCounters[itemName].GetComponent<ItemDepositCounter>().counter = 0;
                            AddItem(itemName); //add one of each object in order to precompute its outline
                                               //these object will be deleted during the next update cycle
                        } else {
                            itemCounters[itemName].GetComponent<ItemDepositCounter>().counter = 1;
                        }

                    }
                }
            }
        }

        if (_addAllItems) {
            _startEmpty = false;
        }

        if (_startEmpty) {
            //start a coroutine that will delete objects from deposit
            StartCoroutine(DelayedDestroyCoroutine());
        }
    }

    public void AddItem(Item.ItemName itemName, int amount = 1) {
        if (itemCounters.ContainsKey(itemName)) {
            if (itemCounters[itemName].GetComponent<ItemDepositCounter>().counter == 0) {
                GameObject spawnedItem = SpawnItem(itemName);
                if (spawnedItem != null) {
                    Item[] itemComponents = spawnedItem.GetComponents<Item>();
                    foreach (Item itemComponent in itemComponents) {
                        itemComponent.GetComponent<Item>().isDeposited = true;
                    }
                    
                } else {
                    Debug.LogWarning("Tried to spawn " + itemName + " in deposit but got null");
                    amount = 0;
                }
            }
            itemCounters[itemName].GetComponent<ItemDepositCounter>().counter += amount;
        } else {
            Debug.LogWarning("No key " + itemName + " in itemCounters");
        }
    }

    public void RemoveItem(Item.ItemName itemName, bool grabbedByPlayer = true, int amount = 1) {
        if (itemCounters.ContainsKey(itemName) && itemCounters[itemName].GetComponent<ItemDepositCounter>().counter > 0) {

            //Remove reference to deposited object
            if (_spawnedItems.ContainsKey(itemName)) {
                if (!grabbedByPlayer) {
                    Destroy(_spawnedItems[itemName].gameObject);
                }
                _spawnedItems.Remove(itemName);
            }

            //remove selected amount from deposit
            itemCounters[itemName].GetComponent<ItemDepositCounter>().counter -= amount;
            if (itemCounters[itemName].GetComponent<ItemDepositCounter>().counter > 0) {
                GameObject spawnedItem = SpawnItem(itemName);
                spawnedItem.GetComponent<Item>().isDeposited = true;
            }

        }
    }

    public GameObject SpawnItem(Item.ItemName itemName) {
        if (itemAssets[itemName]) {
            GameObject spawned = Instantiate(itemAssets[itemName], transform.parent);
            
            spawned.transform.localPosition = itemAssets[itemName].GetComponent<Item>().depositPosition;
            spawned.transform.localRotation = Quaternion.Euler(itemAssets[itemName].GetComponent<Item>().depositRotation);

            // GameObject spawned = Instantiate(itemAssets[itemName], itemAssets[itemName].GetComponent<Item>().depositPosition, Quaternion.Euler(itemAssets[itemName].GetComponent<Item>().depositRotation), transform.parent);

            //add reference to the instantiated object
            if (spawned) {
                foreach (Item itemComponent in spawned.GetComponents<Item>()) {
                    itemComponent.isDeposited = true;
                }
                Rigidbody rigidbody = spawned.GetComponent<Rigidbody>();
                if (rigidbody != null) {
                    rigidbody.isKinematic = true;
                }
                _spawnedItems.Add(itemName, spawned);
            }
            
            return spawned;
        } else { return null; }
    }

    IEnumerator DelayedDestroyCoroutine() {
        yield return null;
        List<Item.ItemName> names = _spawnedItems.Keys.ToList();
        foreach(Item.ItemName itemName in names) {
            RemoveItem(itemName, false);
        }
    }

}
