using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZioTobia : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private List<Dialogue> dialogues;

    public int currentDialogue = 0;
        
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P)
            && Vector3.Distance(player.transform.position, gameObject.transform.position) < 5)
        {
            GetComponent<DialogueTrigger>().TriggerDialogueManager(dialogues[currentDialogue]);
        }
    }
}
