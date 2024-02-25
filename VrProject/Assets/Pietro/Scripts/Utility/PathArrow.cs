using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathArrow : MonoBehaviour {

    [SerializeField] private Color _startColor;
    [SerializeField] private Color _endColor;
    [SerializeField] private float _period = 1f;
    [SerializeField] Renderer renderer;
    [Range(0f, 1f)][SerializeField] private float _startNextTiming = 0.5f;
    [SerializeField] private float _pauseTime = 0f;
    private bool _isRunning = false;
    private Coroutine _motionCoroutine = null;
    private PathArrow _next;
    private float _frequency;
    private bool _isLast = false;
    //private bool _forward = true;
    private float _motionElapsed = 0;

    private void Start() {
        
        _frequency = 1f / _period;
        if (_next == null) {
            _isLast = true;
        }

        if (renderer == null) {
            Renderer renderer = GetComponent<Renderer>();   
        }

        if (renderer == null) {
            Debug.LogError(name + "renderer is null");
        }

        renderer.material.color = _startColor;
    }

    //public void SetIsLast(bool value) { _isLast = value; }
    public void SetNext(PathArrow next) {
        _next = next;
        if (_next == null) {
            _isLast = true;
        }
    }

    public bool IsRunning() {
        return _isRunning;
    }

    IEnumerator MotionCoroutine() {
        Color a;
        Color b;
        float t;
        while (_isRunning) {
            bool _endCycle = false;

            _motionElapsed += Time.deltaTime;
            float _motionValue = _motionElapsed * _frequency;

            if (_motionValue > 2) {
                //reset cycle
                t = 0;
                _motionElapsed = 0;
                _endCycle = true;

            } else if (_motionValue > 1) {
                t = 2 - _motionValue;

            } else {
                t = _motionValue;
            }

            Color color = Color.Lerp(_startColor, _endColor, t);
            renderer.material.color = color;
            if (_motionValue >= 2*_startNextTiming && !_isLast && !_next.IsRunning()) {
                _next.DoMotion();
            }


            if (!_endCycle) {
                yield return null;
            } else {
                yield return new WaitForSeconds(_pauseTime);
            }
        }
    }

    public void DoMotion(bool overridePrevious = false) {
        if (_motionCoroutine == null || overridePrevious) {
            if (_motionCoroutine != null) {
                StopCoroutine(_motionCoroutine);
            }

            renderer.material.color = _startColor;
            _motionElapsed = 0;
            _isRunning = true;
            _motionCoroutine = StartCoroutine(MotionCoroutine());
        }

        
    }

    public void StopMotion() {
        _isRunning = false;
        if (_motionCoroutine != null) {
            _motionCoroutine = null;
        }

        if (!_isLast && _next != null) {
            _next.StopMotion();
        }
    }

    private void OnDisable() {
        StopMotion();
    }

}
