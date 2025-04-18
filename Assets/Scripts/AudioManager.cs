using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    
    [SerializeField] private EventReference sceneMusic;

    private List<EventInstance> eventInstances = new();
    private EventInstance musicEventInstance;

    private Bus sfxBus;
    private Bus musicBus;

    private void Awake() {
        if (instance != null) {
            Debug.LogError("More than one AudioManager in the scene!");
        }
        instance = this;

        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        musicBus.getVolume(out float volume);
        
        LoadPlayerSettings();
        InitializeMusic(sceneMusic);
        
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
        sfxBus.setVolume(newSfxVolume);
        PlayerPrefs.SetFloat("SfxVolume", newSfxVolume);
    }

    private void SetMusicVolume(float newMusicVolume) {
        musicBus.setVolume(newMusicVolume);
        PlayerPrefs.SetFloat("MusicVolume", newMusicVolume);
    }
    
    private void InitializeMusic(EventReference musicReference) {
        musicEventInstance = CreateInstance(musicReference);
        RuntimeManager.AttachInstanceToGameObject(musicEventInstance, GameObject.FindWithTag("MainCamera"));
        musicEventInstance.start();
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
    
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }
    
    private void OnDestroy()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }
}
