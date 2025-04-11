using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    [SerializeField] private float pickupCollisionDuration;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material collidingMaterial;

    private MeshRenderer meshRenderer;
    
    private List<GameObject> collidersInTrigger = new ();
    private float timeCollisionStart = 0;

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update() {
        if (timeCollisionStart != 0) {
            if (Time.time - timeCollisionStart > pickupCollisionDuration) {
                PickUp();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Add(other.gameObject);
        
        if (collidersInTrigger.Count > 0) {
            if (timeCollisionStart == 0) timeCollisionStart = Time.time;
            meshRenderer.material = collidingMaterial;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Remove(other.gameObject);
        
        if (collidersInTrigger.Count == 0) {
            timeCollisionStart = 0;
            meshRenderer.material = defaultMaterial;
        }
    }
    
    protected virtual void PickUp() {
        Debug.Log("Picked up object! New Pickup!!!");
        Destroy(gameObject);
    }
}
