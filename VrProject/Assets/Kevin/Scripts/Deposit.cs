using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

public class Deposit : MonoBehaviour
{

    private Dictionary<Item, bool> itemWrappers = new Dictionary<Item, bool>();
    
    public Dictionary<Item.ItemName, GameObject> itemAssets = new Dictionary<Item.ItemName, GameObject>();

    public void Start()
    {
        itemAssets.Add(Item.ItemName.Apple, (GameObject) Resources.Load("Prefabs/Apple", typeof(GameObject)));
        itemAssets.Add(Item.ItemName.Bucket, (GameObject) Resources.Load("Prefabs/Bucket", typeof(GameObject)));
        itemAssets.Add(Item.ItemName.Egg, (GameObject) Resources.Load("Prefabs/Egg", typeof(GameObject)));
    }
    
    public void Drop(Item item)
    {
        if (itemWrappers.ContainsKey(item))
        {
            itemWrappers[item] = false;
            // SHOW OBJ
        }
    }

    public void Pick(Item item)
    {
        if (itemWrappers.ContainsKey(item))
        {
            itemWrappers[item] = true;
            // HIDE OBJ
        }
    }
    
}
