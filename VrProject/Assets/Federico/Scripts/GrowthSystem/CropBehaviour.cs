using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{

    public SeedData seedToGrow;
    
    [Header("stages of Life")]
   // public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;
   
   
    public CropState cropState;
    // Start is called before the first frame update
    public int growth=0;
    public enum CropState
    {
        Seed,SeedLing,Harvestable
    }

    // PLANTING SYSTEM //it should receive the seedData from outside 
    public void Plant( )
    { 
        seedling = Instantiate(seedToGrow.seedling,transform);
        seedling.transform.localPosition=Vector3.zero;
        seedling.transform.localScale = Vector3.one;  // Add this line
        ItemData cropToYield=seedToGrow.cropToYield;
        harvestable = Instantiate(cropToYield.gameModel,transform);
        harvestable.transform.localPosition=Vector3.zero;
        harvestable.transform.localScale = Vector3.one;  // Add this lin
        switchState(CropState.Seed);
    }
    //GROWTH SYSTEM
    public void Growth()
    {
        growth++;
        switch(growth)
        {
            default: Debug.Log("problemini");
            break;
            case 1: switchState(CropState.SeedLing);
            break;
            case 2: switchState(CropState.Harvestable);
            break;
        }
    }
public void switchState(CropState stateToSwitch)
{
    //reset the visible object
    //gameObject.SetActive(false);
    seedling.SetActive(false);
    harvestable.SetActive(false);
    switch(stateToSwitch)
    {
        case CropState.Seed:
        gameObject.SetActive(true);
        break;
         case CropState.SeedLing:
         seedling.SetActive(true);
        break;
         case CropState.Harvestable:
         harvestable.SetActive(true);
         harvestable.transform.parent=  null;
         Debug.Log("harvestabl ready ");
         Destroy(gameObject);
        break;

    }
    
    cropState=stateToSwitch;
}


}



/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
   
    public int daysToGrow;
    public int cropToYield;
    // SeedData seedToGrow;
    [Header("stages of Life")]
    
    //game objects 
    
    public GameObject seedling;
    public GameObject harvestable;
   // public  Seed tmp; // temporary object used for debugging purpose 
   
     // this field will be asggined with the land where the crop is planted 
    
    public CropState cropState;
    // Start is called before the first frame update
    public int growth=0;
    public enum CropState
    {
        Seed,SeedLing,Harvestable
    }

    // PLANTING SYSTEM //it should receive the seedData from outside 
    public void Plant( Seed seed )
    { 
        
        seedling = Instantiate(seed.seedling,transform);
        seedling.transform.localPosition = Vector3.zero;
        harvestable= Instantiate(seed.cropToYield,transform);
        harvestable.transform.localPosition = Vector3.zero;
        switchState(CropState.Seed);
    }
    //GROWTH SYSTEM
    public void Growth()
    {
        growth++;
        switch(growth)
        {
            default: Debug.Log("problemini");
            break;
            case 1: switchState(CropState.SeedLing);
            break;
            case 2: switchState(CropState.Harvestable);
            break;
        }
    }
public void switchState(CropState stateToSwitch)
{
    //reset the visible object
    gameObject.SetActive(false);
    seedling.SetActive(false);
    harvestable.SetActive(false);
    switch(stateToSwitch)
    {
        case CropState.Seed:
        gameObject.SetActive(true);
        break;
         case CropState.SeedLing:
         seedling.SetActive(true);
        break;
         case CropState.Harvestable:
         harvestable.SetActive(true);
         harvestable.transform.parent=  null;
         Debug.Log("harvestabl ready ");
         Destroy(gameObject);
       break;
    }
    
    cropState=stateToSwitch;
}

    
}
*/