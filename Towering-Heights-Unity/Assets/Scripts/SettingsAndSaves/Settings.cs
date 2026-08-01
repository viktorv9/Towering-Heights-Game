using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour {
    
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Toggle horizontalInputInvertedToggle;
    [SerializeField] private Toggle verticalInputInvertedToggle;
    
    public class GameSettings {
        public float musicVolume;
        public float sfxVolume;
        public bool fullscreen;
        public float cameraSensitivity;
        public bool invertCameraX;
        public bool invertCameraY;
    }

    private GameSettings gameSettings = new();

    private bool depenentGameObjectsLoaded;
    
    public delegate void UpdateSettingsAction(GameSettings gameSettings);
    public static event UpdateSettingsAction OnUpdateSettings;
    
    private void OnEnable() {
        SceneManager.sceneLoaded += Setup;
        OnUpdateSettings += SetFullscreen;
    }
    
    private void OnDisable() {
        SceneManager.sceneLoaded -= Setup;
        OnUpdateSettings -= SetFullscreen;
    }
    
    private void SetFullscreen(GameSettings newGameSettings) {
        Debug.Log("SetFullscreen: " + newGameSettings.fullscreen);
        if (newGameSettings.fullscreen) {
            Resolution current = Screen.currentResolution;

            Screen.SetResolution(
                current.width,
                current.height,
                FullScreenMode.FullScreenWindow
            );
        } else {
            Screen.SetResolution(
                1280,
                720,
                FullScreenMode.Windowed
            );
        }
    }
    
    private void Setup(Scene scene, LoadSceneMode mode) {
        LoadPlayerSettings();
        AssignEvents();

        OnUpdateSettings?.Invoke(gameSettings);
    }

    private void AssignEvents() {
        sfxVolumeSlider.onValueChanged.AddListener(delegate {
            gameSettings.sfxVolume = sfxVolumeSlider.value;
            PlayerPrefs.SetFloat("SfxVolume", gameSettings.sfxVolume);
            OnUpdateSettings?.Invoke(gameSettings);
        });
        musicVolumeSlider.onValueChanged.AddListener(delegate {
            gameSettings.musicVolume = musicVolumeSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", gameSettings.musicVolume);
            OnUpdateSettings?.Invoke(gameSettings);
        });
        
        fullscreenToggle.onValueChanged.AddListener(delegate {
            gameSettings.fullscreen = fullscreenToggle.isOn;
            PlayerPrefs.SetInt("Fullscreen", gameSettings.fullscreen ? 1 : 0);
            OnUpdateSettings?.Invoke(gameSettings);
        });
        
        cameraSensitivitySlider.onValueChanged.AddListener(delegate {
            gameSettings.cameraSensitivity = cameraSensitivitySlider.value;
            PlayerPrefs.SetFloat("CameraSensitivity", gameSettings.cameraSensitivity);
            OnUpdateSettings?.Invoke(gameSettings);
        });
        horizontalInputInvertedToggle.onValueChanged.AddListener(delegate {
            gameSettings.invertCameraX = horizontalInputInvertedToggle.isOn;
            PlayerPrefs.SetInt("HorizontalInputInverted", gameSettings.invertCameraX ? 1 : 0);
            OnUpdateSettings?.Invoke(gameSettings);
        });
        verticalInputInvertedToggle.onValueChanged.AddListener(delegate {
            gameSettings.invertCameraY = verticalInputInvertedToggle.isOn;
            PlayerPrefs.SetInt("VerticalInputInverted", gameSettings.invertCameraY ? 1 : 0);
            OnUpdateSettings?.Invoke(gameSettings);
        });
    }

    private void LoadPlayerSettings()
    {
        gameSettings.musicVolume = PlayerPrefs.GetFloat("MusicVolume", sfxVolumeSlider.value);
        musicVolumeSlider.value = gameSettings.musicVolume;
        gameSettings.sfxVolume = PlayerPrefs.GetFloat("SfxVolume", musicVolumeSlider.value);
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        
        Debug.Log("PlayerPrefs.GetInt(\"Fullscreen\") " + PlayerPrefs.GetInt("Fullscreen"));
        
        gameSettings.fullscreen = Convert.ToBoolean(
            PlayerPrefs.GetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0));
        fullscreenToggle.isOn = gameSettings.fullscreen;
        
        gameSettings.cameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", cameraSensitivitySlider.value);
        cameraSensitivitySlider.value = gameSettings.cameraSensitivity;
        gameSettings.invertCameraX = Convert.ToBoolean(
            PlayerPrefs.GetInt("HorizontalInputInverted", horizontalInputInvertedToggle.isOn ? 1 : 0));
        horizontalInputInvertedToggle.isOn = gameSettings.invertCameraX;
        gameSettings.invertCameraY = Convert.ToBoolean(
            PlayerPrefs.GetInt("VerticalInputInverted", verticalInputInvertedToggle.isOn ? 1 : 0));
        verticalInputInvertedToggle.isOn = gameSettings.invertCameraY;
    }
}
