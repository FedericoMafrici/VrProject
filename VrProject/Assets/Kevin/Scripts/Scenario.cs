using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Scenario : MonoBehaviour
{
    public ScenarioName name;
    public List<Quest> questsList;
    public int questsCompletedCount;
    public Camera pipeCamera;
    public Animator pipeAnimator;
    public AudioClip soundtrack;
    public Scene scene;

    public Vector3 mainCameraPos;
    public Vector3 mainCameraRot;
    public Vector3 playerPos;
    public Vector3 playerRot;
}
