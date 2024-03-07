using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {
    private static int _disableInteractionsRequests = 0;
    private static int _disableMovementRequests = 0;
    private static int _disableMenuRequests = 0;
    private static int _disableDialoguesRequests = 0;
    private static int _disableJumpRequests = 1;

    public static void EnableInteractions() {
        if (_disableInteractionsRequests > 0) {
            _disableInteractionsRequests--;
        }
    }

    public static void DisableInteractions() {   
        _disableInteractionsRequests++;
        if (_disableInteractionsRequests == 1) {
        }
    }

    public static bool InteractionsAreEnabled() {
        return (_disableInteractionsRequests == 0);
    }

    public static void EnableMovement() {
        if (_disableMovementRequests > 0) {
            _disableMovementRequests--;
        }
    }

    public static void DisableMovement() {
        _disableMovementRequests++;
        if (_disableMovementRequests == 1) {
        }
    }

    public static bool MovementIsEnabled() {
        return _disableMovementRequests == 0;
    }

    public static void EnableMenu() {
        if (_disableMenuRequests > 0) {
            _disableMenuRequests--;
        }
    }

    public static void DisableMenu() {
        _disableMenuRequests++;
        if (_disableMenuRequests == 1) {

        }
    }

    public static bool MenuIsEnabled() {
        return (_disableMenuRequests == 0);
    }

    public static void EnableDialogues() {
        if (_disableDialoguesRequests > 0) {
            _disableDialoguesRequests--;
        }
    }

    public static void DisableDialogues() {
        _disableDialoguesRequests++;
        if (_disableDialoguesRequests == 1) {
        }
    }

    public static bool DialoguesAreEnabled() {
        return (_disableDialoguesRequests == 0);
    }

    public static void EnableJump() {
        if (_disableJumpRequests > 0) {
            _disableJumpRequests--;
        }
    }

    public static void DisableJump() {
        _disableJumpRequests++;
        if (_disableJumpRequests == 1) {
        }
    }

    public static bool JumpIsEnabled() {
        return (_disableJumpRequests == 0);
    }

}
