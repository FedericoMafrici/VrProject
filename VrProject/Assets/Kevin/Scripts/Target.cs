using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Sprite greenTarget;
    public event Action OnTargetClicked; //modificato da Pietro (motivazione: se l'evento è statico minigiochi relativi ad animali diversi lo ricevono sempre e potrebbe creare problemi)
    private bool alreadyClicked = false; //aggiunto da Pietro, evita che si possa clicclare ripetutamente su un target che sta scomparendo

    private void OnMouseDown()
    {
        if (!alreadyClicked) {
            if (OnTargetClicked != null)
                OnTargetClicked();

            gameObject.GetComponent<SpriteRenderer>().sprite = greenTarget;

            Collider coll = gameObject.GetComponent<Collider>();
            if (coll != null) {
                coll.enabled = false;
            }

            StartFading();
            alreadyClicked = true;
        }
    }

    public void StartFading()
    {
        StartCoroutine(StartFadingOut());
    }

    IEnumerator StartFadingOut()
    {
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(FadingOut());
        Destroy(gameObject);
    }

    IEnumerator FadingOut()
    {
        for (float f = 1f; f >= -0.05; f -= 0.05f)
        {
            Color c = GetComponent<Renderer>().material.color;
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public bool IsAlreadyClicked() {
        return alreadyClicked;
    }
    
}
