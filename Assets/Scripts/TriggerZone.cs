using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour {
    
    public UnityEvent<Collider> onTriggerEnterEvent;
    public UnityEvent<Collider> onTriggerExitEvent;
    
    public void OnTriggerEnter(Collider other) {
        onTriggerEnterEvent.Invoke(other);
    }

    public void OnTriggerExit(Collider other) {
        onTriggerExitEvent.Invoke(other);
    }
}
