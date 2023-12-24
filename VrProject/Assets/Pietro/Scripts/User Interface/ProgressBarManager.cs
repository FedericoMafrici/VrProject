using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarManager : MonoBehaviour {
    [SerializeField] private float _verticalSpacing = 5f;
    [SerializeField] private float _offsetFromBottom = 40f;

    private HashSet<Bar> _progressBars = new HashSet<Bar>();

    public void RegisterProgressBar(Bar pBar) {
        pBar.BarShown += OnBarShown;
        pBar.BarHidden += OnBarHidden;
    }

    protected void AddProgressBar(Bar pBar) {
        _progressBars.Add(pBar);
        RepositionProgressBars();
    }

    protected void RemoveProgressBar(Bar pBar) {
        _progressBars.Remove(pBar);
        RepositionProgressBars();
    }

    protected void RepositionProgressBars() {

        float yPos = (-Screen.height/2f) + _offsetFromBottom;

        foreach (Bar pBar in _progressBars) {
            RectTransform rt = pBar.GetBackgroundRectTransform();
            if (rt != null ) {
                rt.anchoredPosition = new Vector2(0f, yPos);
                yPos += (rt.sizeDelta.y/2) + _verticalSpacing;
            }
        }
    }

    private void OnBarShown(object sender, BarEventArgs args) {
        Bar pBar = args.bar;
        AddProgressBar(pBar);
    }

    private void OnBarHidden(object sender, BarEventArgs args) {
        Bar pBar = args.bar;
        if (pBar != null) {
            RemoveProgressBar(pBar);
        }
    }

}
