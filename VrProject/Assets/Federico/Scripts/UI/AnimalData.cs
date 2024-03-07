using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName ="AnimalData")]


public class AnimalData : ScriptableObject
{
    /*
    public class AnimalMi˜ssion: MonoBehaviour
{
    public QuestID questId;

    public string ciao;
}
*/
    [Header("Missions Related to the animals")]
  //  [SerializeField] private List<Quest> _targetQuestList; 
    // ENUM ANIMAL 

    // Descrizione degli animali    
    public string animalDescription;

    //Descrizione delle missioni degli animali 
    public string animalMissionsList;
    
    // immagine da visualizzare
    public Sprite AnimalSprite;
     HashSet<Quest> quests = new HashSet<Quest>();


 protected virtual void Awake()
 {
            //adding all the quest inserted into the quests list 
            /*
        if (_targetQuestList != null) {
            foreach (Quest quest in _targetQuestList) {
               quests.Add(quest);
            }
        
        }
            */
 }
}
