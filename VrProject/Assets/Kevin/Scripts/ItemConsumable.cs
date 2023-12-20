using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConsumable : Item
{
    private List<Item> productItems;
    
    public ItemConsumable(ItemName itemName)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Consumable;
    }
    
    // TO DO: cambia in metood abstract da implementare in tutti gli script degli oggetti che ne derivano
    public void Consume()
    {
        
    }
}
