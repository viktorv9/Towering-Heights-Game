using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeDropper : MonoBehaviour {

    [SerializeField] private GameUI gameUI;

    [SerializeField] private Transform heightFollowTarget;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private List<GameObject> shapes;
    [SerializeField] private List<GameObject> shapePreviews;
    
    private int currentScore;
    
    private GameObject nextShapePreview;
    private int nextShapeIndex;
    private Vector3 nextRotation;

    void Start() {
        GenerateNextShape();
    }

    void Update() {
        if (PauseMenu.GameIsPaused) return;
        
        UpdatePosition();
        UpdatePreviewPosition();
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) HandleDrop();
    }

    void HandleDrop() {
        GameObject dropShape = shapes[nextShapeIndex];
        Quaternion nextRotationQuaternion = Quaternion.Euler(nextRotation);
        Instantiate(dropShape, transform.position, nextRotationQuaternion);
        
        currentScore++;
        gameUI.UpdateScore(currentScore);
        
        Destroy(nextShapePreview);
        GenerateNextShape();
    }
    
    void GenerateNextShape() {
        nextShapeIndex = Random.Range(0, (shapes.Count));
        nextRotation = GenerateRandomRotation();
        nextShapePreview = Instantiate(shapePreviews[nextShapeIndex]);
    }
    
    Vector3 GenerateRandomRotation() {
        int randomXRot = Random.Range(0, 4) * 90;
        int randomYRot = Random.Range(0, 4) * 90;
        int randomZRot = Random.Range(0, 4) * 90;
        return new Vector3(randomXRot, randomYRot, randomZRot);
    }
    
    void UpdatePreviewPosition() {
        nextShapePreview.transform.position = transform.position;
        nextShapePreview.transform.eulerAngles = nextRotation;
    }
    
    void UpdatePosition() {
        Vector3 newPosition = transform.position;

        newPosition.y = heightFollowTarget.position.y + 1f;

        if (Input.GetKey(KeyCode.W)) newPosition += cameraTransform.forward * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A)) newPosition -= cameraTransform.right * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S)) newPosition -= cameraTransform.forward * (horizontalSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D)) newPosition += cameraTransform.right * (horizontalSpeed * Time.deltaTime);
        
        newPosition = Vector3.MoveTowards(transform.position, newPosition, 0.01f);

        transform.position = newPosition;
    }
}
