using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

static class Constants
{
    public const int Capacity = 6;
}

public class Hotbar : MonoBehaviour
{
    
    public Item[] items;
    public Transform[] itemSlotArray;
    
    public int firstEmpty = 0;
    public int numActiveItem;
    public Item activeItem;

    private void Start()
    {
        items = new Item[Constants.Capacity];
        itemSlotArray = new Transform[Constants.Capacity];
        
        Transform itemParentTransform = gameObject.transform.Find("ItemParent");

        if (itemParentTransform)
        {
            string itemSlot = "ItemSlot";
            for (int i = 1; i <= Constants.Capacity; i++)
            {
                string itemSlotTemp = itemSlot + i;
                Transform itemSlotTransform = itemParentTransform.transform.Find(itemSlotTemp);
                if(itemSlotTransform)
                    itemSlotArray[i-1] = itemSlotTransform;
            }
        }
        
    }
    
    public int Add(Item item, bool isGrabbed)
    {
        if (item && firstEmpty < Constants.Capacity)
        {
            items[firstEmpty] = item;
            Transform buttonTransform = itemSlotArray[firstEmpty].transform.Find("ItemButton");
            Transform imageTransform = buttonTransform.transform.Find("Image");
            if (buttonTransform && imageTransform)
            {
                if (isGrabbed)
                {
                    Button button = buttonTransform.transform.GetComponent<Button>();
                    button.interactable = false;
                }
                
                Image image = imageTransform.transform.GetComponent<Image>();
                image.sprite = item.icon;
                Color color = image.color;
                color.a = 1;
                image.color = color;
            }

            int lastAddedItemPos = firstEmpty;
            firstEmpty++;
            while (items[firstEmpty] != null && firstEmpty != Constants.Capacity)
            {
                firstEmpty++;
            }
            return lastAddedItemPos;
        }

        return -1;
    }

    public void Remove(Item item)
    {
        if (item)
        {
            for (int i = 0; i < Constants.Capacity; i++)
            {
                if (items[i] == item)
                {
                    Transform buttonTransform = itemSlotArray[i].transform.Find("ItemButton");
                    Transform imageTransform = buttonTransform.transform.Find("Image");
                    if (buttonTransform && imageTransform)
                    {
                        Button button = buttonTransform.transform.GetComponent<Button>();
                        button.interactable = true;
                        
                        Image image = imageTransform.transform.GetComponent<Image>();
                        image.sprite = null;
                        Color color = image.color;
                        color.a = 0;
                        image.color = color;
                    }
                    
                    items[i] = null;
                    if (i < firstEmpty)
                        firstEmpty = i;
                    break;
                }
            }
        }
    }

    public void Select(int number)
    {
        Item item;
        for (int i = 0; i <= Constants.Capacity; i++)
        {
            item = items[i];
            Transform buttonTransform = itemSlotArray[i].transform.Find("ItemButton");
            Transform imageTransform = buttonTransform.transform.Find("Image");
            Button button = buttonTransform.transform.GetComponent<Button>();
            if (i == number)
            {
                button.interactable = false;
                activeItem = items[number];
            }
            else
            {
                button.interactable = true;
            }
        }
        // TO DO: sparisce item dalla mano (se presente) e spawna il nuovo
    }

    public void Deselect()
    {
        if (activeItem)
        {
            for (int i = 0; i < Constants.Capacity; i++)
            {
                if (items[i])
                {
                    Transform buttonTransform = itemSlotArray[i].transform.Find("ItemButton");
                    Transform imageTransform = buttonTransform.transform.Find("Image");
                    Button button = buttonTransform.transform.GetComponent<Button>();
                    button.interactable = true;
                }
            }
            
            activeItem = null;
            // TO DO: SPARISCE ITEM DALLA MANO
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
            Select(0);
        if (Input.GetKey(KeyCode.Alpha2))
            Select(1);
        if (Input.GetKey(KeyCode.Alpha3))
            Select(2);
        if (Input.GetKey(KeyCode.Alpha4))
            Select(3);
        if (Input.GetKey(KeyCode.Alpha5))
            Select(4);
        if (Input.GetKey(KeyCode.Alpha6))
            Select(5);
        if (Input.GetKey(KeyCode.Alpha0))
            Deselect();
    }
}
