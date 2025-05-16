using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {
    
    // TODO: This script saves data to JSON, but the SaveSystem/SaveData classes write to binary.
    // TODO: Convert or delete this code and work with SaveSystem!

    [SerializeField] private List<Button> uiButtons;
    
    public class PlayerSaveData {
        public int playerLevel;
        public int unlockedLevels;
    }

    public static int playerLevel;
    public static int unlockedLevels;

    private void Start() {
        LoadGame();
        
        int i = 0;
        foreach (Button uiButton in uiButtons) {
            uiButton.interactable = i <= unlockedLevels;
            i++;
        }
        
        playerLevel++;
        unlockedLevels++;
        SaveGame();
    }

    private static void SaveGame() {
        PlayerSaveData playerData = new PlayerSaveData();
        playerData.playerLevel = playerLevel;
        playerData.unlockedLevels = unlockedLevels;

        string json = JsonUtility.ToJson(playerData);
        string path = Application.persistentDataPath + "/PlayerSaveData.json";
        File.WriteAllText(path, json);
    }

    private static void LoadGame() {
        string path = Application.persistentDataPath + "/PlayerSaveData.json";
        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            PlayerSaveData loadedData = JsonUtility.FromJson<PlayerSaveData>(json);

            playerLevel = loadedData.playerLevel;
            unlockedLevels = loadedData.unlockedLevels;
        } else {
            playerLevel = 0;
            unlockedLevels = 0;
        }
    }

    public void LoadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
