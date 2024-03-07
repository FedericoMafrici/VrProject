using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStructuredQuest : AbstractStructuredQuest
{
    [SerializeField] List<Quest> _stepList;

    protected override void Init() {
        _steps = _stepList;
        base.Init();
    }

}
