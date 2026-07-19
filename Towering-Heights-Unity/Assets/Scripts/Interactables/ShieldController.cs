using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private EventReference shieldSound;

    private Controls playerControls;
    private EventInstance soundEventInstance;
    
    void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        
        if (playerControls.Player.Shield.IsPressed() && !shieldObject.activeSelf) {
            shieldObject.SetActive(true);
            soundEventInstance = AudioManager.instance.CreateInstance(shieldSound);
            RuntimeManager.AttachInstanceToGameObject(soundEventInstance, GameObject.FindWithTag("MainCamera"));
            soundEventInstance.start();
        }
        if (!playerControls.Player.Shield.IsPressed() && shieldObject.activeSelf) {
            shieldObject.SetActive(false);
            soundEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
