using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectSpawner : MonoBehaviour {
    
    [SerializeField] private List<GameObject> effects;
    [SerializeField] float radius;
    [SerializeField] float interval;
    
    float timeLastSpawn = 0;

    private void OnDrawGizmosSelected() {
        Gizmos.color = new Color(0.3f, 0.1f, 0.55f, 1f);
        Gizmos.DrawWireSphere(transform.position, radius); 
    }

    private void FixedUpdate() {
        if (Time.time - timeLastSpawn > interval) {
            timeLastSpawn = Time.time;
            GameObject spawnEffect = Instantiate(effects[Random.Range(0, effects.Count)], Random.insideUnitSphere * radius, transform.rotation);
            Destroy(spawnEffect, 3f);
        }
    }
}
