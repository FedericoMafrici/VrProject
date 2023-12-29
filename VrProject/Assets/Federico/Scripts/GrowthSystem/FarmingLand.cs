using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingLand : MonoBehaviour
{
    float yOffset=1.03f;
    
   
    GameObject tmp;
    public CropBehaviour  crop=null;
   public  void Interact(Seed seed)
    {
        Debug.Log(" finalmente hai puntato alla terra");
        
        if(crop==null)
        {
            // istanzio l'oggetto crop che avrà uno script CropbBehaviour e tengo una reference 
            
            tmp=Instantiate(seed.seed,gameObject.transform);
            tmp.transform.localPosition=Vector3.zero;
            tmp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);  

            crop=tmp.GetComponent<CropBehaviour>();
            crop.transform.localPosition=Vector3.zero;
            crop.Plant();



        }
        else
        {
           crop.Growth();
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