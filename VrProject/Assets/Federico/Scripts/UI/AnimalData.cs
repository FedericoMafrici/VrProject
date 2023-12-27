using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName ="AnimalData")]
public class AnimalData : ScriptableObject
{
    // Descrizione degli animali 
    public string animalDescription;

    //Descrizione delle missioni degli animali 
    public string animalMissionsList;
    
    // immagine da visualizzare
    public Sprite AnimalSprite;
}
