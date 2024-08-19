using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBase : MonoBehaviour {

    [SerializeField] private ShapeDropper shapeDropper;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Block")) {
            shapeDropper.SetGameOver(true);
        }
    }
}
