using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public /*abstract*/ class ItemTool : Item
{
    public ItemTool(ItemName itemName)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Tool;
    }
}
