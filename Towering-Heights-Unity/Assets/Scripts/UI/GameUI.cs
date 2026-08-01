using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject scoreDisplay;
    [SerializeField] private TMP_Text scoreTextArea;
    
    [SerializeField] private ShapeDropper shapeDropper;
    [SerializeField] private GameObject controlsMenuUI;
    
    private Controls playerControls;
    
    void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (playerControls.Player.Help.triggered) {
            if (!PauseMenu.GameIsPaused || controlsMenuUI.activeSelf) ToggleShowControls();
        }
    }

    public void SetShowScore(bool newValue) {
        scoreDisplay.SetActive(newValue);
    }

    public void UpdateScore(int newScore) {
        string newText = "score :\n" + newScore + " / 50";
        if (newScore >= 50) newText += "\n\nadvanced\nshapes\nadded!";
        scoreTextArea.SetText(newText);
        if (newScore == 50) scoreTextArea.color = Color.yellow;
    }
    
    private void ToggleShowControls() {
        controlsMenuUI.SetActive(!controlsMenuUI.activeSelf);
        PauseMenu.GameIsPaused = controlsMenuUI.activeSelf;
        Time.timeScale = controlsMenuUI.activeSelf ? 0f : 1f;
    }
}
