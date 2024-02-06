using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public bool currState=false;
    public GameObject canvasTrigger;
    // Start is called before the first frame update
    public UiGameManager UiManager;
    void Start()
    {
        canvasTrigger.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
    {
        currState=!currState;
        Debug.Log(currState);
        if(currState)
        {
            
            UiManager.StartUI();
            
        }
        else
        {
       
        UiManager.CloseUI();
        }
    }
    }
}
