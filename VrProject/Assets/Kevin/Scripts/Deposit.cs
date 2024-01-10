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

    public void Start()
    {
        Vector3 itemCounterPosition;
        Array itemNames = Enum.GetValues(typeof(Item.ItemName));
        foreach(Item.ItemName itemName in itemNames)
        {
            // TODO: if da rimuovere quando gli oggetti saranno pronti
            if (itemName == Item.ItemName.Apple
                || itemName == Item.ItemName.Bucket
                || itemName == Item.ItemName.Egg
                || itemName == Item.ItemName.WheatSeed
                || itemName == Item.ItemName.WateringCan
                || itemName == Item.ItemName.EarOfWheat
                || itemName == Item.ItemName.AppleSeed
                || itemName == Item.ItemName.Shaver
                || itemName == Item.ItemName.Sponge
                || itemName == Item.ItemName.EatableApple
                || itemName == Item.ItemName.ChickenFood)
            {
                itemAssets.Add(itemName, (GameObject) Resources.Load("Prefabs/"+itemName, typeof(GameObject)));
                itemCounters.Add(itemName, (GameObject) Resources.Load("Prefabs/ItemDepositCounter", typeof(GameObject)));
                itemCounterPosition = itemAssets[itemName].GetComponent<Item>().depositPosition;
                itemCounterPosition.x += (float) 0.3;
                itemCounterPosition.y -= (float) 0.6;
                GameObject itemCounterObject = Instantiate(itemCounters[itemName], itemCounterPosition, new Quaternion(0,0,0,0));
                itemCounterObject.GetComponent<ItemDepositCounter>().player = player;
                itemCounters[itemName] = itemCounterObject;
                
            }
        }
        
        // TODO: per ogni oggetto, con degli if va settato il valore predefinito di counter nel deposito
        itemCounters[Item.ItemName.Apple].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.Bucket].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.Egg].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.Shaver].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.Sponge].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.EatableApple].GetComponent<ItemDepositCounter>().counter = 1;
        itemCounters[Item.ItemName.ChickenFood].GetComponent<ItemDepositCounter>().counter = 1;
    }

}
