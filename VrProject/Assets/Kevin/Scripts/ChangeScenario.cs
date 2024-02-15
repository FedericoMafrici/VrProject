using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public enum ScenarioName
{
    Farm,
    Savannah
}

public class ChangeScenario : QuestEventReceiver
{
    public int currentScenario = 0;

    public List<Scenario> scenarios = new List<Scenario>();
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator pipeAnimator;

    public void Start()
    {
        SelectScenario(0);
    }

    public void SelectScenario(int newScenario)
    {
        if (newScenario < scenarios.Count && scenarios[newScenario] != null)
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            gameObject.transform.GetComponent<AudioSource>().clip = scenarios[newScenario].soundtrack;
            gameObject.transform.GetComponent<AudioSource>().Play();
            
           scenarios[currentScenario].scene.SetActive(false);
           scenarios[newScenario].scene.SetActive(true);

           player.transform.position = scenarios[newScenario].playerPos;
           player.transform.rotation = Quaternion.Euler(scenarios[newScenario].playerRot);
           
           currentScenario = newScenario;

           scenarios[currentScenario].pipeEmitter.Play();
        }
        else
        {
            Debug.Log("Errore: non esiste nessuno scenario successivo");
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE && !quest.IsStep())
        {
            scenarios[currentScenario].questsCompletedCount++;
            if (scenarios[currentScenario].questsCompletedCount == scenarios[currentScenario].questsList.transform.childCount)
            {
                scenarios[currentScenario + 1].unlocked = true;
                StartCoroutine(PlayAnimation());
            }
        }
    }
    
    IEnumerator PlayAnimation()
    {
        mainCamera.gameObject.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = false;
        scenarios[currentScenario].pipeCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);

        if (currentScenario == 0)
        {
            pipeAnimator.SetBool("IsSpawned", true);
            yield return new WaitForSeconds(2f);
        }
        
        yield return new WaitForSeconds(1f);

        mainCamera.gameObject.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = true;
        scenarios[currentScenario].pipeCamera.gameObject.SetActive(false);
    }

    public Transform GetCurrentScenarioParent() {
        return scenarios[currentScenario].spawnedObjectsParent;
    }
}
