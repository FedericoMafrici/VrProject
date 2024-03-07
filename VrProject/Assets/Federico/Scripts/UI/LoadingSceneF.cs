using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneF : MonoBehaviour
{
    public void LoadFarm()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
                SceneManager.LoadScene("Main_Scene");
    }
}
