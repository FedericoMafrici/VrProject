using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStructuredQuest : AbstractStructuredQuest
{
    [SerializeField] List<Quest> _stepList;
    // Start is called before the first frame update
    protected override void Start()
    {
        _steps = _stepList;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
