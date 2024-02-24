using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image; // Usando una direttiva 'using static
using TMPro;

public class NewTropheyAlert : MonoBehaviour
{
    // Start is called before the first frame update
  
    private const float alphaValue = 0.95f;
    private const float waitingTime = 4f;
    
    private Image background;
    private TMP_Text alertText;

    void Awake()
    {
        background = GetComponent<Image>();
        alertText = gameObject.transform.Find("AlertText").GetComponent<TMP_Text>();
        alertText.text = "Nuovo trofeo sbloccato!";

        Color c = background.color;
        c.a = 0;
        background.color = c;
        alertText.alpha = 0;
    }

  

    IEnumerator FadingOut()
    {
        for (int i = 0; i < 20; i++)
        {
            Color c = background.color;
            c.a -= 0.05f;
            background.color = c;
            alertText.alpha -= 0.05f;

            yield return new WaitForSeconds(0.05f);
        }
    }

    public void StartFading()
    {
        StartCoroutine(StartFadingOut());
    }

    IEnumerator StartFadingOut()
    {
        yield return new WaitForSeconds(waitingTime);
        yield return StartCoroutine(FadingOut());
    }

    public void Show()
    {
        Color c;
        if(background==null)
        {
        background = GetComponent<Image>();
        alertText = gameObject.transform.Find("AlertText").GetComponent<TMP_Text>();
        alertText.text = "Nuovo trofeo sbloccato!";
        c = background.color;
        c.a = 0;
        background.color = c;
        alertText.alpha = 0;
       
        }
        c = background.color;
        c.a = alphaValue;
        background.color = c;
        alertText.alpha = alphaValue;
        StartFading();
    }
}
