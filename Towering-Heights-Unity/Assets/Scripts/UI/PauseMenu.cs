using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    
    [SerializeField] private EventSystem eventSystem;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject pauseMenuFirstSelected;

    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject settingsMenuFirstSelected;

    [SerializeField] private GameObject controlsMenuUI;
    [SerializeField] private GameObject controlsMenuFirstSelected;

    private GameObject previousMenuUI;
    private GameObject previousMenuFirstSelected;
    
    private Controls playerControls;

    private void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        controlsMenuUI.SetActive(false);
    }
    
    public void Resume() {
        DisableAllMenus();
        Time.timeScale = 1f;
        StartCoroutine(SetGamePausedStateAfterDelay(false));
    }
    
    public void Pause() {
        DisableAllMenus();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        
        eventSystem.SetSelectedGameObject(pauseMenuFirstSelected);
    }
    
    public void NavigateControls() {
        DisableAllMenus();
        controlsMenuUI.SetActive(true);
        eventSystem.SetSelectedGameObject(controlsMenuFirstSelected);

        previousMenuUI = pauseMenuUI;
        previousMenuFirstSelected = pauseMenuFirstSelected;
    }
    
    public void NavigateSettings() {
        DisableAllMenus();
        settingsMenuUI.SetActive(true);
        eventSystem.SetSelectedGameObject(settingsMenuFirstSelected);

        previousMenuUI = pauseMenuUI;
        previousMenuFirstSelected = pauseMenuFirstSelected;
    }
    
    public void NavigateBack() {
        if (previousMenuUI == null) Resume();
        else {
            DisableAllMenus();
            previousMenuUI.SetActive(true);
            eventSystem.SetSelectedGameObject(previousMenuFirstSelected);

            previousMenuUI = null;
            previousMenuFirstSelected = null;
        }
    }
    
    public void NavigateLevels() {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("LevelSelectMenu");
    }
    
    public void Quit() {
        Application.Quit();
    }
    
    IEnumerator SetGamePausedStateAfterDelay(bool newPausedValue) {
        yield return new WaitForSeconds(0.1f);
        GameIsPaused = newPausedValue;
    }
}
