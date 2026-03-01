using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject settingsMenuUI;

    private GameData gameData;

    private void Start() {
        gameData = SaveSystem.LoadGameData();
    }

    
    public void Play() {
        if (gameData.tutorialCompleted) {
            SceneManager.LoadScene("LevelSelectMenu");
        } else {
            SceneManager.LoadScene("Tutorial");
        }
    }
    
    public void OpenSettings() {
        mainMenuUI.SetActive(false);
        settingsMenuUI.SetActive(true);
    }
    
    public void CloseSettings() {
        settingsMenuUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }
}
