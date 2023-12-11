using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Timers;
using Unity.VisualScripting;
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

    public ItemName itemName;
    public bool isTool;
    public int amount;
    
    public Sprite icon;
    
    Renderer renderer;
    
    public Item(ItemName itemNameVar, bool isToolVar, int amountVar = 1)
    {
        itemName = itemNameVar;
        isTool = isToolVar;
        amount = amountVar;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
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
        StartCoroutine(StartFadingOut());
    }

    IEnumerator StartFadingOut()
    {
        yield return StartCoroutine(FadingOut());
        Destroy(gameObject);
    }

}
