using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Bar : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    private float _currentValue = 0.0f;

    [SerializeField] private RectTransform _bar;
    private float _fullWidth;
    private float _curWidth = 0.0f;

    [SerializeField] private float _animationSpeed = 10.0f;
    private Coroutine _adjustBarCoroutine;

    [SerializeField] protected CanvasGroup _barCanvasGroup;
    [SerializeField] protected bool _startHidden = true;
    protected bool _isHidden;

    public void Start() {
        if (_barCanvasGroup == null)
            Debug.LogError(transform.name + ": no bar canvas set");

        if (_bar == null)
            Debug.LogError(transform.name + ": not RectTransform reference to represent the bar");
        _fullWidth = _bar.rect.width;
        _bar.sizeDelta = new Vector2(0, _bar.rect.height);

        if (_startHidden) {
            _barCanvasGroup.alpha = 0.0f;
        } else {
            _barCanvasGroup.alpha = 1.0f;
        }
        _isHidden = _startHidden;
    }

    public void Increase(float delta) {
        SetValue(_currentValue + delta);
    }

    public void SetValue(float newValue) {
        _currentValue = Mathf.Clamp(newValue, 0, _maxValue);
        UpdateBarWidth();
    }

    private void UpdateBarWidth() {
        _curWidth = _currentValue * _fullWidth / _maxValue;
        if (_adjustBarCoroutine == null) {
            _adjustBarCoroutine = StartCoroutine(AdjustBarWidth());
            //StopCoroutine(_adjustBarCoroutine);
        }
        //_bar.sizeDelta = new Vector2(_curWidth, _bar.rect.height);
    }

    private IEnumerator AdjustBarWidth() {
        bool isFull = false;
        while (!isFull) {
            if (_curWidth >= _fullWidth) {
                _curWidth = _fullWidth;
                isFull = true;
            }

            _bar.sizeDelta = new Vector2(Mathf.Lerp(_bar.rect.width, _curWidth, Time.deltaTime * _animationSpeed), _bar.rect.height);
            

            yield return null;
        }
        //_bar.sizeDelta = new Vector2(_curWidth, _bar.rect.height);
    }

    public virtual void Hide() {
        _barCanvasGroup.alpha = 0;
        _isHidden = true;
    }

    public virtual void Show() {
        _barCanvasGroup.alpha =1.0f;
        _isHidden = false;
    }

    public bool IsHidden() {
        return _isHidden;
    }
}
