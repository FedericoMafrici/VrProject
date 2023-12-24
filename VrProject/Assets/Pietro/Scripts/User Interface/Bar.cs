using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;


public class Bar : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    private float _currentValue = 0.0f;

    [SerializeField] private RectTransform _barRectTransform;
    [SerializeField] private RectTransform _barBackgroundTransform;
    [SerializeField] private ProgressBarManager _barManager;
    private float _fullWidth;
    private float _curWidth = 0.0f;

    [SerializeField] private float _animationSpeed = 10.0f;
    private Coroutine _adjustBarCoroutine;

    [SerializeField] protected CanvasGroup _barCanvasGroup;
    [SerializeField] protected bool _startHidden = true;
    protected bool _isHidden;

    public event EventHandler<BarEventArgs> BarShown;
    public event EventHandler<BarEventArgs> BarHidden;

    public void Start() {
        if (_barCanvasGroup == null)
            Debug.LogError(transform.name + ": no bar canvas set");

        if (_barBackgroundTransform == null)
            Debug.LogError(transform.name + ": not RectTransform reference to bar background");

        if (_barRectTransform == null)
            Debug.LogError(transform.name + ": not RectTransform reference to represent the bar");

        if (_barManager != null) {
            _barManager.RegisterProgressBar(this);
        }

        _fullWidth = _barRectTransform.rect.width;
        _barRectTransform.sizeDelta = new Vector2(0, _barRectTransform.rect.height);

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

            _barRectTransform.sizeDelta = new Vector2(Mathf.Lerp(_barRectTransform.rect.width, _curWidth, Time.deltaTime * _animationSpeed), _barRectTransform.rect.height);
            

            yield return null;
        }
        //_bar.sizeDelta = new Vector2(_curWidth, _bar.rect.height);
    }

    public virtual void Hide() {
        _barCanvasGroup.alpha = 0;
        _isHidden = true;
        ThrowHiddenEvent();
    }

    public virtual void Show() {
        _barCanvasGroup.alpha =1.0f;
        _isHidden = false;
        ThrowShownEvent();
    }

    public bool IsHidden() {
        return _isHidden;
    }

    protected void ThrowHiddenEvent() {
        if (BarHidden != null) {
            BarHidden(this, new BarEventArgs(this));
        }
    }  

    protected void ThrowShownEvent() {
        if (BarShown != null) {
            BarShown(this, new BarEventArgs(this));
        }
    }

    public RectTransform GetBackgroundRectTransform() {
        return _barBackgroundTransform;
    }
}

public class BarEventArgs : EventArgs {
    public Bar bar;
    public BarEventArgs(Bar b) {
        bar = b;
    }
}
