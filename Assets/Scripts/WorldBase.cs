using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBase : MonoBehaviour {

    [SerializeField] private HeightGoal heightGoal;
    [SerializeField] private float platformY = 1;
    [SerializeField] private GameObject platformPart;

    [SerializeField] private List<Pickup> pickups;

    private ShapeDropper shapeDropper;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        SpawnPickup(pickups[Random.Range(0, pickups.Count-1)]);
    }

    private void OnEnable() {
        Pickup.OnPickup += HandlePickup;
    }

    private void OnDisable() {
        Pickup.OnPickup -= HandlePickup;
    }

    public void SpawnPickup(Pickup pickup) {
        float range = 3;
        float maxHeight = 5; // TODO: beter scaling based on current tower height
        while (true) {
            for (int i = 0; i < 100; i++) {
                Vector3 randomSpawnPoint = new Vector3(
                    (int) Random.Range(range, -range),
                    (int) Random.Range(platformY + 1, maxHeight),
                    (int) Random.Range(range, -range)
                    );
                if (!Physics.CheckBox(randomSpawnPoint, Vector3.one / 2.1f)) {
                    Instantiate(pickup, randomSpawnPoint, Quaternion.identity);
                    return;
                }
            }
            range++;
        }
    }
    
    private void HandlePickup(Pickup.PickupType pickupType) {
        if (pickupType == Pickup.PickupType.Platform) {
            SpawnNewPlatform();
        }
        SpawnPickup(pickups[Random.Range(0, pickups.Count-1)]);
    }

    private void SpawnNewPlatform() {
        float range = 3;
        while (true) {
            for (int i = 0; i < 100; i++) {
                Vector3 randomSpawnPoint = new Vector3((int) Random.Range(range, -range), platformY, (int) Random.Range(range, -range));
                if (!Physics.CheckBox(randomSpawnPoint, Vector3.one / 2.1f)) {
                    Instantiate(platformPart, randomSpawnPoint, Quaternion.identity);
                    return;
                }
            }

            range++;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (shapeDropper.GetHasWon()) return;
        
        if (other.CompareTag("Block")) {
            shapeDropper.SetGameOver(true);
        }
    }
}
