using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class ChangeScenario : QuestEventReceiver
{
    public enum Scenario
    {
        Farm,
        Savannah
    }
    
    [SerializeField] private List<AudioClip> soundtracks = new List<AudioClip>(); // to be added through unity script in player

    public int currentScenario = 0;
    public Dictionary<int, Scenario> scenarioMap = new Dictionary<int, Scenario>();
    private int completed = 0;
    
    [SerializeField] private List<Quest> quests;
    [SerializeField] private Camera pipeCamera;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator pipeAnimator;

    public void Start()
    {
        scenarioMap[0] = Scenario.Farm;
        scenarioMap[1] = Scenario.Savannah;
        
        SelectScenario(0);
    }

    public void SelectScenario(int newScenario)
    {
        if (scenarioMap.ContainsKey(newScenario))
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            currentScenario = newScenario;
            gameObject.transform.GetComponent<AudioSource>().clip = soundtracks[newScenario];
            gameObject.transform.GetComponent<AudioSource>().Play();
            
            // ANIMAZIONE PLAYER O CAMBIO SCENA
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE)
        {
            completed++;
            if (completed == quests.Count)
            {
                playerCamera.gameObject.SetActive(false);
                player.GetComponent<FirstPersonController>().enabled = false;
                pipeCamera.gameObject.SetActive(true);
                
                StartCoroutine(PlayAnimation());
            }
        }
    }
    
    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1f);
        pipeAnimator.SetBool("IsSpawned", true);
        yield return new WaitForSeconds(4f);

        playerCamera.gameObject.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = true;
        pipeCamera.gameObject.SetActive(false);
    }
}
