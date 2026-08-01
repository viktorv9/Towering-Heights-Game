using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class WindArea : MonoBehaviour {
    [SerializeField] private float windStrength;
    [SerializeField] private GameObject windCollisionEffect;
    
    // Stores last effect spawn time per object
    private Dictionary<GameObject, float> lastEffectTime =
        new ();

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Block"))
            return;

        GameObject obj = other.gameObject;
        
        if (!lastEffectTime.ContainsKey(obj)) {
            lastEffectTime[obj] = Time.time - Random.Range(0.0f, 5.0f);

            if (!Physics.Raycast(obj.transform.position, -transform.forward, out RaycastHit _, 4)) {
                Instantiate(
                    windCollisionEffect,
                    obj.transform.position - transform.forward * Random.Range(1.0f, 1.5f),
                    Quaternion.identity
                );
            }
        }

        Debug.DrawRay(obj.transform.position, transform.forward * -4, Color.blue);

        if (!Physics.Raycast(obj.transform.position, -transform.forward, out RaycastHit _, 4)) {
            obj.transform.parent.GetComponent<Rigidbody>()
                .AddForce(transform.forward * windStrength);

            if (Time.time - lastEffectTime[obj] >= 5.0f) {
                Instantiate(
                    windCollisionEffect,
                    obj.transform.position - transform.forward,
                    Quaternion.identity
                );

                lastEffectTime[obj] = Time.time;
            }
        }
    }
}
