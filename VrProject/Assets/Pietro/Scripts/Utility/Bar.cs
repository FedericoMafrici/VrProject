using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    private float _currentValue = 0.0f;

    public void Increae(float newValue) {
        _currentValue = Mathf.Clamp(_currentValue + newValue, 0, _maxValue);
    }

    public void SetValue(float newValue) {
        _currentValue = Mathf.Clamp(newValue, 0, _maxValue);
    }
}
