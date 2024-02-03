using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectingPoint : MonoBehaviour
{
    public Collider itemCollectionCollider;
    public Dictionary<Item.ItemName, int> collectedItems = new Dictionary<Item.ItemName, int>();

    public event Action<Item> ItemInCollectingPoint;
    public event Action<Item> ItemOutOfCollectingPoint;
    
    void OnTriggerStay(Collider other)
    {
        // check se il collider ha reagito con il collider corretto del punto di raccolta (quello verticale)
        if (other)
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                Item[] itemComponents = item.GetComponents<Item>();
                // oggetto lasciato cadere nel punto di raccolta
                if (other.GetComponent<Rigidbody>().useGravity 
                    && item.isCollected == false 
                    && item.itemCategory != Item.ItemCategory.Tool)
                {
                    foreach (Item i in itemComponents) {
                        i.isCollected = true;
                    }
                    if (collectedItems.ContainsKey(item.itemName))
                    {
                        collectedItems[item.itemName]++;
                    }
                    else
                    {
                        collectedItems.Add(item.itemName, 1);
                        Debug.Log("Item added");
                    }

                    //throw event, needed for collect quest

                    if (ItemInCollectingPoint != null) {
                        foreach (Item i in itemComponents) {
                            ItemInCollectingPoint(i);
                        }
                    }

                    //hide marker in case it was shown
                    QuestMarkerManager questMarkerManager = item.GetComponent<QuestMarkerManager>();
                    if (questMarkerManager != null) {
                        questMarkerManager.SetIsCollected(true);
                    }
                }
                // oggetto nell'area del punto di raccolta ma tenuto in mano
                else if(other.GetComponent<Rigidbody>().useGravity == false 
                        && collectedItems.ContainsKey(item.itemName) 
                        && item.isCollected 
                        && item.itemCategory != Item.ItemCategory.Tool)
                {
                    foreach (Item i in itemComponents) {
                        i.isCollected = false;
                    }
                    collectedItems[item.itemName]--;
                    if (collectedItems[item.itemName] == 0)
                    {
                        collectedItems.Remove(item.itemName);
                        Debug.Log("Item removed");
                    }

                    //throw event, needed for collect quest
                    
                    if (ItemOutOfCollectingPoint != null) {
                        foreach (Item i in itemComponents) {
                            ItemOutOfCollectingPoint(i);
                        }
                    }

                    //tell marker that it can be shown
                    QuestMarkerManager questMarkerManager = item.GetComponent<QuestMarkerManager>();
                    if (questMarkerManager != null) {
                        questMarkerManager.SetIsCollected(false);
                    }
                }
            }
        }
    }

    void Update()
    {
    }
    
}
