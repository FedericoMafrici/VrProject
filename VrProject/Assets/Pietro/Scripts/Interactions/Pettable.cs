using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pettable : MonoBehaviour {
    private NPCMover _npcMover;
    private const float _maxFriendship = 100.0f;
    private float _friendship;
    private bool _isAtMaxFriendship;
    [SerializeField] private float _friendshipGrowthRate = 20f;
    [SerializeField] private Bar _progressBar;
    //[SerializeField] private GameObject _heartIcon;
    [SerializeField] private AudioSource _maxFriendshipSource;
    [SerializeField] private ObjectEmitter _emitter;
    [SerializeField] private EmissionData _emission;


    public event EventHandler Befriended;
    public static event EventHandler StaticBefriended;

    private void Start() {

        if (_progressBar == null)
            Debug.LogError("No progress bar assigned to " + transform.name);

        NPCMover _tmpMover = transform.GetComponent<NPCMover>();

        if (_tmpMover != null)
            _npcMover= _tmpMover;

        _friendship = 0;
    }

    public void Pet(Petter petter, float travelledDistance) {

        if (_friendship < _maxFriendship) {
            _friendship += travelledDistance * _friendshipGrowthRate;
            if (_friendship >= _maxFriendship) {
                /*
                if (_heartIcon != null) {
                    _heartIcon.SetActive(true);
                }
                */

                if (_maxFriendshipSource != null) {
                    _maxFriendshipSource.gameObject.SetActive(true);
                }

                if (_emitter != null) {
                    _emitter.AddEmission(_emission);
                }

                _friendship = _maxFriendship;
                _isAtMaxFriendship = true;
                
                if (!_progressBar.IsHidden())
                    _progressBar.Hide();

                if (Befriended != null) {
                    Befriended(this, EventArgs.Empty);
                }
                if (StaticBefriended != null) {
                    StaticBefriended(this, EventArgs.Empty);
                }

            }
            _progressBar.SetValue(_friendship);
        }

        
    }

    public void PettingStarted() {
        ShowProgressBar();
        StopMoving();
    }

    public void PettingStopped() {
        HideProgressBar();
        StartMoving();
    }

    
    private void HideProgressBar() {
        _progressBar.Hide();
    }

    private void ShowProgressBar() {
        if (!_isAtMaxFriendship) {
            _progressBar.Show();
        }
    }

    private void StopMoving() {
        if (_npcMover!= null) {
            _npcMover.StopMoving();
        }
    }

    private void StartMoving() {
        if (_npcMover != null) {
            _npcMover.StartMovingDelayed(2f);
        }
    }

    public bool IsAtMaxFriendship() {
        return _isAtMaxFriendship;
    }
}
