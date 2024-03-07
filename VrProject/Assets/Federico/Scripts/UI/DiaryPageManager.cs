   using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiaryPageManager : MonoBehaviour
{
    public TMP_Text animalDescription;
    public TMP_Text animalMissions;
    public Button animalList;
    public Button gameControls;
    public Button menu;
    // Start is called before the first frame update
    void Start()
    {
        animalDescription.text="Welcome to my game";
        animalList.GetComponentInChildren<TMP_Text>().text="Welcome into summoner's rift";
    }

}
