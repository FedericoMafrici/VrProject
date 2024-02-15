using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;




public class UiGameManager : MonoBehaviour {
    // Informazioni per ogni animale da aggiungere tramite inspector


    [Header("Pannelli interattivi che stampano le missioni")]// add here the information about animal 
    public QuestListD uiPanel;

    public QuestListD PlantuiPanel;
    // questi da togliere 
    public AnimalData leone;

    public AnimalData mucca;
    [Header("Informazioni Animali Ui  ")]// add here the information about animal 
    [SerializeField] private List<AnimalDataUi> _animals;

    [Header("Informazioni Piante Ui  ")]// add here the information about animal 
    [SerializeField] private List<AnimalDataUi> _plants;
    private LinkedList<AnimalData> animalList = new LinkedList<AnimalData>();

    Dictionary<int, AnimalDataUi> animalMap = new Dictionary<int, AnimalDataUi>();

    Dictionary<int, AnimalDataUi> plantMap = new Dictionary<int, AnimalDataUi>();
    //map index 

    private int currAnimalKey = 0;
    private int currPlantKey = 0;

    private int animalNumber = 0;

    private int plantNumber = 0;
    private AnimalDataUi entity = null;
    // Informazioni per i controllì
    public ControlSettingsData controlSettings;



    //CAMERA

    public GameObject playerCamera;

    //public GameObject realPlayerCamera;
    // Instanzia quelle che sono i vari componenti del canvas che poi cambieranno a seconda dell'interazione dell'utente 
    [Header("Animal Panel Components")]// add here the information about animal 
    public TMP_Text description;
    public TMP_Text missions;
    public Button gameControls;
    public Button menu;
    public Button NextAnimalButton;
    public Button PreviousAnimalButton;
    public Image image;
    public Image AnimalNameTitle;
    private LinkedListNode<AnimalData> currNode;


    private LinkedListNode<GameObject> missionsList;


    // COMPONENT FOR SETTINGS UI 
    public TMP_Text setttingsList;
    public TMP_Text additionalRules;
    // CANVASTLIST FOR THE UI 
    public GameObject PlantsCanvas;
    public GameObject AnimalsCanvas;
    public GameObject ControlSettings;
    public GameObject Menu;
    public PlayerUIController player;



    // COMPONENT FOR THE UI MENU 
    [Header("Componenti UI MENU")]
    public Button soundOn;

    public Button soundOf;

    public Sprite soundButtonGreenON;

    public Sprite soundButtonPurpleON;

    public Sprite soundButtonGreenOFF;

    public Sprite soundButtonPurpleOFF;

    private bool currSoundState=true;
    public Sprite leftArrow;
    public Sprite rightArrow;
    // COMPONENT FOR THE PLANT UI 
    [Header("Componenti UI Piante")]
    public TMP_Text plantDescription;
    public TMP_Text plantMissions;
    public Image plantNameTitle;
    public Button NextPlantlButton;
    public Button PreviousPlantButton;
    public Image Plantsimage;

    // CHECKBOX TEXT

    private Dictionary<int, TMP_Text> descriptions = new Dictionary<int, TMP_Text>();

