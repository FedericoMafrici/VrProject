using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = AnimalName.Elephant;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
