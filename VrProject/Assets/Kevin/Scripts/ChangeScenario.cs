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
        currentScenario = 0;
        gameObject.transform.GetComponent<AudioSource>().clip = scenarios[currentScenario].soundtrack;
        gameObject.transform.GetComponent<AudioSource>().Play();
        SelectScenario(currentScenario);
    }

    public void SelectScenario(int newScenario)
    {
        if (newScenario < scenarios.Count && scenarios[newScenario] != null && newScenario != currentScenario)
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            gameObject.transform.GetComponent<AudioSource>().clip = scenarios[newScenario].soundtrack;
            gameObject.transform.GetComponent<AudioSource>().Play();
<<<<<<< HEAD
            
            scenarios[newScenario].scene.SetActive(true);
            player.transform.position = scenarios[newScenario].playerPos; 
            player.transform.rotation = Quaternion.Euler(scenarios[newScenario].playerRot);
            scenarios[currentScenario].scene.SetActive(false);
            currentScenario = newScenario;
            
            scenarios[currentScenario].pipeEmitter.Play();
=======

            if (currentScenario != newScenario) {
                scenarios[currentScenario].scene.SetActive(false);
                scenarios[newScenario].scene.SetActive(true);
            }

           player.transform.position = scenarios[newScenario].playerPos;
           player.transform.rotation = Quaternion.Euler(scenarios[newScenario].playerRot);
           
           currentScenario = newScenario;

            if (scenarios[currentScenario].pipeEmitter != null) {
                scenarios[currentScenario].pipeEmitter.Play();
            }
>>>>>>> d29e146dd578ad0967b57d3fcedc4d3e03db4f46
        }
        else
        {
            Debug.Log("Errore: non esiste nessuno scenario successivo");
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE)
        {
            scenarios[currentScenario + 1].unlocked = true;
            StartCoroutine(PlayAnimation());
        }
    }
    
    IEnumerator PlayAnimation()
    {
        scenarios[currentScenario].pipeCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        // player.GetComponent<FirstPersonController>().enabled = false;

        yield return new WaitForSeconds(1f);
        
        if (currentScenario == 0)
        {
            pipeAnimator.SetBool("IsSpawned", true);
            yield return new WaitForSeconds(2f);
        }
        
        yield return new WaitForSeconds(1f);

        mainCamera.gameObject.SetActive(true);
        scenarios[currentScenario].pipeCamera.gameObject.SetActive(false);
        // player.GetComponent<FirstPersonController>().enabled = true;
    }
}
