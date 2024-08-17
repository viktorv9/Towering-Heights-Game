using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    [SerializeField] private float moveSpeed;

    private float minCameraHeight;

    private void Start() {
        minCameraHeight = transform.position.y;
    }

    void Update()
    {
        float scrollMovement = Input.mouseScrollDelta.y;
        transform.position = new Vector3(transform.position.x, transform.position.y + scrollMovement * moveSpeed * Time.deltaTime, transform.position.z);
        if (transform.position.y < minCameraHeight) transform.position = new Vector3(transform.position.x, minCameraHeight, transform.position.z);
    }
}
