using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBase : MonoBehaviour {

    [SerializeField] private HeightGoal heightGoal;
    [SerializeField] private float platformY = 1;
    [SerializeField] private GameObject platformPart;

    [SerializeField] private List<Pickup> randomPickups;

    private ShapeDropper shapeDropper;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        if (randomPickups.Count > 0) SpawnPickup(randomPickups[Random.Range(0, randomPickups.Count-1)]);
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
                    var pickupGameObject = Instantiate(pickup, randomSpawnPoint, Quaternion.identity);
                    Pickup newPickup = pickupGameObject.GetComponent<Pickup>();
                    if (newPickup.GetPickupType() == Pickup.PickupType.Platform) newPickup.OnPickup += HandlePlatformPickup;
                    return;
                }
            }
            range++;
        }
    }
    
    private void HandlePlatformPickup() {
        SpawnNewPlatform();
        SpawnPickup(randomPickups[Random.Range(0, randomPickups.Count-1)]);
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
