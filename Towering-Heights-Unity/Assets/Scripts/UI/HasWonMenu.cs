using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasWonMenu : MonoBehaviour {
    
    [SerializeField] private ShapeDropper shapeDropper;
    
    private Controls playerControls;
    
    private void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (playerControls.Player.Pause.triggered) {
            gameObject.SetActive(false);
        }
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
        PauseMenu.GameIsPaused = false;
    }
    
    public void Restart() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PauseMenu.GameIsPaused = false;
        shapeDropper.SetGameOver(false);
    }
    
    public void LevelSelect() {
        SceneManager.LoadScene("LevelSelectMenu");
        PauseMenu.GameIsPaused = false;
    }
}
