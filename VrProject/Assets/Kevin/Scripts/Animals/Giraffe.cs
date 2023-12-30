using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giraffe : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = AnimalName.Giraffe;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
