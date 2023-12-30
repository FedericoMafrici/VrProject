using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lion : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = AnimalName.Lion;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