    void Start() {

        int i = 0;
        currAnimalKey = 0;
        currPlantKey = 0;
        //adding all the quest inserted into the quests list 
        if (_animals != null) {
            foreach (AnimalDataUi animal in _animals) {
                animalMap.Add(i++, animal);
            }
            animalNumber = i;
        }
        i = 0;
        if (_animals != null) {
            foreach (AnimalDataUi plant in _plants) {
                plantMap.Add(i++, plant);
            }
            plantNumber = i;
        }



        AnimalData[] animals = { leone, mucca };
        LinkedList<AnimalData> al = new LinkedList<AnimalData>(animals);
        animalList = al;
        // initializing fields:
        missions.text = "";
        description.text = "";
        image.sprite = null;
        if (animalMap.TryGetValue(currAnimalKey, out AnimalDataUi value)) {
            entity = value;
        }
        // 


        currNode = animalList.First;
        // Debugging Purpose 
        //AnimalsCanvas.SetActive(true);
        Debug.Log(entity.description[entity.index]);
        AnimalNameTitle.sprite = entity.AnimalTitleImage;
        description.text = entity.description[entity.index];
        image.sprite = entity.currAnimalImage;
        foreach (Quest quest in entity._targetQuestSet) {

            // missions.text+="-"+quest.GetQuestDescription();
            // missions.text+="\n";
            uiPanel.AddMission(quest);


        }
        setttingsList.text = "WASD → movimento \n movimento Mouse → rotazione visuale/Movimento cursore (nei menù) \n click sinistro → utilizza oggetto attivo/Interazione contestuale/Selezione elementi (nei menù) \n click destro → interazione mano libera \n Q → lascia oggetto E → raccogli oggetto/parla con NPC \n P → avanza dialogo/Avanza pop-up tutorial \n tasti numerici → selezione oggetti nella hotbar \n Esc → apri menù \n ";
        additionalRules.text = "le galline produranno periodicamente uova basta avere un po di pazienza! \n usando la pala è possibile rimuovere alberi e piante indesiderate! \n se non sai piu come procedere consulta il tuo diario per sapere cosa ti manca \n incontrando degli animali sbloccherai delle preziosi informazioni";
        // bottoni automatici 
        if (currAnimalKey == 0) {
            // NextAnimalButton.Sprite=null;
            Image tmp = PreviousAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;
        } else if (currAnimalKey == animalNumber - 1) {

            Image tmp = NextAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;

            tmp.sprite = leftArrow;
        } else {
            Image tmp = PreviousAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = leftArrow;
            // same for right arrow 
            tmp = NextAnimalButton.GetComponent<Image>();
            buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = rightArrow;
        }

    }

