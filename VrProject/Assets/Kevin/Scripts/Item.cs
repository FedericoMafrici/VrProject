using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New item", menuName = "Item/Create New Item")]
public class Item : Grabbable
{
    public string itemName;
    public Image icon;
    public GameObject obj;

    public void Show()
    {
        obj.SetActive(true);
    }
    
    public void Hide()
    {
        obj.SetActive(false);
    }

    // public void Use()
    // {
    //     
    // }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
