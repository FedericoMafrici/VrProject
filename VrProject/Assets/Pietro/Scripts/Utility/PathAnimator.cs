using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimator : MonoBehaviour
{
    List<PathArrow> _children = new List<PathArrow>();
    void Start()
    {
        FindChildren();
        DoMotion();
    }

    private void FindChildren() {
        _children.Clear();

        PathArrow[] _childrenArray = GetComponentsInChildren<PathArrow>();
        for ( int i = 0; i < _childrenArray.Length; i++) {
            PathArrow child = _childrenArray[i];

            if (i < _childrenArray.Length - 1) {
                child.SetNext(_childrenArray[i + 1]);
            } else {
                child.SetNext(null);
            }

            _children.Add(child);
            
        }

    }

    private void DoMotion(bool overridePrevious = false) {
        if (_children.Count > 0) {
            _children[0].DoMotion(overridePrevious);
        }
    }

    private void StopMotion() {
        if (_children.Count > 0) {
            _children[0].StopMotion();
        }
    }

    private void OnEnable() {
        FindChildren();
        DoMotion(true);
    }

    private void OnDisable() {
        StopMotion();
    }
}
