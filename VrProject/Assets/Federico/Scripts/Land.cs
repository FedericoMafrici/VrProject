using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour  
{
    [Header("Crops")]
    //The crop prefab to instantiate
    public GameObject cropPrefab;
   // private farm
    //The crop currently planted on the land
    CropBehaviour cropPlanted = null;
    
    public  void Interact()
    {
        //Instantiate the crop object parented to the land
            if(cropPlanted==null){
            GameObject cropObject = Instantiate(cropPrefab, gameObject.transform);
           Debug.Log(gameObject.transform.position);
            //Access the CropBehaviour of the crop we're going to plant
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            //Plant it with the seed's information
            cropPlanted.Plant();
            }
            else
            {
              cropPlanted.Growth();
            }
            
    }
}
