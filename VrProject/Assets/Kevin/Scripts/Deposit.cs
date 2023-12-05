using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;

public class Deposit : MonoBehaviour
{
    private Dictionary<Item, bool> _items = new Dictionary<Item, bool>();

    public void Drop(Item item)
    {
        if (_items.ContainsKey(item))
        {
            _items[item] = false;
            item.Show();
        }
    }

    public void Pick(Item item)
    {
        if (_items.ContainsKey(item))
        {
            _items[item] = true;
            item.Hide();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
