using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject collisionEffect;
    [SerializeField] private GameObject shieldCollisionEffect;
    [SerializeField] private GameObject trailEffectBurst;

    private Vector3 previousPosition;
    private bool hasHit = false;
    float radius;
    
    public void Initialize(Vector3 direction)
    {
        transform.forward = -direction; // - because model is backwards
        rb.velocity = -transform.forward * speed; // - because model is backwards
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        radius = mesh.bounds.extents.magnitude;
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
            Destroy(gameObject);
        }
        else if (other.transform.CompareTag("Block") && other.transform.name != "rock_platform") {
            ApplyExplosionForce();

            hasHit = true;
            Instantiate(collisionEffect, hitPoint, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    
    public void ApplyExplosionForce()
    {
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = new Collider[50];
        List<Rigidbody> rigidbodies = new List<Rigidbody>();
        Physics.OverlapSphereNonAlloc(explosionPosition, 4f, colliders);

        foreach (Collider collider in colliders)
        {
            if (collider != null && collider.attachedRigidbody != null)
            {
                rigidbodies.Add(collider.attachedRigidbody);
            }
        }

        foreach (Rigidbody targetRigidbody in rigidbodies) {
            targetRigidbody.AddExplosionForce(20f, explosionPosition, 1, 1f);

        }
    }
}