    void DisplayAnimalData() {
        missions.text = "";
        description.text = "";
        image.sprite = null;

        if (animalMap.TryGetValue(currAnimalKey, out AnimalDataUi value)) {
            entity = value;
        } else {
            return;
        }
        
        description.text = entity.description[entity.index];
        AnimalNameTitle.sprite = entity.AnimalTitleImage;
        image.sprite = entity.currAnimalImage;
        foreach (Quest quest in entity._targetQuestSet) {

            // missions.text+="-"+quest.GetQuestDescription();
            // missions.text+="\n";
            uiPanel.AddMission(quest);


        }

        if (currAnimalKey == 0) {
            // NextAnimalButton.Sprite=null;
            Image tmp = PreviousAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;
            PreviousAnimalButton.gameObject.SetActive(false);
        } else if (currAnimalKey == animalNumber - 1) {

            Image tmp = NextAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;

            tmp.sprite = leftArrow;
            NextAnimalButton.gameObject.SetActive(false);
        } else {
            Image tmp = PreviousAnimalButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = leftArrow;
            // same for right arrow 
            tmp = NextAnimalButton.GetComponent<Image>();
            buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = rightArrow;
            PreviousAnimalButton.gameObject.SetActive(true);
            NextAnimalButton.gameObject.SetActive(true);
        }
    }
    /*
    void DisplayAnimalData()
    {
         missions.text="";
        description.text="";
        image.sprite=null;

         if (animalMap.TryGetValue(currAnimalKey, out AnimalDataUi value))
            {
                entity=value;
            }
            else 
            {
                return;
            }

          description.text=entity.description[entity.index];

          image.sprite=entity.AnimalSprite;
          foreach(Quest quest in entity._targetQuestSet)  
          {
            missions.text+="-"+quest.GetQuestDescription();
            missions.text+="\n";
          }   

          if(currAnimalKey==0)
          {
           // NextAnimalButton.Sprite=null;
           Image tmp=PreviousAnimalButton.GetComponent<Image>();
           Color buttonColor=tmp.color;
           buttonColor.a=0f;
           tmp.color=buttonColor;
          }
          else if(currAnimalKey==animalNumber-1)
          {

            Image tmp=NextAnimalButton.GetComponent<Image>();
            Color buttonColor=tmp.color;
            buttonColor.a=0f;
            tmp.color=buttonColor;

            tmp.sprite=leftArrow;
          }
          else 
          {
            Image tmp=PreviousAnimalButton.GetComponent<Image>();
            Color buttonColor=tmp.color;
            buttonColor.a=255f;
            tmp.color=buttonColor;
            tmp.sprite=leftArrow;
            // same for right arrow 
            tmp=NextAnimalButton.GetComponent<Image>();
            buttonColor=tmp.color;
            buttonColor.a=255f;
            tmp.color=buttonColor;
            tmp.sprite=rightArrow;
          }


    }
    */
    public void DisplayPlantData() {
        plantMissions.text = "";
        plantDescription.text = "";
        Plantsimage.sprite = null;
        PlantuiPanel.deleteMissions();
        if (plantMap.TryGetValue(currPlantKey, out AnimalDataUi value)) {
            entity = value;
        } else {
            return;
        }


        Plantsimage.sprite = entity.currAnimalImage;
        plantNameTitle.sprite=entity.AnimalTitleImage;
        foreach (Quest quest in entity._targetQuestSet) {
            PlantuiPanel.AddMission(quest);
        }

        plantDescription.text = entity.description[entity.index];
        if (currPlantKey == 0) {
            // NextAnimalButton.Sprite=null;
            Image tmp = PreviousPlantButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;
            PreviousAnimalButton.gameObject.SetActive(false);
        } else if (currPlantKey == plantNumber - 1) {

            Image tmp = NextPlantlButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 0f;
            tmp.color = buttonColor;
            NextAnimalButton.gameObject.SetActive(false);

        } else {
            Image tmp = PreviousPlantButton.GetComponent<Image>();
            Color buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = leftArrow;
            // same for right arrow 
            tmp = NextPlantlButton.GetComponent<Image>();
            buttonColor = tmp.color;
            buttonColor.a = 255f;
            tmp.color = buttonColor;
            tmp.sprite = rightArrow;
            PreviousAnimalButton.gameObject.SetActive(true);
            NextAnimalButton.gameObject.SetActive(true);
        }

    }
    public void DisplayAnimal() {
        // restting button 
        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;

        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);

