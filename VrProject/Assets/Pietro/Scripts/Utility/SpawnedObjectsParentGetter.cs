using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class SpawnedObjectsParentGetter {
    private ChangeScenario _changeScenario;
    private List<Transform> _parents;
    private static bool _inited = false;
    private static SpawnedObjectsParentGetter _instance = null;
    private SpawnedObjectsParentGetter(List<Transform> parents, ChangeScenario changeScenario) {
        _parents = parents;
        _changeScenario = changeScenario;
    }

    public static void Init(List<Transform> parents, ChangeScenario changeScenario) {
        if (!_inited) {
            _instance = new SpawnedObjectsParentGetter(parents, changeScenario);
            _inited = true;
        }
    }

    public static Transform GetParent() {
        if (_inited && _instance._changeScenario.currentScenario < _instance._parents.Count) {
            return _instance._parents[_instance._changeScenario.currentScenario];
        } else {
            return null;
        }
    }

}
