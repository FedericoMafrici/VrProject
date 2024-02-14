using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingLand : MonoBehaviour
{
    float yOffset=1.03f;
    
    public bool tree=false; 
    GameObject tmp;
    public CropBehaviour  crop=null;

    public event Action<FarmingLand, CropBehaviour, bool> CropPlanted;

    public  void Interact(Seed seed)
    {
        Debug.Log(" finalmente hai puntato alla terra");
        
        if(crop==null && !tree)
        {
            // istanzio l'oggetto crop che avrà uno script CropbBehaviour e tengo una reference 
              if(seed.itemName==Item.ItemName.AppleSeed)
                   {
                    this.tree=true;
                   }
            tmp=Instantiate(seed.crop,gameObject.transform);
            tmp.transform.parent = this.transform;
            tmp.transform.localPosition=new Vector3(0.0f,0.0f,0.0f);
            //tmp=Instantiate(seed.seed,gameObject.transform);
            //tmp.transform.position=new Vector3(0.0f, 0.0f, 0.0f);
            crop=tmp.GetComponent<CropBehaviour>();
            crop.transform.localPosition=new Vector3(0.0f,0.0f,0.0f);
            //crop.transform.localPosition=new Vector3(seed.coordinate.x, seed.coordinate.y, seed.coordinate.z);
            //crop.transform.localScale=new Vector3(0.4f,0.4f,0.4f);
            crop.Plant();


            if (CropPlanted != null) 
            {
                CropPlanted(this, crop, tree);
            }

        }
        else
        {
           if(crop!=null) {
           crop.Growth();
           }
        }
    }

    public void DestroyCrop() {

        if (crop!=null) {
            crop.DestroyCrop();
            tree = false;
            crop = null;
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