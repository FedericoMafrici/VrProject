using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class ItemProduct : Item
{
    public ItemProduct(ItemName itemName=ItemName.Carrot)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Product;
    }

}
