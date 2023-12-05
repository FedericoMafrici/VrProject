using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "New item", menuName = "Item/Create New Item")]
public class Item : MonoBehaviour
{
    public string itemName;

    // public Sprite icon;

    public GameObject mesh;

    public void Show()
    {
        mesh.SetActive(true);
    }
    
    public void Hide()
    {
        mesh.SetActive(false);
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
