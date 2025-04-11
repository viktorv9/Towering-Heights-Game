using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public static bool TutorialShown = false;
    
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseMenuFirstSelected;

    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject settingsMenuFirstSelected;

    [SerializeField] private GameObject helpMenuUI;
    [SerializeField] private GameObject helpMenuFirstSelected;
    
    private Controls playerControls;

    private void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();

        if (!TutorialShown) {
            HowToPlay();
            Time.timeScale = 0f;
            GameIsPaused = true;
            TutorialShown = true;
        }
    }

    private void Update() {
        if (playerControls.Player.Pause.triggered) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void DisableAllMenus() {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        helpMenuUI.SetActive(false);
    }
    
    public void Resume() {
        DisableAllMenus();
        Time.timeScale = 1f;
        StartCoroutine(SetGamePausedAfterDelay(false));
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void Pause() {
        DisableAllMenus();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        eventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
    }
    
    public void NavigateSettings() {
        DisableAllMenus();
        settingsMenuUI.SetActive(true);
        
        eventSystem.SetSelectedGameObject(settingsMenuFirstSelected);
    }
    
    public void HowToPlay() {
        DisableAllMenus();
        helpMenuUI.SetActive(true);
        
        eventSystem.SetSelectedGameObject(helpMenuFirstSelected);
    }
    
    public void Quit() {
        Application.Quit();
    }
    
    IEnumerator SetGamePausedAfterDelay(bool newPausedValue) {
        yield return new WaitForSeconds(0.1f);
        GameIsPaused = newPausedValue;
    }
}
