using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum SpriteType {
    DOT,
    HAND,
}
struct SpriteData {
    public SpriteData(SpriteType t, float s, Sprite sr) {
        type = t;
        scale = s;
        sprite = sr;
        if (sprite == null) {
            Debug.LogError("No sprite reference for the following sprite type: " + type);
        }
    } 

    public SpriteType type;
    public float scale;
    public Sprite sprite;
}

public class SpriteManager : MonoBehaviour {

    [SerializeField] private Sprite _dotSprite;
    [SerializeField] private Sprite _handSprite;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private SpriteData _dotSpriteData;
    private SpriteData _handSpriteData;
    private Dictionary<SpriteType, SpriteData> _spritesData = new Dictionary<SpriteType, SpriteData>;

    // Start is called before the first frame update
    void Start() {
        if (_dotSprite == null)
            Debug.LogError(transform.name + ": missing dot sprite");

        if (_handSprite == null)
            Debug.LogError(transform.name + ": missing hand sprite");

        //init sprites data
        _spritesData.Add(SpriteType.DOT, new SpriteData(SpriteType.DOT, 30f, _dotSprite));
        _spritesData.Add(SpriteType.HAND, new SpriteData(SpriteType.HAND, 10f, _handSprite));
        SetCurrenntSprite(SpriteType.DOT);
    }

    void SetCurrenntSprite(SpriteType key) {
        if (_spritesData.ContainsKey(key)) {
            SpriteData newSpriteData = _spritesData.GetValueOrDefault(key);
            _spriteRenderer.sprite = newSpriteData.sprite;
            _spriteRenderer.transform.localScale = new Vector3(newSpriteData.scale, newSpriteData.scale, newSpriteData.scale);
        }
    }

}
