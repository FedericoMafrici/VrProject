using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    private static int _disableInteractionsRequests = 0;
    private static int _disableMovementRequests = 0;
    private static int _disableMenuRequests = 0;

    public static void EnableInteractions() {
        if (_disableInteractionsRequests > 0) {
            _disableInteractionsRequests--;
            Debug.Log("Interactions enabled");
        }
    }

    public static void DisableInteractions() {   
        _disableInteractionsRequests++;
        if (_disableInteractionsRequests == 1) {
            Debug.Log("Interactions disabled");
        }
    }

    public static bool InteractionsAreEnabled() {
        return (_disableInteractionsRequests == 0);
    }

    public static void EnableMovement() {
        if (_disableMovementRequests > 0) {
            _disableMovementRequests--;
            Debug.Log("Movement enabled");
        }
    }

    public static void DisableMovement() {
        _disableMovementRequests++;
        if (_disableMovementRequests == 1) {
            Debug.Log("Movement disabled");
        }
    }

    public static bool MovementIsEnabled() {
        return _disableMovementRequests == 0;
    }

    public static void EnableMenu() {
        if (_disableMenuRequests > 0) {
            _disableMenuRequests--;
            Debug.Log("Menu enabled");
        }
    }

    public static void DisableMenu() {
        _disableMenuRequests++;
        if (_disableMenuRequests == 1) {
            Debug.Log("Menu disabled");
        }
    }

    public static bool MenuIsEnabled() {
        return (_disableMenuRequests == 0);
    }

}
