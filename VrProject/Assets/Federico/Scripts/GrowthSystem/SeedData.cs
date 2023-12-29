using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Items/Seed")]
public class SeedData : ItemData
{
    //Time it takes before the seed matures into a crop
    public int daysToGrow;

    //The crop the seed will yield
    public ItemData cropToYield; 
    public GameObject seedling;
}