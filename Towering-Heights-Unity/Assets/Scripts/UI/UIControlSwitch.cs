using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlSwitch : MonoBehaviour {
    
    [SerializeField] private List<GameObject> mouseKeyboardOnlyUI;
    [SerializeField] private List<GameObject> controllerOnlyUI;
    
    private void OnEnable() {
        InputHandler.OnInputModeChanged += SetAllGameObjectsActive;
        InputHandler.OnInputModeChanged += SetMouseLockedState;
        SetMouseLockedState(InputHandler.CurrentInputMode);
    }

    private void OnDisable() {
        InputHandler.OnInputModeChanged -= SetAllGameObjectsActive;
        InputHandler.OnInputModeChanged -= SetMouseLockedState;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void SetAllGameObjectsActive(InputHandler.InputMode inputMode) {
        mouseKeyboardOnlyUI.ForEach(listGameObject => {
            listGameObject.SetActive(inputMode == InputHandler.InputMode.Keyboard);
        });
        controllerOnlyUI.ForEach(listGameObject => {
            listGameObject.SetActive(inputMode == InputHandler.InputMode.Controller);
        });
    }
    
    private void SetMouseLockedState(InputHandler.InputMode inputMode) {
        Cursor.lockState = inputMode == InputHandler.InputMode.Keyboard ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = inputMode == InputHandler.InputMode.Keyboard;
    }
}
