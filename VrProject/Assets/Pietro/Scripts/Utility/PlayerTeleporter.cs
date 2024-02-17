using StarterAssets;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Search;
#endif
using UnityEngine;
using UnityEngine.UI;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] GameObject farm;
    [SerializeField] GameObject savana;
    [SerializeField] Transform farmSpawn;
    [SerializeField] Transform savanaSpawn;

    int activeScene = 0;
    bool isTeleporting = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTeleporting) {
            ToggleScene();
        }
    }

    private void ToggleScene() {
        if (activeScene == 0) {
            activeScene= 1;
            StartCoroutine(TeleportPlayer(savanaSpawn));
            farm.SetActive(false);
            savana.SetActive(true);
        } else if (activeScene == 1) {
            activeScene = 0;
            StartCoroutine(TeleportPlayer(farmSpawn));
            farm.SetActive(true);
            savana.SetActive(false);
        }
    }

    IEnumerator TeleportPlayer(Transform newPos) {
        isTeleporting= true;
        FirstPersonController fpc = player.GetComponent<FirstPersonController>();
        if (fpc != null) {
            fpc.enabled = false;
        }

        player.position = newPos.position;
        yield return new WaitForEndOfFrame();
        
        if (fpc != null) {
            fpc.enabled = true;
        }
        isTeleporting= false;
    }
}
