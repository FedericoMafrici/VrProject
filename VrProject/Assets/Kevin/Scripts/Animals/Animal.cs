using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Animal : MonoBehaviour
{
    public enum AnimalName
    {
        Chicken,
        Horse,
        Cow,
        Pig,
        Sheep,
        Giraffe,
        Zebra,
        Lion,
        Elephant
    }

    [SerializeField] private List<AudioClip> noises;
    
    public AnimalName name;
    private AudioSource noise;
    private bool isNoiseGenerated = true;
    
    public void Start()
    {
        noise = gameObject.transform.GetComponent<AudioSource>();
    }

    public void Update()
    {
        if (isNoiseGenerated == true)
        {
            isNoiseGenerated = false;
            float randomTime = Random.Range(10f, 30f);
            int randomClip = Random.Range(0, noises.Count);
            noise.clip = noises[randomClip];
            Invoke("PlayAudio", randomTime);
        }
    }

    private void PlayAudio()
    {
        noise.Play();
        isNoiseGenerated = true;
    }
    
}
