using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTransformModifier : QuestEventReceiver {
    [SerializeField] private Transform _toCopy;

    protected override void Awake() {
        base.Awake();
        foreach (Quest q in _targetQuestSet) {
         
        }
    }

    protected override void OnEventReceived(Quest quest, EventType eventType) {
        quest.transform.position = _toCopy.position;
        quest.transform.rotation = _toCopy.rotation;
        if (quest.transform.parent == _toCopy.parent) {
            quest.transform.localScale = _toCopy.localScale;
        } else {
            ApplyGlobalScale(quest.transform, CalculateGlobalScale(_toCopy));
        }
        Debug.LogWarning(name + ": Transform component modified");
        SetEventSubscription(false, quest, eventType);
    }

    // Function to calculate the global scale of a transform
    Vector3 CalculateGlobalScale(Transform transform) {
        Vector3 globalScale = transform.localScale;

        // Traverse up the hierarchy to accumulate parent scales
        Transform parent = transform.parent;
        while (parent != null) {
            globalScale = Vector3.Scale(globalScale, parent.localScale);
            parent = parent.parent;
        }

        return globalScale;
    }

    // Function to apply global scale to a transform
    void ApplyGlobalScale(Transform transform, Vector3 globalScale) {

        // Cancel out parent scales by dividing by parent's local scale
        if (transform.parent != null) {
            globalScale = Vector3.Scale(globalScale, new Vector3(1f / transform.parent.localScale.x, 1f / transform.parent.localScale.y, 1f / transform.parent.localScale.z));
        }

        // Apply the global scale
        transform.localScale = globalScale;
    }
}
