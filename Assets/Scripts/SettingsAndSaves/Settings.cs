using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Toggle horizontalInputInvertedToggle;
    [SerializeField] private Toggle verticalInputInvertedToggle;
    
    public class GameSettings {
        public float musicVolume;
        public float sfxVolume;
        public float cameraSensitivity;
        public bool invertCameraX;
        public bool invertCameraY;
    }

    private GameSettings gameSettings = new();
    
    public delegate void UpdateSettingsAction(GameSettings gameSettings);
    public static event UpdateSettingsAction OnUpdateSettings;

    private void Start() {
        sfxVolumeSlider.onValueChanged.AddListener(delegate {
            gameSettings.sfxVolume = sfxVolumeSlider.value;
            PlayerPrefs.SetFloat("SfxVolume", gameSettings.sfxVolume);
            OnUpdateSettings.Invoke(gameSettings);
        });
        musicVolumeSlider.onValueChanged.AddListener(delegate {
            gameSettings.musicVolume = musicVolumeSlider.value;
            PlayerPrefs.SetFloat("MusicVolume", gameSettings.musicVolume);
            OnUpdateSettings.Invoke(gameSettings);
        });
        
        cameraSensitivitySlider.onValueChanged.AddListener(delegate {
            gameSettings.cameraSensitivity = cameraSensitivitySlider.value;
            PlayerPrefs.SetFloat("CameraSensitivity", gameSettings.cameraSensitivity);
            OnUpdateSettings.Invoke(gameSettings);
        });
        horizontalInputInvertedToggle.onValueChanged.AddListener(delegate {
            gameSettings.invertCameraX = horizontalInputInvertedToggle.isOn;
            PlayerPrefs.SetInt("HorizontalInputInverted", gameSettings.invertCameraX ? 1 : 0);
            OnUpdateSettings.Invoke(gameSettings);
        });
        verticalInputInvertedToggle.onValueChanged.AddListener(delegate {
            gameSettings.invertCameraY = verticalInputInvertedToggle.isOn;
            PlayerPrefs.SetInt("VerticalInputInverted", gameSettings.invertCameraY ? 1 : 0);
            OnUpdateSettings.Invoke(gameSettings);
        });
    }
    
    private void OnEnable() {
        // temp: dit zorgt ervoor dat alle gameobjects eerst laden en dat DAN de load settings wordt gedaan
        // maar beter zou zijn om dit te handelen in een soort WaitForLoad logica
        StartCoroutine(LoadAfterDelay());
    }
    
    private IEnumerator LoadAfterDelay() {
        yield return new WaitForSecondsRealtime(0.1f);
        LoadPlayerSettings();
    }

    private void LoadPlayerSettings()
    {
        gameSettings.musicVolume = PlayerPrefs.GetFloat("SfxVolume", sfxVolumeSlider.value);
        sfxVolumeSlider.value = gameSettings.musicVolume;
        
        gameSettings.sfxVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolumeSlider.value);
        musicVolumeSlider.value = gameSettings.sfxVolume;
        
        gameSettings.cameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", cameraSensitivitySlider.value);
        cameraSensitivitySlider.value = gameSettings.cameraSensitivity;
        
        gameSettings.invertCameraX = Convert.ToBoolean(
            PlayerPrefs.GetInt("HorizontalInputInverted", horizontalInputInvertedToggle.isOn ? 1 : 0));
        horizontalInputInvertedToggle.isOn = gameSettings.invertCameraX;
        
        gameSettings.invertCameraY = Convert.ToBoolean(
            PlayerPrefs.GetInt("VerticalInputInverted", verticalInputInvertedToggle.isOn ? 1 : 0));
        verticalInputInvertedToggle.isOn = gameSettings.invertCameraY;
        
        OnUpdateSettings.Invoke(gameSettings);
    }
}
