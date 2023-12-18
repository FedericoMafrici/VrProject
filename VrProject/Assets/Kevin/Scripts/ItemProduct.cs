using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProduct : Item
{
    public ItemProduct(ItemName itemName)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Product;
    }
    
}
