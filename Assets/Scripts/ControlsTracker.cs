using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class ControlsTracker : MonoBehaviour {

    public bool isMouseKeyboard = false;
    
    private void Awake() {
        InputSystem.onEvent += TrackInputDevice;
    }
    
    private void OnDestroy() {
        InputSystem.onEvent -= TrackInputDevice;
    }

    private void TrackInputDevice(InputEventPtr eventPtr, InputDevice device) {
        if (device.name == "Mouse" || device.name == "Keyboard") {
            isMouseKeyboard = true;
        } else {
            isMouseKeyboard = false;
        }
        Debug.Log("Is mouse and keyboard? " + isMouseKeyboard);
    }
}
