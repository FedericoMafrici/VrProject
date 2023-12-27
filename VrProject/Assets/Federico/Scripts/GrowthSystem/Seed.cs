using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : ItemConsumable
{
   //GameObject to be shown in the scene
    public GameObject SeedGameModel;
    //The crop the seed will yield
    public GameObject cropToYield; 
    
    public GameObject seedling;
   
    public int debug=1;
    


    void start()
    {
        
    }
}
