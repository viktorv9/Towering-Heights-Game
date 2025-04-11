using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WorldBase : MonoBehaviour {

    [SerializeField] private ShapeDropper shapeDropper;
    [SerializeField] private HeightGoal heightGoal;
    [SerializeField] private float platformY = 1;

    [SerializeField] private List<Pickup> pickups;

    private void Start() {
        SpawnPickup(pickups[Random.Range(0, pickups.Count-1)]);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.C)) {
            SpawnPickup(pickups[Random.Range(0, pickups.Count-1)]);
        }
    }

    public void SpawnPickup(Pickup pickup) {
        float range = 3;
        float maxHeight = (heightGoal != null) ? heightGoal.GetCurrentGoalHeight() - 1 : 100; // temp: if no heightgoal spawn somewhere below y=100
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

    private void SpawnNewPlatform() {
        float range = 3;
        while (true) {
            for (int i = 0; i < 100; i++) {
                Vector3 randomSpawnPoint = new Vector3(Random.Range(range, -range), platformY, Random.Range(range, -range));
                if (Physics.CheckBox(randomSpawnPoint, Vector3.one / 2)) {
                    Debug.Log(randomSpawnPoint);
                    return;
                }
            }

            range = range + 1;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Block")) {
            shapeDropper.SetGameOver(true);
        }
    }
}
