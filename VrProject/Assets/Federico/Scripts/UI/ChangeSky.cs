using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSky : MonoBehaviour
{
    // Start is called before the first frame update
    public Material sky1;
    public Material sky2;

    public bool firstLevel=true;
   public void changeScenario()
   {
    Debug.Log("changeScenario chiamata");
    if(firstLevel) {
    RenderSettings.skybox = sky1;
    firstLevel=false;
    }
    else
    {
        RenderSettings.skybox=sky2;
        firstLevel=true;
    }
   }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            changeScenario();
        }
    }

 
}
