using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class Shape : MonoBehaviour {

    [SerializeField] private EventReference collisionSound;

    private bool enteredCollisionThisFrame;
    
    private void Update() {
        if (enteredCollisionThisFrame) {
            AudioManager.instance.PlayOneShot(collisionSound, transform.position);
        }

        enteredCollisionThisFrame = false;
    }

    private void OnCollisionEnter(Collision collision) {
        enteredCollisionThisFrame = true;
    }
}
