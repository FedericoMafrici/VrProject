using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public enum EmissionType {
    NONE,
    FOOD,
    HEART
}

[Serializable]
public struct EmissionData {
    public float frequency;
    public List<GameObject> toEmitList;
    public EmissionType emissionType;
    public float moveSpeed;
    public float lifetime;
}

public class ObjectEmitter : MonoBehaviour {

    Dictionary<EmissionType, EmissionData> _emissionDataDict = new Dictionary<EmissionType, EmissionData>();
    [SerializeField] float _pyramidWidth = 0.5f;
    [SerializeField] bool _setParenting = true;
    Dictionary<EmissionType, Coroutine> _emissionCoroutines = new Dictionary<EmissionType, Coroutine>();
    HashSet<GameObject> _spawnedObjects = new HashSet<GameObject>();
    bool wasDisabled = false;

    void Awake() {
        if (_pyramidWidth < 0) {
            _pyramidWidth = -_pyramidWidth;
        } 

        if (_pyramidWidth < 0.001) {
            Debug.LogWarning(this.name + ": pyramid with too small, setting it to 0.001");
            _pyramidWidth = 0.001f;
        }
    }

    public void AddEmission(EmissionData emission) {
        if (emission.toEmitList != null && emission.toEmitList.Count > 0 && !_emissionDataDict.ContainsKey(emission.emissionType)) {
            _emissionDataDict.Add(emission.emissionType, emission);

            if (_emissionCoroutines.ContainsKey(emission.emissionType)) {
                Coroutine toStop = _emissionCoroutines[emission.emissionType];

                if (toStop != null) {
                    StopCoroutine(toStop);
                }

                _emissionCoroutines.Remove(emission.emissionType);
            }

            _emissionCoroutines.Add(emission.emissionType, StartCoroutine(GenerateObjectsCoroutine(emission)));
        }
    }

    public void RemoveEmission(EmissionData emission) {
        if (_emissionDataDict.ContainsKey(emission.emissionType)) {
            _emissionDataDict.Remove(emission.emissionType);
            Coroutine toStop = _emissionCoroutines[emission.emissionType];

            if (toStop != null) {
                StopCoroutine(toStop);
            }

            _emissionCoroutines.Remove(emission.emissionType);
        }
    }

    IEnumerator GenerateObjectsCoroutine(EmissionData emission) {
        while (true) {
            
            int index = UnityEngine.Random.Range(0, emission.toEmitList.Count-1);
            GameObject spawned = Instantiate(emission.toEmitList[index]);
            
            if (spawned) {
                spawned.transform.position = this.transform.position;

                
                if (_setParenting) {
                    spawned.transform.SetParent(this.transform, true);
                }
                
                
                //spawned.transform.localScale = Vector3.Scale(new Vector3(emission.scale, emission.scale, emission.scale), spawned.transform.localScale);
                _spawnedObjects.Add(spawned);
                float x = UnityEngine.Random.Range(spawned.transform.position.x- (_pyramidWidth / 2f), spawned.transform.position.x + (_pyramidWidth / 2f));
                float y = spawned.transform.position.y + 1;
                float z = UnityEngine.Random.Range(spawned.transform.position.z - (_pyramidWidth / 2f), spawned.transform.position.z + (_pyramidWidth / 2f));
                Vector3 direction = (new Vector3(x, y, z) - this.transform.position).normalized;


                Coroutine moveCoroutine = StartCoroutine(MoveObject(spawned, direction, emission.moveSpeed));
                StartCoroutine(DestroyObject(spawned, emission.lifetime, moveCoroutine));

                yield return new WaitForSeconds(1/emission.frequency);
            } else {
                yield break;
            }
        }
    }

    IEnumerator MoveObject(GameObject toMove, Vector3 direction, float moveSpeed) {
        while (true) {
            // Move the object along the direction vector
            Vector3 toLookAt = new Vector3(Camera.main.transform.position.x, toMove.transform.position.y, Camera.main.transform.position.z);
            toMove.transform.LookAt(toLookAt);
            toMove.transform.Translate(direction * moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator DestroyObject(GameObject obj, float lifetime, Coroutine moveCoroutine) {
        if (lifetime > 0) {
            float toWait = Mathf.Clamp(lifetime - 1, 0, lifetime);

            yield return new WaitForSeconds(toWait);
            yield return FadeOut(obj, moveCoroutine);
                }
    }

    protected IEnumerator FadeOut(GameObject toFadeOut, Coroutine moveCoroutine) {
        float timeDelta = 0.05f;
        float deltaAlpha = timeDelta;


        bool isCompleted = false;
        Renderer _renderer = toFadeOut.GetComponent<Renderer>();

        if (_renderer != null) {
            while (!isCompleted) {
                Color newColor = _renderer.material.color;
                newColor.a -= deltaAlpha;

                if (newColor.a <= 0f) {
                    newColor.a = 0f;
                    isCompleted = true;
                }

                _renderer.material.color = newColor;

                yield return new WaitForSeconds(timeDelta);
            }
        } else {
            yield return new WaitForSeconds(1);
        }
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        _spawnedObjects.Remove(toFadeOut);
        Destroy(toFadeOut.gameObject);
    }

    private void OnDisable() {
        foreach (GameObject spawned in _spawnedObjects) {
            Destroy(spawned.gameObject);
        }
    }

    private void OnEnable() {
        foreach (EmissionType type in _emissionDataDict.Keys) {
            EmissionData emission = _emissionDataDict[type];

            if (_emissionCoroutines.ContainsKey(emission.emissionType)) {
                Coroutine toStop = _emissionCoroutines[emission.emissionType];

                if (toStop != null) {
                    StopCoroutine(toStop);
                }

                _emissionCoroutines.Remove(emission.emissionType);
            }

            _emissionCoroutines.Add(type, StartCoroutine(GenerateObjectsCoroutine(emission)));
        }
    }

}
