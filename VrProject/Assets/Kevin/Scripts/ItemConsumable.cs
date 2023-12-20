using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class ItemConsumable : Item
{
    private List<Item> productItems;
    
    public ItemConsumable(ItemName itemName)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Consumable;
    }
    
    public void Consume()
    {
        
    }
}
