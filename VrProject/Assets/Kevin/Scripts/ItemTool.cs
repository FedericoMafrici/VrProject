using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTool : Item
{
    public Vector3 depositPosition;
    
    public ItemTool(ItemName itemName)
    {
        this.itemName = itemName;
        this.itemCategory = ItemCategory.Tool;
    }
}
