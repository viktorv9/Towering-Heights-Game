using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinEffect : MonoBehaviour
{
    private ShapeDropper shapeDropper;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
    }

    void Update() {
        transform.position = shapeDropper.transform.position;
    }
}
