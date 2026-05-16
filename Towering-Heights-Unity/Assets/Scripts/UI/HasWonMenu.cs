using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HasWonMenu : MonoBehaviour {
    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void Restart() {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void LevelSelect() {
        SceneManager.LoadScene("LevelSelectMenu");
    }
}
