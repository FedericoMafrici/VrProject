using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    private List<Item> _items = new List<Item>();

    private int _firstEmpty = 0;

    public Item active;

    public void Add(Item item)
    {
        _items[_firstEmpty] = item;
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (_items[i] == item)
            {
                _items[i] = null;
                if (i < _firstEmpty)
                    _firstEmpty = i;
            }
        }
    }

    public void Select(int number)
    {
        if (_items[number] != null)
            active = _items[number];
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
