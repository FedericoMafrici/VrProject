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
        Sponge,
        ChickenFood,
        Bucket,
        ClosedPomade,
        BucketMilk,
        OpenPomade,
        Egg,
        Wool,
        Apple,
        Carrot,
        EarOfWheat,
        TreeBranch,
        Leaf,
        BaobabFruit,
        NoItem, //needed for TargetMinigameActivator
        Shovel,
        AcaciaSeed,
        BaobabSeed
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
    
    public Sprite icon;
    MeshRenderer renderer;
    
    public bool isDeposited = false;
    [NonSerialized] public bool isFading = false;
    [NonSerialized] public bool isCollected = false;
    
    private AudioSource emitter;
    public AudioClip grabSound;
    public AudioClip usageSound;

    public Item() {}

    public void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        emitter = GetComponent<AudioSource>();
        emitter.volume = 1f;
        if (grabSound == null)
            grabSound = (AudioClip) Resources.Load("Sounds/GeneralSound");
        if (usageSound == null)
            usageSound = (AudioClip) Resources.Load("Sounds/GeneralSound");
    }

    public void StartFading()
    {
        // isFading = true;
        // StartCoroutine(StartFadingOut());
        Destroy(gameObject);
    }

    IEnumerator StartFadingOut()
    {
        foreach (Material material in renderer.materials)
        {
            material.SetFloat("_Mode", 3);
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
        }
        
        yield return StartCoroutine(FadingOut());
        Destroy(gameObject);
    }
    
    IEnumerator FadingOut()
    {
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            foreach (Material material in renderer.materials)
            {
                Color c = material.color;
                c.a = f;
                material.color = c;
            }
            yield return new WaitForSeconds(0.05f);
        }
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
