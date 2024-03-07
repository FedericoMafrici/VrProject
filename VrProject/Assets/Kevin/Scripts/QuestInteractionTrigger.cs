using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteractionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Player") & (1 << other.transform.gameObject.layer)) > 0)
        {
            
        }
    }
}
