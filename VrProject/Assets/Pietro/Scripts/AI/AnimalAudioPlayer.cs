using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAudioPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> _clips;
    [SerializeField] float _minDelay = 10;
    [SerializeField] float _maxDelay = 30;

    AudioSource _audioSource;
    bool _timerWasReset = false;
    Coroutine _audioCoroutine;

    // Start is called before the first frame update
    void Start() {

        if (_clips.Count == 0) {
            Debug.LogWarning(name + ": no  audio clips in clip list");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null ) {
            Debug.LogError(transform.name + ": no audio source found");
        }
        
    }

    public void PlaySound(int index) {
        if (_clips.Count > index) {
            _audioSource.clip = _clips[index];
            _audioSource.Play();
        }
    }

    public void PlaySound(AudioClip clip) {
            _audioSource.clip = clip;
            _audioSource.Play();
    }

    public void PlayAndReset(int index) {
        PlaySound(index);
        if (_audioCoroutine != null) {
            StopCoroutine(_audioCoroutine);
        }

        _audioCoroutine = StartCoroutine(SoundCoroutine());
    }

    public void PlayAndReset(AudioClip clip) {
        PlaySound(clip);
        if (_audioCoroutine != null) {
            StopCoroutine(_audioCoroutine);
        }

        _audioCoroutine = StartCoroutine(SoundCoroutine());
    }

    IEnumerator SoundCoroutine() {
        yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
        PlayAndReset(Random.Range(0, _clips.Count -1));
    }

    private void OnEnable() {
        if (_audioCoroutine == null) {
            _audioCoroutine = StartCoroutine(SoundCoroutine());
        }
    }

    private void OnDisable() {
        if (_audioCoroutine != null ) {
            StopCoroutine(_audioCoroutine);
        }

        _audioCoroutine = null;
    }
}
