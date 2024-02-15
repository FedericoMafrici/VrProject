using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Scenario : MonoBehaviour
{
    public ScenarioName name;
    public GameObject questsList;
    public int questsCompletedCount;
    public Camera pipeCamera;
    public AudioClip soundtrack;
    public GameObject scene;
    public Transform spawnedObjectsParent;
    public bool unlocked;
    public AudioSource pipeEmitter;

    public Vector3 playerPos;
    public Vector3 playerRot;
}
