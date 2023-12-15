using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Timers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New item", menuName = "Item/Create New Item")]
public class Item : Grabbable
{
    public enum ItemName
    {
        WateringCan,
        Shaver,
        HorseShoe,
        Hammer,
        AppleSeed,
        WheatSeed,
        CarrotsSprout,
        Egg,
        Wool,
        Sponge,
        Apple,
        Carrot,
        EarOfWheat,
        Bucket,
        Milk,
        Pomade
    }

    public enum ItemCategory
    {
        Tool,
        Product,
        Food
    }

    public ItemName itemName;
    public ItemCategory itemCategory;
    
    public Sprite icon;
    
    Renderer renderer;
    public bool isFading = false;

    public Item(ItemName itemName, ItemCategory itemCategory)
    {
        this.itemName = itemName;
        this.itemCategory = itemCategory;
    }

    public void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    IEnumerator FadingOut()
    {
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void StartFading()
    {
        isFading = true;
        StartCoroutine(StartFadingOut());
    }

    IEnumerator StartFadingOut()
    {
        yield return StartCoroutine(FadingOut());
        Destroy(gameObject);
    }

    /*public void Show()
    {
        Color c = renderer.material.color;
        c.a = 1;
        renderer.material.color = c;
        gameObject.SetActive(true);
    }*/
}
