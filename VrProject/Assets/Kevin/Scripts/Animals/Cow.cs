using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cow : Animal
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        name = AnimalName.Cow;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
