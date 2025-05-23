using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class InputHandler : MonoBehaviour {

    public enum InputMode { Controller, Keyboard, Touch };
    public static Action<InputMode> OnInputModeChanged;
    public static InputMode CurrentInputMode;
    
    private InputMode inputModeLastFrame;
    
    private void Start() {
        CurrentInputMode = InputMode.Keyboard;
    }
    
    private void Update() {
        CurrentInputMode = SetInputMode();
        
        if (CurrentInputMode != inputModeLastFrame) {
            OnInputModeChanged?.Invoke(CurrentInputMode);
        }
        
        inputModeLastFrame = SetInputMode();
    }
    
    private InputMode SetInputMode() {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            return InputMode.Touch;
        }
        
        if (Input.GetJoystickNames().Length == 0) {
            return InputMode.Keyboard;
        }
        
        if (Input.anyKeyDown) {
            if (Input.GetKeyDown(KeyCode.JoystickButton0)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton1)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton2)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton3)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton4)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton5)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton6)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton7)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton8)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton9)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton10)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton11)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton12)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton13)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton14)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton15)) return InputMode.Controller;
            if (Input.GetKeyDown(KeyCode.JoystickButton16)) return InputMode.Controller;
            return InputMode.Keyboard;
        }

        return CurrentInputMode;
    }
}
