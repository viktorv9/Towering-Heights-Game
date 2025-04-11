using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {
    
    [SerializeField] private PickupType pickupType;
    [SerializeField] private float pickupCollisionDuration;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material collidingMaterial;

    private MeshRenderer meshRenderer;
    
    private List<GameObject> collidersInTrigger = new ();
    private float timeCollisionStart = 0;
    
    public enum PickupType {
        Default,
        Platform
    }

    public delegate void PickupAction(PickupType pickupType);
    public static event PickupAction OnPickup;
    
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
    
    private void PickUp() {
        OnPickup?.Invoke(pickupType);
        Destroy(gameObject);
    }
}
