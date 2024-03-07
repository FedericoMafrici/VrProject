using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingBar : Bar {
    private Coroutine _fadeCoroutine;

    new void Start() {
        base.Start();
    }

    private IEnumerator Fade(bool isFadingOut) {
        float timeDelta = 0.05f;
        float deltaAlpha = timeDelta;
        float targetAlpha = 1.0f;
        if (isFadingOut) {
            deltaAlpha = -deltaAlpha;
            targetAlpha = 0.0f;
        } else {
            ThrowShownEvent();
        }

        bool isCompleted = false;

        while (!isCompleted) {
            _barCanvasGroup.alpha += deltaAlpha;
            if ((isFadingOut && _barCanvasGroup.alpha <= targetAlpha) || (!isFadingOut && _barCanvasGroup.alpha >= targetAlpha)) {
                _barCanvasGroup.alpha = targetAlpha;
                isCompleted = true;
            }
            yield return new WaitForSeconds(timeDelta);
        }
        _fadeCoroutine = null;
        
        if (isFadingOut) {
            ThrowHiddenEvent();
        }

        /*
        for (float alpha = 1f; alpha >= -0.05; alpha -= 0.05f) {
            c.a = f;
            GetComponent<Renderer>().material.color = c;
            yield return new WaitForSeconds(0.05f);
        }*/
    }


    public override void Show() {
        if (_isHidden && _fadeCoroutine != null) {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        if (_fadeCoroutine == null) {
            _fadeCoroutine = StartCoroutine(Fade(false));
            _isHidden=false;
        }
    }

    public override void Hide() {
        if(!_isHidden && _fadeCoroutine != null) {
            StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = null;
        }

        if (!_isHidden) {
            _fadeCoroutine = StartCoroutine(Fade(true));
            _isHidden=true;
        }
    }

}
