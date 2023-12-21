using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    public int daysToGrow;
    public int cropToYield;
    SeedData seedToGrow;
    [Header("stages of Life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;
    public  SeedData tmp; // oggetto tmp usato perchè non ho la hotbar al momento  
   
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
        SeedData seedToGrow=tmp; //usiamo il tmp come placeholder per inserire i cambi seedling e harvestable
        this.seedToGrow=seedToGrow;
        seedling = Instantiate(seedToGrow.seedling,transform);
        ItemData cropToYield=seedToGrow.cropToYield;
        harvestable = Instantiate(cropToYield.gameModel,transform);
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
    seed.SetActive(false);
    seedling.SetActive(false);
    harvestable.SetActive(false);
    switch(stateToSwitch)
    {
        case CropState.Seed:
        seed.SetActive(true);
        break;
         case CropState.SeedLing:
         seedling.SetActive(true);
        break;
         case CropState.Harvestable:
         harvestable.SetActive(true);
        break;

    }
    
    cropState=stateToSwitch;
}


}
