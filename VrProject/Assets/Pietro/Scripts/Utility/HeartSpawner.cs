using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public float moveDistance = 2f; // Distance to move left and right
    public float upwardMoveDuration = 1f; // Duration of each left-right movement
    public float lrMoveDuration = 1f; // Duration of each left-right movement
    public float moveHeight = 3f; // Height to move upward

    [SerializeField] private GameObject _toSpawnPrefab;
    // Start is called before the first frame update
    /*
    void Start()
    {
        //StartCoroutine(SpawnHeartCoroutine());
    }*/

    public void SpawnHeart(Vector3 position, Transform toLookAt = null) {
        GameObject spawned = Instantiate(_toSpawnPrefab);

        spawned.transform.position = position;
        

        if (toLookAt != null ) {

            Vector3 pointToLookAt = new Vector3(toLookAt.position.x, spawned.transform.position.y, toLookAt.position.z);
            spawned.transform.LookAt(pointToLookAt);
            
        }

        // Initial position
        Vector3 startPos = spawned.transform.localPosition;

        // Create a sequence of movements for left and right motion
        DG.Tweening.Sequence lrSequence = DOTween.Sequence();
        lrSequence.Append(spawned.transform.DOLocalMoveX(startPos.x + moveDistance, lrMoveDuration).SetEase(Ease.OutQuad));
        lrSequence.Append(spawned.transform.DOLocalMoveX(startPos.x, lrMoveDuration).SetEase(Ease.Linear));
        lrSequence.Append(spawned.transform.DOLocalMoveX(startPos.x - moveDistance, lrMoveDuration).SetEase(Ease.OutQuad));
        lrSequence.Append(spawned.transform.DOLocalMoveX(startPos.x, lrMoveDuration).SetEase(Ease.Linear));
        lrSequence.SetLoops(-1);

        // Create a sequence of movements for upward motion
        DG.Tweening.Sequence upwardSequence = DOTween.Sequence();
        upwardSequence.Append(spawned.transform.DOMoveY(startPos.y + moveHeight, upwardMoveDuration).SetEase(Ease.InOutQuad));
        //upwardSequence.Append(spawned.transform.DOMoveY(startPos.y, moveDuration).SetEase(Ease.InOutQuad));
        upwardSequence.SetLoops(-1);

        // Start both sequences
        lrSequence.Play();
        upwardSequence.Play();
        StartCoroutine(DestroyCoroutine(spawned, upwardMoveDuration, lrSequence, upwardSequence));
    }

    IEnumerator SpawnHeartCoroutine(Vector3 position) {
        while (true) {
            SpawnHeart(position);
            yield return new WaitForSeconds(1);

        }
    }

    IEnumerator DestroyCoroutine(GameObject toDestroy, float toWait, DG.Tweening.Sequence lrSequence, DG.Tweening.Sequence upwardSequence) {
        yield return new WaitForSeconds(toWait);
        //StartCoroutine(FadeOut(toDestroy, lrSequence, upwardSequence));
        lrSequence.Kill();
        upwardSequence.Kill();
        Destroy(toDestroy.gameObject);
    }

    protected IEnumerator FadeOut(GameObject toFadeOut, DG.Tweening.Sequence lrSequence, DG.Tweening.Sequence upwardSequence) {
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

        lrSequence.Kill();
        upwardSequence.Kill();
        Destroy(toFadeOut.gameObject);
    }
}
