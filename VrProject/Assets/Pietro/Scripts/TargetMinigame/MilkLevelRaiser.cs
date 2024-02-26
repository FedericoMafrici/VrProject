using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkLevelRaiser : MinigameCallback
{
    [SerializeField] private float _finalZ;
    [SerializeField] private float _raisingSpeed = 1f;
    private AudioSource _audioSource;
    private Coroutine runningCoroutine = null;
    private Renderer _renderer;
    private float _zIncrement;
    private int _curStep = 0;
    private int _numSteps;
    private float _startZ;
    private float _toReachHeight;
    private bool _started = false;
    private bool _inited = false;

    // Start is called before the first frame update
    void Start() {
        if (!_started) {
            _audioSource = GetComponent<AudioSource>();
            bool isActive = gameObject.activeSelf;

            _renderer = GetComponent<Renderer>();

            if (!isActive) {
                gameObject.SetActive(true);
            }

            _startZ = transform.localPosition.z;
            _started = true;

            if (!isActive) {
                gameObject.SetActive(false);
            }
        }
    }


    public override void Init(TargetMinigame minigame) {
        if (!_started) {
            Start();
        }

        _numSteps = minigame.GetNumTotalTargets(); 
        _zIncrement = Mathf.Abs(_finalZ - _startZ) / (_numSteps-1); //first and last target clicked will not make any changes
        _inited = true;
    }

    public override void ProgressCallback() {

        if (_inited) {
            if (_curStep > 0) { //first step will do nothing

                

                if (_curStep == 1) {

                    if (_renderer != null) {
                        _renderer.enabled = true;
                    }

                }

                if (_curStep < _numSteps) {
                    _toReachHeight = transform.localPosition.z + _zIncrement;
                    if (_audioSource != null) {
                        _audioSource.Play();
                    }
                    if (runningCoroutine == null) {
                        StartCoroutine(RaiseLevel());
                    }
                    //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + _zIncrement);
                }
            }

            _curStep++;
        }
    }

    public override void BeginCallback() {
        if (_inited) {
            if (_renderer != null) {
                _renderer.enabled = false;
            }

            if (runningCoroutine != null) {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }

            _curStep = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, _startZ);
        }
    }

    public override void EndCallback(bool success) {
        if (_inited) {

            if (runningCoroutine != null) {
                StopCoroutine(runningCoroutine);
                runningCoroutine = null;
            }

        }
    }

    IEnumerator RaiseLevel() {
        bool levelReached = false;

        while (!levelReached) {
            float newZ = Mathf.Lerp(transform.localPosition.z, _toReachHeight, _raisingSpeed * Time.deltaTime);
            if (newZ >= _toReachHeight) {
                newZ = _toReachHeight;
            }

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, newZ);
            yield return null;

            if (newZ >= _toReachHeight) {
                levelReached = true;
            }
        }

        runningCoroutine = null;
    }

}
