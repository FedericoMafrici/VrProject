using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingLand : MonoBehaviour
{
    
    
    Seed seedPlanted=null;
   public  void Interact(Seed seed)
    {
        Debug.Log(" finalmente hai puntato alla terra");
        
        if(this.seedPlanted==null)
        {
            // istanzio l'oggetto crop che avrà uno script CropbBehaviour e tengo una reference 
            this.seedPlanted=seed;
            GameObject cropObject=Instantiate(seedPlanted.SeedGameModel,gameObject.transform);
            cropObject.transform.localPosition=Vector3.zero;
            
            //TO DO 
            // ADD THE GET COMPONENT OF THE OBJ
            
        }
    }
}

 /*
        //Instantiate the crop object parented to the land
            if(cropPlanted==null){
            GameObject cropObject = Instantiate(cropPrefab, gameObject.transform);
            
            //Access the CropBehaviour of the crop we're going to plant
            // Reset local position to zero
            cropObject.transform.localPosition = Vector3.zero;

            
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            //Plant it with the seed's information
            
            cropPlanted.Plant();

            }
            else
            {
              cropPlanted.Growth();
            }
          */