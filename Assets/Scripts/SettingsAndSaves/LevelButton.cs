using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {
    [Header("Prefab links")]
    [SerializeField] private Button button;
    [SerializeField] private Image pathImage;
    
    [Header("Settings")]
    [SerializeField] private string levelSceneName;
    [SerializeField] private string unlockedAfterLevel;

    private void Start() {
        button.interactable = false;
        pathImage.color = new Color(0.5f, 0.5f, 0.5f, 1);

        GameData gameData = SaveSystem.LoadGameData();
        if (unlockedAfterLevel == "" || gameData.completedLevels.Contains(unlockedAfterLevel)) {
            button.interactable = true;
            pathImage.color = Color.white;
        }
    }
    
    public void LoadLevel() {
        SceneManager.LoadScene(levelSceneName);
    }
}
