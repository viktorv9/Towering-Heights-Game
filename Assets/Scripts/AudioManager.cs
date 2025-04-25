using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

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
        
        InitializeMusic(sceneMusic);
    }
    
    private void OnEnable() {
        Settings.OnUpdateSettings += SetVolumeSettings;
    }

    private void OnDisable() {
        Settings.OnUpdateSettings -= SetVolumeSettings;
    }

    private void SetVolumeSettings(Settings.GameSettings gameSettings) {
        sfxBus.setVolume(gameSettings.sfxVolume);
        musicBus.setVolume(gameSettings.musicVolume);
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
