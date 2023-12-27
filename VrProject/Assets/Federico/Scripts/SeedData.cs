using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Items/Seed")]
public class SeedData : ItemConsumable 
{

    //GameObject to be shown in the scene
    public GameObject SeedGameModel;
    //The crop the seed will yield
    public GameObject cropToYield; 
    
    public GameObject seedling;
   
   
    public SeedData(ItemName itemName) : base(itemName)
    {
        this.itemName=itemName;
        this.itemCategory=ItemCategory.Consumable;
        // il resto dei campi viene passato tramite inspector  
        
    }
}