using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using StarterAssets;
using UnityEngine;

public enum ScenarioName
{
    Farm,
    Savannah
}

public class ChangeScenario : QuestEventReceiver
{
    public int currentScenario;
    private bool firstLevel=true;

    public Material sky1;
    public Material sky2;
    public List<Scenario> scenarios = new List<Scenario>();
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject player;
    [SerializeField] private Animator pipeAnimator;

    public void Start()
    {
        currentScenario = 0;
        gameObject.transform.GetComponent<AudioSource>().clip = scenarios[currentScenario].soundtrack;
        gameObject.transform.GetComponent<AudioSource>().Play();
    }

    public void SelectScenario(int newScenario)
    {
        if (newScenario < scenarios.Count && scenarios[newScenario] != null && newScenario != currentScenario)
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            gameObject.transform.GetComponent<AudioSource>().clip = scenarios[newScenario].soundtrack;
            gameObject.transform.GetComponent<AudioSource>().Play();
            
            scenarios[newScenario].scene.SetActive(true);
            if(newScenario == 0)
                pipeAnimator.SetBool("IsSpawned", true);

            player.GetComponent<FirstPersonController>().enabled = false;
            
            player.transform.position = scenarios[newScenario].playerPos;
            player.transform.rotation = Quaternion.Euler(scenarios[newScenario].playerRot);

            StartCoroutine(EnableFPController());
            
            scenarios[currentScenario].scene.SetActive(false);
            
            currentScenario = newScenario;
            
            if (scenarios[currentScenario].pipeEmitter != null) {
                scenarios[currentScenario].pipeEmitter.Play();
            }
            //AGGIUNTO DA FEDE
            if(sky1!=null && sky2!=null) {
                if(firstLevel) {
                RenderSettings.skybox = sky1;
                firstLevel=false;
                }
                else
                {
                    RenderSettings.skybox=sky2;
                    firstLevel=true;
                }
            }
        }
        else
        {
            Debug.Log("Errore: non esiste nessuno scenario successivo");
        }
    }

    IEnumerator EnableFPController()
    {
        yield return new WaitForEndOfFrame();
        player.GetComponent<FirstPersonController>().enabled = true;
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE)
        {
            if (scenarios.Count > currentScenario + 1) {
                scenarios[currentScenario + 1].unlocked = true;
                StartCoroutine(PlayAnimation());
            }
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
