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
    public PlayerPickUpDrop player;

    public ItemWrapper[] itemWrappers;
    public Transform[] itemSlotArray;
    
    public int firstEmpty = 0;
    public int numSelectedButton = -1;
    public ItemWrapper activeItemWrapper;
    public Item activeItemObj;

    private void Start()
    {
        itemWrappers = new ItemWrapper[Constants.Capacity];
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
        if (item)
        {
            // Item già presente nella lista di itemWrappers: incremento il suo amount
            for (int i = 0; i < Constants.Capacity; i++)
            {
                if (itemWrappers[i] != null && itemWrappers[i].itemName == item.itemName)
                {
                    itemWrappers[i].amount++;
                    return i;
                }
            }
            
            // Item non presente nella lista di itemWrappers: lo inserisco nel primo slot libero
            if (firstEmpty < Constants.Capacity)
            {
                itemWrappers[firstEmpty] = new ItemWrapper(item.itemName, item.itemCategory);
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
                while (firstEmpty != Constants.Capacity && itemWrappers[firstEmpty] != null)
                {
                    firstEmpty++;
                }
                return lastAddedItemPos;
            }
        }
        
        return -1;
    }

    public void Remove(ItemWrapper itemWrapper)
    {
        if (itemWrapper != null)
        {
            for (int i = 0; i < Constants.Capacity; i++)
            {
                if (itemWrappers[i] != null && itemWrappers[i].itemName == itemWrapper.itemName)
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
                    
                    itemWrappers[i] = null;
                    if (i < firstEmpty)
                        firstEmpty = i;
                    break;
                }
            }
        }
    }

    public void Select(int number)
    {
        if(activeItemObj)
            Destroy(activeItemObj.gameObject);

        Item item;        
        for (int i = 0; i < Constants.Capacity; i++)
        {
            Transform buttonTransform = itemSlotArray[i].transform.Find("ItemButton");
            Transform imageTransform = buttonTransform.transform.Find("Image");
            Button button = buttonTransform.transform.GetComponent<Button>();
            if (i == number)
            {
                button.interactable = false;
                activeItemWrapper = itemWrappers[number];
                numSelectedButton = number;
            }
            else
            {
                button.interactable = true;
            }
        }
    }

    public void Deselect()
    {
        for (int i = 0; i < Constants.Capacity; i++)
        {
            Transform buttonTransform = itemSlotArray[i].transform.Find("ItemButton");
            Transform imageTransform = buttonTransform.transform.Find("Image");
            Button button = buttonTransform.transform.GetComponent<Button>();
            button.interactable = true;
        }

        numSelectedButton = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.InteractionsAreEnabled()) {
            int i = 0;
            if (Input.GetKey(KeyCode.Alpha1)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            i++;
            if (Input.GetKey(KeyCode.Alpha2)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            i++;
            if (Input.GetKey(KeyCode.Alpha3)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            i++;
            if (Input.GetKey(KeyCode.Alpha4)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            i++;
            if (Input.GetKey(KeyCode.Alpha5)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            i++;
            if (Input.GetKey(KeyCode.Alpha6)
                && numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
                InstantiateItem(i);
            }

            if (Input.GetKey(KeyCode.Alpha0)) {
                if (activeItemObj && activeItemWrapper != null) {
                    activeItemWrapper = null;
                    Item item = activeItemObj;
                    activeItemObj = null;
                    Destroy(item.gameObject);
                }
                Deselect();
            }
        }
        
    }

    public void InstantiateItem(int i)
    {
        Select(i);
        GameObject spawnedGameObject = Instantiate(player.deposit.itemAssets[itemWrappers[i].itemName], player.objectGrabPointTransform.position, Quaternion.Euler(player.deposit.itemAssets[itemWrappers[i].itemName].GetComponent<Item>().grabRotation));
        spawnedGameObject.GetComponent<Item>().Grab(player.objectGrabPointTransform, false);
        activeItemObj = spawnedGameObject.GetComponent<Item>();
        activeItemWrapper = itemWrappers[i];
    }

    public void RemoveHeldItem() {

        Item heldItem = activeItemObj;
        activeItemWrapper.amount--;
        player.ThrowDropEvent(activeItemObj);
        Deselect();
        Destroy(heldItem.gameObject);
        if (activeItemObj) {
            activeItemObj = null;
        }
        if (activeItemWrapper.amount == 0) {
            Remove(activeItemWrapper);
        }
    }

    public void MakeHoldItem(int i) {
        if (numSelectedButton != i
                && itemSlotArray[i].transform.Find("ItemButton").transform.Find("Image").transform.GetComponent<Image>().sprite) {
            InstantiateItem(i);
        }
    }
}
