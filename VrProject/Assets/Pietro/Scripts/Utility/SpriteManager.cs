using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum SpriteType {
    DOT,
    HAND,
}
[System.Serializable]
public struct SpriteData {
    public SpriteData(SpriteType t, float s, Sprite sr, Color c) {
        Type = t;
        DefaultScale = s;
        Sprite = sr;
        DefaultColor = c;
        if (Sprite == null) {
            Debug.LogError("No sprite reference for the following sprite type: " + Type);
        }
    } 

    [SerializeField] public SpriteType Type;
    public float DefaultScale;
    public Sprite Sprite;
    public Color DefaultColor;
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
            _spriteRenderer.sprite = _currentSpriteData.Sprite;
            _spriteRenderer.color = _currentSpriteData.DefaultColor;
            RectTransform rectTransform = _spriteRenderer.transform.GetComponent<RectTransform>();

            if (rectTransform != null) {
                rectTransform.localScale = new Vector3(_currentSpriteData.DefaultScale, _currentSpriteData.DefaultScale, _currentSpriteData.DefaultScale);
            } else {
                Debug.LogWarning(_spriteRenderer.transform.name + " has no RectTransform component to set scale");
            }

        }
    }



    public void SetCurrentSpriteColor(Color color) {
        _currentSpriteData.DefaultColor = color;
        _spriteRenderer.color = _currentSpriteData.DefaultColor;
    }

    public void ResetCurrentSpriteColor() {
        if (_spritesDataDict.ContainsKey(_currentSpriteData.Type)) {
            _currentSpriteData.DefaultColor = _spritesDataDict.GetValueOrDefault(_currentSpriteData.Type).DefaultColor;
            _spriteRenderer.color = _currentSpriteData.DefaultColor;
        }
    }

}
