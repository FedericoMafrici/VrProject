using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Alert : QuestEventReceiver
{
    private const float alphaValue = 0.82f;
    private const float waitingTime = 7f;
    
    private Image background;
    private TMP_Text alertText;

    void Start()
    {
        background = GetComponent<Image>();
        alertText = gameObject.transform.Find("AlertText").GetComponent<TMP_Text>();
        alertText.text = "Nuove informazioni sul diario!";

        Color c = background.color;
        c.a = 0;
        background.color = c;
        alertText.alpha = 0;
    }

    protected override void OnEventReceived(Quest quest, EventType eventType)
    {
        if (eventType == EventType.COMPLETE && quest.GetInfos().Count > 0)
        {
            Show();
        }
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
        Color c = background.color;
        c.a = alphaValue;
        background.color = c;
        alertText.alpha = alphaValue;
        StartFading();
    }

}
