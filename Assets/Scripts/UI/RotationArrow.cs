using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationArrow : MonoBehaviour {
    
    [SerializeField] private Vector3 rotationValue;

    private RotationUpgrade rotationUpgrade;

    private void Start() {
        rotationUpgrade = transform.parent.parent.GetComponent<RotationUpgrade>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U)) {
            transform.Rotate(rotationValue, Space.World);
            rotationUpgrade.ExecuteRotate(rotationValue);
        }
    }

    private void OnMouseDown() {
        rotationUpgrade.ExecuteRotate(rotationValue);
    }
}
