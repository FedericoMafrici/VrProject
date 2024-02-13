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

    public void Start()
    {
        
        SelectScenario(0);
    }

    public void SelectScenario(int newScenario)
    {
        if (scenarios[newScenario] != null)
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            currentScenario = newScenario;
            gameObject.transform.GetComponent<AudioSource>().clip = scenarios[newScenario].soundtrack;
            gameObject.transform.GetComponent<AudioSource>().Play();
            
           // CAMBIO SCENA

           mainCamera.transform.position = scenarios[newScenario].mainCameraPos;
           mainCamera.transform.rotation = Quaternion.Euler(scenarios[newScenario].mainCameraRot);
           player.transform.position = scenarios[newScenario].playerPos;
           player.transform.rotation = Quaternion.Euler(scenarios[newScenario].playerRot);

        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE)
        {
            scenarios[currentScenario].questsCompletedCount++;
            if (scenarios[currentScenario].questsCompletedCount == scenarios[currentScenario].questsList.Count)
            {
                mainCamera.gameObject.SetActive(false);
                player.GetComponent<FirstPersonController>().enabled = false;
                scenarios[currentScenario].pipeCamera.gameObject.SetActive(true);
                
                StartCoroutine(PlayAnimation());
            }
        }
    }
    
    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(1f);
        scenarios[currentScenario].pipeAnimator.SetBool("IsSpawned", true);
        yield return new WaitForSeconds(4f);

        mainCamera.gameObject.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = true;
        scenarios[currentScenario].pipeCamera.gameObject.SetActive(false);
    }

    public Transform GetCurrentScenarioParent() {
        return scenarios[currentScenario].spawnedObjectsParent;
    }
}
