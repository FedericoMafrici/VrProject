using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScenario : MonoBehaviour
{
    public enum Scenario
    {
        Farm,
        Savannah
    }
    
    [SerializeField] private List<AudioClip> soundtracks = new List<AudioClip>();

    private Scenario scenario;
    private Dictionary<Scenario, int> scenarioToSoundtrack = new Dictionary<Scenario, int>();

    public void Start()
    {
        scenarioToSoundtrack[Scenario.Farm] = 0;
        scenarioToSoundtrack[Scenario.Savannah] = 1;
        
        SelectScenario(Scenario.Farm);
    }

    public void SelectScenario(Scenario newScenario)
    {
        if (scenarioToSoundtrack.ContainsKey(scenario))
        {
            if(gameObject.transform.GetComponent<AudioSource>().isPlaying)
                gameObject.transform.GetComponent<AudioSource>().Stop();
            scenario = newScenario;
            gameObject.transform.GetComponent<AudioSource>().clip = soundtracks[scenarioToSoundtrack[scenario]];
            gameObject.transform.GetComponent<AudioSource>().Play();
        }
    }
    
}
