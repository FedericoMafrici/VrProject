using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeVolume : MonoBehaviour {


    [SerializeField] private Color _gizmosColor = Color.green;
    private float _radius;
    private SphereCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider col = GetComponent<SphereCollider>();
        if (col != null) {
            _radius = col.radius;
            _collider = col;
        } else {
            float defaultRadiusValue = 2.0f;
            Debug.LogWarning(transform.name + " is a RangeVolume but has no SphereCollider, setting Radius to " + defaultRadiusValue);
            _radius = defaultRadiusValue;
        }
        
    }

    public float getRadius() {
        return _radius;
    }

    private void OnDrawGizmosSelected() {
        DrawSphereColliderGizmo();
    }

    private void DrawSphereColliderGizmo() {
        if (_collider == null)
            return;

        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(_collider.transform.position, _collider.radius);
    }
    
}
