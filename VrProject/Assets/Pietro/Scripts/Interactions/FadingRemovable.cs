using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingRemovable : RemovablePart {
    [SerializeField] private float _alphaStep = 0.2f;

    private float _maxAlphaBeforeDestroying = 0.5f;
    private float _targetAlpha = 1.0f;
    private bool _partialFadeRunning = false;
    private Coroutine _fadingCoroutine = null;

    protected override void Start() {
        base.Start();
    }

    public override void Remove() {
        if (!_isRemoved) {
            _targetAlpha -= _alphaStep;
            if (_targetAlpha <= _maxAlphaBeforeDestroying) {
                _targetAlpha = _maxAlphaBeforeDestroying;

                if (_partialFadeRunning && _fadingCoroutine != null) {
                    StopCoroutine(_fadingCoroutine);
                    _partialFadeRunning = false;
                }

                base.Remove();
                StartCoroutine(FadeOut());
            }

            if (!_partialFadeRunning || _fadingCoroutine == null) {
                _fadingCoroutine = StartCoroutine(PartialFadeOut());
            }

            
        }
    }

    private IEnumerator PartialFadeOut() {
        _partialFadeRunning= true;
        float deltaTime = 0.05f;
        float deltaAlpha = 0.05f;
        bool isComplete = false;
        while(!isComplete) {
            Color newColor = _renderer.material.color;
            newColor.a -= deltaAlpha;
            if (newColor.a <= _targetAlpha) {
                newColor.a = _targetAlpha;
                isComplete = true;
                _partialFadeRunning = false;
            }
            _renderer.material.color = newColor;
            yield return new WaitForSeconds(deltaTime);
        }

    }

    public override void RemovalStarted() {
        Debug.LogWarning("<color=orange> NPC stopped moving </color>");
        MakeNPCStopMoving();
    }

    public override void RemovalStopped() {
        Debug.LogWarning("<color=cyan> NPC will start moving in 2 seconds </color>");
        MakeNPCStartMoving(true, 2f);
    }
}
