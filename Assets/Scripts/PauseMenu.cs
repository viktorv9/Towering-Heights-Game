using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public static bool TutorialShown = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject helpMenuUI;

    private void Start() {
        if (!TutorialShown) {
            HowToPlay();
            Time.timeScale = 0f;
            GameIsPaused = true;
            TutorialShown = true;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }
    
    public void Resume() {
        pauseMenuUI.SetActive(false);
        helpMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void HowToPlay() {
        pauseMenuUI.SetActive(false);
        helpMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void Quit() {
        Application.Quit();
    }
}
