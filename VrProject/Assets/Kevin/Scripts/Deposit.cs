using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    private Dictionary<Item, bool> items = new Dictionary<Item, bool>();

    public void Drop(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item] = false;
            // SHOW OBJ
        }
    }

    public void Pick(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item] = true;
            // HIDE OBJ
        }
    }
}
