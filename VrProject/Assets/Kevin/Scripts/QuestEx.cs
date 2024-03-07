using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class QuestEx : Quest
{
    private string description = "Raccogli uova e mettile nell'apetto";
    private int numuova = 3;
    private int numraccolte = 0;
    private int mex = 0;

    protected override void Init()
    {
        // CODICE QUA
        
        base.Init();
    }

    public override string GetQuestDescription()
    {
        return description + mex +"/" + numuova;
    }

    protected override void OnQuestStart()
    {
        base.OnQuestStart();
        
        
        // CODICE QUI
    }

    public void Update()
    {
        if (_state == QuestState.ACTIVE)
        {
            mex++;
            Debug.Log("Mex");
            Progress();
        }

        if (mex == 3)
        {
            Complete();
        }
    }
}
