using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UI.Image;

public class Hotbar : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<Image> images;
    public int firstEmpty = 0;
    public int selectedItem;
    public Item activeItem;

    public void Add(Item item)
    {
        if (items.Count < 6)
        {
            Image border = images[firstEmpty].GetComponent<Image>();
            Button borderColor = images[firstEmpty].GetComponent<Button>();
            Image itemSprite = border.GetComponent<Image>();
            items[firstEmpty] = item;
            itemSprite = item.icon;
        }
    }

    public void Remove(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                if (i < firstEmpty)
                    firstEmpty = i;
            }
        }
    }

    public void Select(int number)
    {
        if (items[number-1] != null)
        {
            activeItem = items[number-1];
            selectedItem = number-1;
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            Select(1);
        if (Input.GetKey(KeyCode.Alpha2))
            Select(2);
        if (Input.GetKey(KeyCode.Alpha3))
            Select(3);
        if (Input.GetKey(KeyCode.Alpha4))
            Select(4);
        if (Input.GetKey(KeyCode.Alpha5))
            Select(5);
        if (Input.GetKey(KeyCode.Alpha6))
            Select(6);
    }
}
