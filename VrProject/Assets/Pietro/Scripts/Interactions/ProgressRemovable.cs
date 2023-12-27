using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProgressRemovable : RemovablePart {
    [SerializeField] private Bar _progressBar;
    [SerializeField] private ProgressBarManager _barManager;
    [SerializeField] private float _removalStep = 10;
    private float _removalPercentage = 0f;

    protected override void Start() {
        if (_progressBar == null)
            Debug.LogError("No progress bar assigned to " + transform.name);

        if(_barManager != null) {
            _barManager.RegisterProgressBar(_progressBar);
        }

        base.Start();
    }

    public override void Remove() {
        if (_removalPercentage < 100f) {
            _removalPercentage += _removalStep;
            if (_removalPercentage >= 100f) {
                _removalPercentage = 100f;
                base.Remove();
                StartCoroutine(FadeOut());
            }
            _progressBar.SetValue(_removalPercentage);
        }
    }

    public override void RemovalStarted() {
        _progressBar.Show();
        MakeNPCStopMoving();
    }

    public override void RemovalStopped() {
        _progressBar.Hide();
        MakeNPCStartMoving();
    }

}
