using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class ItemConsumable : Item { 
    private List<Item> productItems;
    private bool consumed = false;
    
    public ItemConsumable(ItemName itemName=ItemName.WheatSeed)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Consumable;
    }
    
    public void Consume()
    {
        consumed = true;
    }

    public bool IsConsumed() {
        return consumed;
    }
}
