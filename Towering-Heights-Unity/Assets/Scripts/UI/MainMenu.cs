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
#if DEMO
        SceneManager.LoadScene("EndlessMode");
#else
        if (gameData.tutorialCompleted) {
            SceneManager.LoadScene("LevelSelectMenu");
        } else {
            SceneManager.LoadScene("Tutorial");
        }
#endif
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
    
    public void UnlockAllLevels() {
        gameData.completedLevels = new List<string>() {
            "Tutorial",
            "EarthLevel",
            "WaterLevel",
            "FireLevel",
            "AirLevel",
            "SpaceLevel",
        };
        SaveSystem.SaveGameData(gameData);
    }
    
    public void UnlockAllUpgrades() {
        gameData.rotationUnlocked = true;
        gameData.holdBlockUnlocked = true;
        gameData.undoBlockUnlocked = true;
        SaveSystem.SaveGameData(gameData);
    }
}
