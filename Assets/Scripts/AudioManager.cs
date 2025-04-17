using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private FMOD.Studio.Bus sfx;
    private FMOD.Studio.Bus music;

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;

    public static float sfxVolume { get; private set; }
    public static float musicVolume { get; private set; }

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager in the scene!");
        }
        instance = this;
        
        // todo: hier het opzetten van de FMOD sound banks
        
        LoadPlayerSettings();
        
        sfxVolumeSlider.onValueChanged.AddListener(delegate {
            SetSfxVolume(sfxVolumeSlider.value);
        });
        musicVolumeSlider.onValueChanged.AddListener(delegate {
            SetMusicVolume(musicVolumeSlider.value);
        });
    }

    private void LoadPlayerSettings() {
        float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", sfxVolumeSlider.value);
        SetSfxVolume(savedSfxVolume);
        sfxVolumeSlider.value = savedSfxVolume;
        
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolumeSlider.value);
        SetMusicVolume(savedMusicVolume);
        musicVolumeSlider.value = savedMusicVolume;
    }

    private void SetSfxVolume(float newSfxVolume) {
        // todo: hier de volume van de sound bank aanpassen
        PlayerPrefs.SetFloat("SfxVolume", newSfxVolume);
    }

    private void SetMusicVolume(float newMusicVolume) {
        // todo: hier de volume van de sound bank aanpassen
        PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
    }
    
    public void PlayOneShot(EventReference sound, Vector3 worldPos) {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
    
    public void PlayBlockCollision(EventReference sound, Vector3 worldPos, float velocity) {
        var soundInstance = RuntimeManager.CreateInstance(sound);
        soundInstance.set3DAttributes(worldPos.To3DAttributes());
        soundInstance.setParameterByName("velocity", velocity);
        soundInstance.start();
        soundInstance.release();
    }
}
