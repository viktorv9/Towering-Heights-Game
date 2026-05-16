using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Shape : MonoBehaviour {

    [SerializeField] private EventReference collisionSound;
    [SerializeField] private float collisionSoundCooldown;

    private float timeLastCollisionSound;

    private void OnCollisionEnter(Collision collision) {
        // if two blocks collide, make only 1 play a sound effect
        if (collision.gameObject.CompareTag("Block")) {
            if (collision.gameObject.GetInstanceID() < gameObject.GetInstanceID()) return;
        }
        
        if (collision.relativeVelocity.magnitude > 0.5 && Time.time - timeLastCollisionSound > collisionSoundCooldown) {
            AudioManager.instance.PlayBlockCollision(collisionSound, transform.position, collision.relativeVelocity.magnitude);
            timeLastCollisionSound = Time.time;
        }
    }
}
