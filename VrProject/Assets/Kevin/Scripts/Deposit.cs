using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

public class Deposit : MonoBehaviour
{
    public PlayerPickUpDrop player;
    public Dictionary<Item.ItemName, GameObject> itemAssets = new Dictionary<Item.ItemName, GameObject>();
    public Dictionary<Item.ItemName, GameObject> itemCounters = new Dictionary<Item.ItemName, GameObject>();

    public void Awake()
    {
        Array itemNames = Enum.GetValues(typeof(Item.ItemName));
        foreach(Item.ItemName itemName in itemNames)
        {
            itemAssets.Add(itemName, (GameObject) Resources.Load("Prefabs/"+itemName, typeof(GameObject)));
            
            if (itemName != Item.ItemName.BucketMilk && itemName != Item.ItemName.OpenPomade){
                itemCounters.Add(itemName, GameObject.Find("ItemDepositCounter"+itemName));
                if (itemCounters[itemName])
                {
                    itemCounters[itemName].GetComponent<ItemDepositCounter>().counter = 1;
                    
                    itemCounters[itemName].GetComponent<ItemDepositCounter>().player = player;
                }
            }
        }
    }

    public void AddItem(Item.ItemName itemName, int amount = 1) {
        if (itemCounters[itemName].GetComponent<ItemDepositCounter>().counter == 0) {
            GameObject spawnedItem = SpawnItem(itemName);
            Item[] itemComponents = spawnedItem.GetComponents<Item>();
            foreach (Item itemComponent in itemComponents) {
                itemComponent.GetComponent<Item>().isDeposited = true;
            }
        }
        itemCounters[itemName].GetComponent<ItemDepositCounter>().counter += amount;
    }

    private GameObject SpawnItem(Item.ItemName itemName) {
        return Instantiate(itemAssets[itemName],
            itemAssets[itemName].GetComponent<Item>().depositPosition,
            Quaternion.Euler(itemAssets[itemName].GetComponent<Item>().depositRotation));
    }

}
