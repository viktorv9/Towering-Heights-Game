using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    [SerializeField] private float stepSize;
    [SerializeField] private float moveSpeed;

    private float scrollMovement;
    
    private float minCameraHeight;
    private Vector3 targetPosition;

    private void Start() {
        minCameraHeight = transform.position.y;
        targetPosition = transform.position;
    }

    void Update() {
        float scrollMovement = Input.mouseScrollDelta.y;
        // if (Input.mouseScrollDelta.y != 0) scrollMovement = (Input.mouseScrollDelta.y > 0 ? 1 : -1);
        
        targetPosition = new Vector3(targetPosition.x, targetPosition.y + scrollMovement * stepSize, targetPosition.z);
        if (targetPosition.y < minCameraHeight) targetPosition = new Vector3(targetPosition.x, minCameraHeight, targetPosition.z);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
