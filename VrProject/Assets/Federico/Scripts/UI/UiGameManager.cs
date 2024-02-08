using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class UiGameManager : MonoBehaviour
{
 // Informazioni per ogni animale da aggiungere tramite inspector




public  AnimalData leone;

public AnimalData mucca;
[Header("Animals Ui information ")]// add here the information about animal 
[SerializeField] private List<AnimalDataUi> _animals; 
 private LinkedList<AnimalData> animalList = new LinkedList<AnimalData>();

 Dictionary<int, AnimalDataUi> animalMap = new Dictionary<int,AnimalDataUi>();
 private int currkey=0;
 private AnimalDataUi currAnimal=null;
// Informazioni per i controllì
public ControlSettingsData controlSettings;

//CAMERA

public GameObject playerCamera;


// Instanzia quelle che sono i vari componenti del canvas che poi cambieranno a seconda dell'interazione dell'utente 

public TMP_Text animalDescription;
public TMP_Text animalMissions;
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
 public PlayerUIController player;

 // COMPONENT FOR THE UI MENU 

 public Button soundOn;
 
 public Button soundOf;

 public Sprite soundButtonGreenON;

 public Sprite soundButtonPurpleON;

 public Sprite soundButtonGreenOFF;

 public Sprite soundButtonPurpleOFF;

  public Sprite leftArrow;
  public Sprite rightArrow;

void Start()
    {
            int i=0;
                //adding all the quest inserted into the quests list 
            if (_animals != null) {
                foreach (AnimalDataUi animal in _animals) {
                animalMap.Add(i++,animal);
                }
            
            }
         

       AnimalData[] animals= {leone, mucca };
       LinkedList<AnimalData> al = new LinkedList<AnimalData>(animals);
       animalList=al;
        // initializing fields:
        animalMissions.text="";
        animalDescription.text="";
        animalImage.sprite=null;
        if (animalMap.TryGetValue(currkey, out AnimalDataUi value))
        {
            currAnimal=value;
        }


      currNode = animalList.First;
       // Debugging Purpose 
      AnimalsCanvas.SetActive(true);
      Debug.Log(currAnimal.description[currAnimal.index]);
      
      animalDescription.text=currAnimal.description[currAnimal.index];
      animalImage.sprite=currAnimal.AnimalSprite;
      foreach(Quest quest in currAnimal._targetQuestSet)  
      {
       
        animalMissions.text+="-"+quest.GetQuestDescription();
        animalMissions.text+="\n";
      }
      setttingsList.text=controlSettings.comandi;
      additionalRules.text=controlSettings.regoleAggiuntive;
        // bottoni automatici 
      if(currkey==0)
      {
       // NextAnimalButton.Sprite=null;
       Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=null;
      }
      else
      {
        
        Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=leftArrow;
      }
    }
void DisplayAnimalData()
{
     animalMissions.text="";
   animalDescription.text="";
   animalImage.sprite=null;

     if (animalMap.TryGetValue(currkey, out AnimalDataUi value))
        {
            currAnimal=value;
        }
        else 
        {
            return;
        }

      animalDescription.text=currAnimal.description[currAnimal.index];
    
      animalImage.sprite=currAnimal.AnimalSprite;
      foreach(Quest quest in currAnimal._targetQuestSet)  
      {
        animalMissions.text+="-"+quest.GetQuestDescription();
        animalMissions.text+="\n";
      }   
      
            if(currkey==0)
      {
       // NextAnimalButton.Sprite=null;
       Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=null;
      }
      else
      {
        
        Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=leftArrow;
      }

}

public void DisplayAnimal()
{
   animalMissions.text="";
   animalDescription.text="";
   animalImage.sprite=null;
   currkey=0;
   DisplayAnimalData();
   AnimalsCanvas.SetActive(true);
   ControlSettings.SetActive(false); 
   Menu.SetActive(false);
   currNode=animalList.First;
   
    
}

public void DisplayMenu()
{
   ControlSettings.SetActive(false);
   AnimalsCanvas.SetActive(false);
   Menu.SetActive(true);
   
}

public void NextAnimal()
{
        currkey+=1;
        DisplayAnimalData();
}

public void PreviousAnimal()
{
        if(currkey!=0)
        {
            currkey-=1;
            DisplayAnimalData();
        }
        
}
public void DisplayControlSettings()
{
   AnimalsCanvas.SetActive(false);
   ControlSettings.SetActive(true);
}
public void StartUI()
{
   gameObject.SetActive(true);
   playerCamera.SetActive(false);
   ControlSettings.SetActive(false);
   AnimalsCanvas.SetActive(true);
   Menu.SetActive(false);
   Cursor.lockState =  CursorLockMode.None;
        Cursor.visible = true;
        currkey =0;
        // initializing fields:
        animalMissions.text="";
        animalDescription.text="";
        animalImage.sprite=null;
        // searching for the key otherwise we return 
        if (animalMap.TryGetValue(currkey, out AnimalDataUi value))
        {
            currAnimal=value;
        }
        else 
        {
            return;
        }
     
      animalDescription.text=currAnimal.description[currAnimal.index];
      animalImage.sprite=currAnimal.AnimalSprite;
      foreach(Quest quest in currAnimal._targetQuestSet)  
      {
         animalMissions.text+="-"+quest.GetQuestDescription();
        animalMissions.text+="\n";
      }
        // parte gestione dinamica dei bottoni
         if(currkey==0)
      {
       // NextAnimalButton.Sprite=null;
       Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=null;
      }
      else
      {
        
        Image tmp=PreviousAnimalButton.GetComponent<Image>();
       tmp.sprite=leftArrow;
      }
  





}
public void CloseUI()
{
   gameObject.SetActive(false);
   playerCamera.SetActive(true);
   ControlSettings.SetActive(false);
   AnimalsCanvas.SetActive(false);
   Menu.SetActive(false);
   currNode=animalList.First;
   Cursor.lockState = CursorLockMode.Locked ;
        Cursor.visible = false;
    }
public void CloseUIButton()
{
   player.currState=false;
   CloseUI();
   
   
}
public void StartGame()
{
   // TO DO -- BUILD SETTINGS ADD THE SCENE YOU WANT TO LOAD 
 //  SceneManager.LoadScene("GrowthSystemF");
}
public void ExitGame()
{
    Application.Quit();
}
public void MuteSounds()
    {
        // Trova tutti gli AudioListener nella scena
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // Disabilita gli AudioListener
        foreach (AudioListener listener in audioListeners)
        {
            listener.enabled = false;
        }
    }
public void EnableSounds()
    {
        // Trova tutti gli AudioListener nella scena
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();

        // Disabilita gli AudioListener
        foreach (AudioListener listener in audioListeners)
        {
            listener.enabled = true;
        }
    }
public void soundON()
{
soundOn.image.sprite= soundButtonGreenON;
soundOf.image.sprite=soundButtonPurpleOFF;

}
public void soundOFF()
{
soundOn.image.sprite= soundButtonPurpleON;
soundOf.image.sprite=soundButtonGreenOFF;
}



}


