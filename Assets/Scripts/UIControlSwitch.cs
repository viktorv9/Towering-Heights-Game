using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControlSwitch : MonoBehaviour {
    
    [SerializeField] private ControlsTracker controlsTracker;

    [SerializeField] private List<GameObject> mouseKeyboardOnlyUI;
    [SerializeField] private List<GameObject> controllerOnlyUI;
    
    private bool isMouseKeyboard = true;

    void Update() {
        if (isMouseKeyboard != controlsTracker.isMouseKeyboard) {
            isMouseKeyboard = controlsTracker.isMouseKeyboard;

            SetAllGameObjectsActive(mouseKeyboardOnlyUI, isMouseKeyboard);
            SetAllGameObjectsActive(controllerOnlyUI, !isMouseKeyboard);
        }
    }
    
    private void SetAllGameObjectsActive(List<GameObject> gameObjects, bool newValue) {
        gameObjects.ForEach(listGameObject => {
            listGameObject.SetActive(newValue);
        });
    }
}
