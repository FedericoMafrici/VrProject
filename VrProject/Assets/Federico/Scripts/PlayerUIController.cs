using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIController : MonoBehaviour
{
    public bool currState=true;
    public GameObject canvasTrigger;
    // Start is called before the first frame update
    
    void Start()
    {
        canvasTrigger=GameObject.FindWithTag("AnimalsUI");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
    {
        Debug.Log(currState);
        currState=!currState;
        canvasTrigger.SetActive(currState);
        
    }
    }
}
