using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item")]
public class ItemData : ScriptableObject
{
    public string description;

    //Icon to be displayed in UI
    public Sprite thumbnail;

    //GameObject to be shown in the scene
    public GameObject gameModel;
}
