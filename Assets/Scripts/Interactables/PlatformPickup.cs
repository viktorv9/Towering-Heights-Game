using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformPickup : Pickup {
    [SerializeField] private GameObject platformPrefab;
    
    private float platformY = 1;
    
    protected override void PickUp() {
        SpawnNewPlatform();
        Destroy(gameObject);
    }
    
    private void SpawnNewPlatform() {
        float range = 3;
        while (true) {
            for (int i = 0; i < 100; i++) {
                Vector3 randomSpawnPoint = new Vector3((int) Random.Range(range, -range), platformY, (int) Random.Range(range, -range));
                if (!Physics.CheckBox(randomSpawnPoint, Vector3.one / 2.1f)) {
                    Instantiate(platformPrefab, randomSpawnPoint, Quaternion.identity);
                    return;
                }
            }

            range = range + 1;
        }
    }
}
