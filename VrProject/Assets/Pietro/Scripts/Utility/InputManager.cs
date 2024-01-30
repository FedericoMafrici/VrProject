using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    private static bool _inputsEnabled = true;

    public static void EnableInputs() {
        Debug.Log("Inputs enabled");
        _inputsEnabled = true;
    }

    public static void DisableInteractions() {
        Debug.Log("Inputs disabled");
        _inputsEnabled = false; 
    }

    public static bool InteractionsAreEnabled() {
        return _inputsEnabled;
    }

}
