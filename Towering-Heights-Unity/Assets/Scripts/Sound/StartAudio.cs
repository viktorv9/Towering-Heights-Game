using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class StartAudio : MonoBehaviour
{
    [SerializeField] private EventReference startSound;
    
    void Start()
    {
        StartCoroutine(PlayStartSound());
    }
    
    IEnumerator PlayStartSound()
    {
        yield return new WaitForSeconds(0.2f);
        var soundInstance = RuntimeManager.CreateInstance(startSound);
        soundInstance.start();
        RuntimeManager.AttachInstanceToGameObject(soundInstance, GameObject.FindWithTag("MainCamera"));
        soundInstance.release();
        yield return null;
    }
}
