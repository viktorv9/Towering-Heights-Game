using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    
    [SerializeField] private RectTransform[] uiElements;
    [SerializeField] private HorizontalLayoutGroup[] layoutGroups;
    private float startYOffset = 1000f;
    private float dropDuration = 0.5f;
    private float delay= 0.1f;

    private GameData gameData;

    private void Start() {
        gameData = SaveSystem.LoadGameData();
        
        for (int i = 0; i < uiElements.Length; i++)
        {
            RectTransform uiElement = uiElements[i];
        
            Vector2 targetPos = uiElement.anchoredPosition;
            uiElement.anchoredPosition = targetPos + Vector2.up * startYOffset;
        
            Sequence seq = DOTween.Sequence();
            seq.SetDelay(i * delay);
            seq.Append(
                uiElement.DOAnchorPos(targetPos, dropDuration)
                    .SetEase(Ease.OutBounce)
            );
            seq.Append(
                uiElement.DOShakeAnchorPos(
                    duration: 0.2f,
                    strength: 10f,
                    vibrato: 15,
                    snapping: false,
                    fadeOut: i > 0
                )
            );
        }
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
