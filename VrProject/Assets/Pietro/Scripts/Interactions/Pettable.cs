using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pettable : MonoBehaviour
{
    private const float _maxFriendship = 100.0f;
    private float _friendship;
    [SerializeField] private float _friendshipGrowthRate;
    [SerializeField] private Bar _progressBar;

    private void Start() {
        if (_progressBar == null)
            Debug.LogError("No progress bar assigned to " + transform.name);

        _friendship = 0;
    }

    public void Pet(Petter petter, float travelledDistance) {
        if (_friendship < _maxFriendship) {
            _friendship += travelledDistance * _friendshipGrowthRate;
            if (_friendship >= _maxFriendship) {
                _friendship = _maxFriendship;
            }
            _progressBar.SetValue(_friendship);
            //Debug.Log("Petted " + transform.name + " travelled distance = " + travelledDistance);
        }

        
    }

    public void HideProgressBar() {
        _progressBar.Hide();
    }

    public void ShowProgressBar() {
        _progressBar.Show();
    }
}
