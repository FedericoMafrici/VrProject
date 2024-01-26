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
using UnityEngine.UIElements;


public /*abstract*/ class Item : Grabbable
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
        EatableApple,
        ChickenFood,
        Carrot,
        EarOfWheat,
        Bucket,
        BucketMilk,
        Pomade,
        TreeBranch
    }

    public enum ItemCategory
    {
        Tool,
        Product,
        Consumable
    }

    public ItemName itemName;
    public ItemCategory itemCategory;
    public Vector3 depositPosition;
    public Vector3 depositRotation;
    public Vector3 grabRotation;
    
    public Sprite icon;
    Renderer renderer;
    
    public bool isDeposited = false;
    [NonSerialized] public bool isFading = false;
    [NonSerialized] public bool isCollected = false;
    
    public Item() {}

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
    
    public virtual UseResult Use(PlayerItemManager itemManager) {
        UseResult result = new UseResult();
        result.itemUsed = false;
        result.itemConsumed = false;
        return result;
    }
}

public struct UseResult {
    public bool itemUsed;
    public bool itemConsumed;
}
