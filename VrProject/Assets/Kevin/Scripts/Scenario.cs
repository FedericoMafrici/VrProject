using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Scenario : MonoBehaviour
{
    public ScenarioName name;
    public GameObject questsList;
    public Camera pipeCamera;
    public AudioClip soundtrack;
    public GameObject scene;
    public bool unlocked;
    public AudioSource pipeEmitter;
    public Material sky;

    public Vector3 playerPos;
    public Vector3 playerRot;
}
