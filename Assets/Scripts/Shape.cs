using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Shape : MonoBehaviour {

    [SerializeField] private EventReference collisionSound;

    private bool playedSoundThisFrame;
    
    private void Update() {
        playedSoundThisFrame = false;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.relativeVelocity.magnitude > 0.1 && !playedSoundThisFrame) {
            AudioManager.instance.PlayBlockCollision(collisionSound, transform.position, collision.relativeVelocity.magnitude);
            playedSoundThisFrame = true;
        }
    }
}
