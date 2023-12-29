using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public Sprite greenTarget;
    public static event Action OnTargetClicked;

    private void OnMouseDown()
    {
        OnTargetClicked?.Invoke();
        gameObject.GetComponent<SpriteRenderer>().sprite = greenTarget;
        StartFading();
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
    
}
