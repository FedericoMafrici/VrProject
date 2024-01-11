using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    private static bool _inputsEnabled = true;

    public static void EnableInputs() {
        Debug.Log("Inputs enbaled");
        _inputsEnabled = true;
    }

    public static void DisableInputs() {
        Debug.Log("Inputs disabled");
        _inputsEnabled = false; 
    }

    public static bool InputsAreEnabled() {
        return _inputsEnabled;
    }

}
