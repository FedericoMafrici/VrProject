using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum SpriteType {
    DOT,
    INTERACT_DOT,
    HAND,
}
[System.Serializable]
public struct SpriteData {
    /*
    public SpriteData(SpriteType t, float s, Sprite sr, UnityEngine.Color c) {
        Type = t;
        defaultScale = s;
        sprite = sr;
        defaultColor = c;
        currentColor = c;
        if (sprite == null) {
            Debug.LogError("No sprite reference for the following sprite type: " + Type);
        }
    } */

    [SerializeField] public SpriteType Type;
    public float defaultScale;
    public Sprite sprite;
    public UnityEngine.Color color;
    //public UnityEngine.Color currentColor;
    //public UnityEngine.Color defaultColor;

    /*
    public void SetCurrentColor(UnityEngine.Color color) {
        currentColor = color;
    }

    public void ResetColor() {
        currentColor = defaultColor;
    }*/
}

public class SpriteManager : MonoBehaviour {

    [SerializeField] private List<SpriteData> _spriteDataList = new List<SpriteData>();
    [SerializeField] private SpriteRenderer _spriteRenderer;

    protected SpriteData _currentSpriteData;
    private Dictionary<SpriteType, SpriteData> _spritesDataDict = new Dictionary<SpriteType, SpriteData>();

    // Start is called before the first frame update
    protected virtual void Start() {

        foreach (SpriteData sd in _spriteDataList) { 
            _spritesDataDict.Add(sd.Type, sd);
        }

        if (_spriteDataList.Count > 0) {
            SetCurrentSprite(_spriteDataList[0].Type);
        }
    }

    public void UpdateCurrentSprite(SpriteType key) {
        if (_currentSpriteData.Type != key) {
            SetCurrentSprite(key);
        }
    }

    private void SetCurrentSprite(SpriteType key) {
        if (_spritesDataDict.ContainsKey(key)) {
            _currentSpriteData = _spritesDataDict.GetValueOrDefault(key);
            _spriteRenderer.sprite = _currentSpriteData.sprite;
            _spriteRenderer.color = _currentSpriteData.color;
            RectTransform rectTransform = _spriteRenderer.transform.GetComponent<RectTransform>();

            if (rectTransform != null) {
                rectTransform.localScale = new Vector3(_currentSpriteData.defaultScale, _currentSpriteData.defaultScale, _currentSpriteData.defaultScale);
            } else {
                Debug.LogWarning(_spriteRenderer.transform.name + " has no RectTransform component to set scale");
            }

        }
    }


    /*
    public void SetCurrentSpriteColor(UnityEngine.Color color) {
        if (_spritesDataDict.ContainsKey(_currentSpriteData.Type)) {
            _spritesDataDict.GetValueOrDefault(_currentSpriteData.Type).SetCurrentColor(color);
        }
        _currentSpriteData.currentColor = color;
        _spriteRenderer.color = color;
    }

    public void ResetCurrentSpriteColor() {
        if (_spritesDataDict.ContainsKey(_currentSpriteData.Type)) {
            _spritesDataDict.GetValueOrDefault(_currentSpriteData.Type).ResetColor();
        }
        _currentSpriteData.currentColor = _currentSpriteData.defaultColor;
        _spriteRenderer.color = _currentSpriteData.currentColor;
    }*/

}
