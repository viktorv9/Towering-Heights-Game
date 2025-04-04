using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlSwitch : MonoBehaviour {
    
    [SerializeField] private List<GameObject> mouseKeyboardOnlyUI;
    [SerializeField] private List<GameObject> controllerOnlyUI;
    
    private void OnEnable() {
        InputHandler.OnInputModeChanged += SetAllGameObjectsActive;
    }

    private void OnDisable() {
        InputHandler.OnInputModeChanged -= SetAllGameObjectsActive;
    }
    
    private void SetAllGameObjectsActive(InputHandler.InputMode inputMode) {
        mouseKeyboardOnlyUI.ForEach(listGameObject => {
            listGameObject.SetActive(inputMode == InputHandler.InputMode.Keyboard);
        });
        controllerOnlyUI.ForEach(listGameObject => {
            listGameObject.SetActive(inputMode == InputHandler.InputMode.Controller);
        });
    }
}