        missions.text = "";
        description.text = "";
        image.sprite = null;
        uiPanel.deleteMissions();
        currAnimalKey = 0;
        currPlantKey = 0;
        DisplayAnimalData();
        AnimalsCanvas.SetActive(true);
        PlantsCanvas.SetActive(false);
        ControlSettings.SetActive(false);
        Menu.SetActive(false);
        currNode = animalList.First;


    }
    public void DisplayPlant() {
        // restting button 
        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;

        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);

        plantMissions.text = "";
        plantDescription.text = "";
        Plantsimage.sprite = null;
        currAnimalKey = 0;
        currPlantKey = 0;
        DisplayPlantData();
        AnimalsCanvas.SetActive(false);
        PlantsCanvas.SetActive(true);
        ControlSettings.SetActive(false);
        Menu.SetActive(false);
        currNode = animalList.First;
    }
    public void DisplayMenu() {
        // restting button 
        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;

        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);
        if(currSoundState) 
        {
        soundON();
        }
        else
        {
        soundOFF();
        }
        ControlSettings.SetActive(false);
        PlantsCanvas.SetActive(false);
        AnimalsCanvas.SetActive(false);
        Menu.SetActive(true);

    }

    public void NextAnimal() {
        if (currAnimalKey + 1 < animalNumber) {
            uiPanel.deleteMissions();
            currAnimalKey += 1;
            DisplayAnimalData();
        }
    }

    public void PreviousAnimal() {
        if (currAnimalKey != 0) {
            uiPanel.deleteMissions();
            currAnimalKey -= 1;
            DisplayAnimalData();
        }

    }
    public void NextPlant() {
        if (currPlantKey + 1 < plantNumber) {
            PlantuiPanel.deleteMissions();
            currPlantKey += 1;
            DisplayPlantData();
        }
    }

    public void PreviousPlant() {
        if (currPlantKey != 0) {
            PlantuiPanel.deleteMissions();
            currPlantKey -= 1;
            DisplayPlantData();
        }

    }
    public void DisplayControlSettings() {
        // restting button 
        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;

        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);


        AnimalsCanvas.SetActive(false);
        ControlSettings.SetActive(true);
        PlantsCanvas.SetActive(false);
        Menu.SetActive(false);
    }
    public void StartUI() {
        // restting button 
        InputManager.DisableInteractions();
            InputManager.DisableMovement();

        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;

        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);

        currAnimalKey = 0;
        currPlantKey = 0;
        gameObject.SetActive(true);
        DisplayAnimal();
        playerCamera.SetActive(false);
        ControlSettings.SetActive(false);
        AnimalsCanvas.SetActive(true);
        Menu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
    }
    public void CloseUI() {
       InputManager.EnableInteractions();
                InputManager.EnableMovement();

        // restting button 
        Image tmp = PreviousPlantButton.GetComponent<Image>();
        Color buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;
        // same for right arrow 
        tmp = NextPlantlButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;
        // Animal button
        tmp = PreviousAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = leftArrow;

        tmp = NextAnimalButton.GetComponent<Image>();
        buttonColor = tmp.color;
        buttonColor.a = 255f;
        tmp.color = buttonColor;
        tmp.sprite = rightArrow;


        PreviousAnimalButton.gameObject.SetActive(true);
        NextAnimalButton.gameObject.SetActive(true);


        gameObject.SetActive(false);
        playerCamera.SetActive(true);
        PlantsCanvas.SetActive(false);
        ControlSettings.SetActive(false);
        AnimalsCanvas.SetActive(false);
        Menu.SetActive(false);
        currNode = animalList.First;
        uiPanel.deleteMissions();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void CloseUIButton() {
        player.currState = false;
        CloseUI();


    }
    public void StartGame() {
        // TO DO -- BUILD SETTINGS ADD THE SCENE YOU WANT TO LOAD 
        //  SceneManager.LoadScene("GrowthSystemF");
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void MuteSounds() {
        // Trova tutti gli AudioListener nella scena
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        playerCamera.SetActive(true);
       AudioListener playerListener= playerCamera.GetComponent<AudioListener>();

        if (playerListener == null)
        {
            Debug.LogError("Nessun componente AudioListener trovato sulla camera principale.");
        }

          
        playerListener.enabled=false;
        playerCamera.SetActive(false);

        // Disabilita gli AudioListener
        foreach (AudioListener listener in audioListeners) {
            listener.enabled = false;
        }
    }
    public void EnableSounds() {
        // Trova tutti gli AudioListener nella scena
        AudioListener[] audioListeners = FindObjectsOfType<AudioListener>();
        playerCamera.SetActive(true);
       AudioListener playerListener= playerCamera.GetComponent<AudioListener>();

        if (playerListener == null)
        {
            Debug.LogError("Nessun componente AudioListener trovato sulla camera principale.");
        }

          
        playerListener.enabled=true;
        playerCamera.SetActive(false);

        // Disabilita gli AudioListener
        foreach (AudioListener listener in audioListeners) {
            listener.enabled = true;
        }
    }
    public void soundON() {
        soundOn.image.sprite = soundButtonGreenON;
        soundOf.image.sprite = soundButtonPurpleOFF;
        currSoundState=true;
    }
    public void soundOFF() {
        soundOn.image.sprite = soundButtonPurpleON;
        soundOf.image.sprite = soundButtonGreenOFF;
        currSoundState=false;
    }

    private void Update() {
        if (playerCamera != null) {
            //aggiunto da pietro, camera segue player camera senza doverla imparentare (imparentarla rompe tutto)
            this.transform.position = playerCamera.transform.position;
            this.transform.rotation = playerCamera.transform.rotation;
        }
    }

}


