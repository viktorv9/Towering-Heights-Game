using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private GameObject shieldCollisionEffect;
    [SerializeField] private GameObject trailEffectBurst;

    [SerializeField] private EventReference shieldCollisionSound;
    [SerializeField] private EventReference blockCollisionSound;
    
    private Vector3 previousPosition;
    private bool hasHit;
    
    public void Initialize(Vector3 direction)
    {
        transform.forward = -direction; // - because model is backwards
        rb.velocity = -transform.forward * speed; // - because model is backwards
    }

    private void Update() {
        previousPosition = transform.position;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (hasHit) return;
        Vector3 hitPoint = other.ClosestPoint(previousPosition);

        if (other.transform.CompareTag("Shield")) {
            hasHit = true;
            Instantiate(shieldCollisionEffect, hitPoint, Quaternion.identity);
            var soundInstance = AudioManager.instance.CreateInstance(shieldCollisionSound);
            soundInstance.set3DAttributes(transform.position.To3DAttributes());
            soundInstance.start();
            soundInstance.release();
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Block")) {
            if (other.transform.name != "rock_platform") {
                ApplyExplosionForce(hitPoint);
            }
            
            var soundInstance = AudioManager.instance.CreateInstance(blockCollisionSound);
            soundInstance.set3DAttributes(transform.position.To3DAttributes());
            soundInstance.start();
            soundInstance.release();
            
            hasHit = true;
            Instantiate(collisionEffect, hitPoint, Quaternion.identity);
            
            Destroy(gameObject);
        }
    }
    
    public void ApplyExplosionForce(Vector3 hitPoint)
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = new Collider[50];
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        Physics.OverlapSphereNonAlloc(explosionPosition, 1f, colliders);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.attachedRigidbody != null)
            {
                rigidbodies.Add(collider.attachedRigidbody);
            }
        }

        foreach (Rigidbody targetRigidbody in rigidbodies) {
            targetRigidbody.AddForceAtPosition(-transform.forward * 2f, hitPoint, ForceMode.Impulse); // again. - because model backwards
        }
    }
}
