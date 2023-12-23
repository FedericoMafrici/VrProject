using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RemovableType {
    WOOL,
    MUD
}

public abstract class RemovablePart : MonoBehaviour {
    private bool _isRemoved;
    private Renderer _renderer;
    [SerializeField] private RemovableType _type;

    protected virtual void Start() {
        _renderer = GetComponent<Renderer>();
        if (_renderer == null ) {
            Debug.LogError(transform.name + " no renderer assigned");
        }
        _isRemoved= false;
    }
    public virtual void Remove() {
        _isRemoved = true;
    }

    protected IEnumerator FadeOut() {
        Debug.Log("Fading out");
        float timeDelta = 0.05f;
        float deltaAlpha = timeDelta;

        bool isCompleted = false;

        while (!isCompleted) {
            Color newColor = _renderer.material.color;
            newColor.a -= deltaAlpha;
            
            if (newColor.a <= 0f ) { 
                newColor.a = 0f;
                isCompleted= true;
            }

            _renderer.material.color = newColor;
            Debug.Log(transform.name + ": new color alpha is: " + _renderer.material.color.a);

            yield return new WaitForSeconds(timeDelta);
        }
        Debug.Log("Destroying part");
        Destroy(gameObject);
    }

    public RemovableType GetRemovableType() {
        return _type;
    }

    public bool IsRemoved() {
        return _isRemoved;
    }

}
