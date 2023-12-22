using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;




public class UiGameManager : MonoBehaviour
{
 // Informazioni per ogni animale da aggiungere tramite inspector
public  AnimalData leone;

public AnimalData mucca;

// Informazioni per il menu 
// TO DO PERCHè NON SO COME REGOLARE LE IMPOSTAZIONI DEL SUONO ETC ETC 
// load scene etc, da fare alla fine 

// Informazioni per i controllì
public ControlSettingsData controlSettings;


// Instanzia quelle che sono i vari componenti del canvas che poi cambieranno a seconda dell'interazione dell'utente 

public TMP_Text animalDescription;
public TMP_Text animalMissions;
public Button animalList;
public Button gameControls;
public Button menu;
public Button NextAnimalButton;
public Button PreviousAnimalButton;
public Image animalImage;
private LinkedListNode<AnimalData> currNode;
// COMPONENT FOR SETTINGS UI 
public TMP_Text setttingsList;
public TMP_Text additionalRules;
// CANVASTLIST FOR THE UI 
 public GameObject AnimalsCanvas;
 public GameObject ControlSettings;
 public GameObject Menu;

void Start()
    {
      
      AnimalData[] animals= {leone, mucca };
      LinkedList<AnimalData> animalList = new LinkedList<AnimalData>(animals);
      currNode = animalList.First;
       // Debuggin Purpose 
      AnimalsCanvas.SetActive(true);
      Debug.Log(currNode.Value.animalDescription);
      Debug.Log(currNode.Value.animalMissionsList);
     
      animalDescription.text=currNode.Value.animalDescription;
      animalMissions.text=currNode.Value.animalMissionsList;
      animalImage.sprite=currNode.Value.AnimalSprite;
      
      // 
      setttingsList.text=controlSettings.comandi;
      additionalRules.text=controlSettings.regoleAggiuntive;
      
    }


void DisplayAnimalData( LinkedListNode<AnimalData> currAnimal)
{
      animalDescription.text=currAnimal.Value.animalDescription;
      animalMissions.text=currAnimal.Value.animalMissionsList; 
      animalImage.sprite=currNode.Value.AnimalSprite;     
}
public void NextAnimal()
{
      if(currNode.Next!=null)
      {
      currNode=currNode.Next;
      DisplayAnimalData(currNode);   
      } 
}

public void PreviousAnimal()
{
      if(currNode.Previous!=null)
      {
      currNode=currNode.Previous;
      DisplayAnimalData(currNode);   
      } 
}
public void DisplayControlSettings()
{
   AnimalsCanvas.SetActive(false);
   ControlSettings.SetActive(true);
}

public void DisplayMenu()
{
   
}





}


