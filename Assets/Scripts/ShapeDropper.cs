using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeDropper : MonoBehaviour {

    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private Transform heightFollowTarget;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private List<GameObject> shapes;
    [SerializeField] private List<GameObject> shapePreviews;
    
    private bool gameOver = false;
    private int currentScore;
    
    private GameObject nextShapePreview;
    private int nextShapeIndex;
    private Vector3 nextRotation;
    private float timeLastDrop;

    void Start() {
        timeLastDrop = Time.time - 1.0f;
        GenerateNextShape();
    }
    
    public bool IsGameOver() {
        return gameOver;
    }
    
    public void SetGameOver(bool newState) {
        gameOver = newState;
        if (gameOver) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            gameOverMenu.SetActive(true);
        }
    }

    void Update() {
        if (PauseMenu.GameIsPaused) return;
        
        UpdatePosition();
        UpdatePreviewPosition();
        
        if (IsGameOver()) return;
        if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0)) HandleDrop();
    }

    void HandleDrop() {
        if (Time.time - timeLastDrop < 1.0f) return;
        
        GameObject dropShape = shapes[nextShapeIndex];
        Quaternion nextRotationQuaternion = Quaternion.Euler(nextRotation);
        Instantiate(dropShape, transform.position, nextRotationQuaternion);
        
        currentScore++;
        gameUI.UpdateScore(currentScore);

        timeLastDrop = Time.time;
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
        
        Transform flattenedCameraTransform = cameraTransform;
        flattenedCameraTransform.eulerAngles =
            new Vector3(0, flattenedCameraTransform.eulerAngles.y, flattenedCameraTransform.eulerAngles.z);
        
        if (Input.GetKey(KeyCode.W)) newPosition += cameraTransform.forward * (horizontalSpeed);
        if (Input.GetKey(KeyCode.A)) newPosition -= cameraTransform.right * (horizontalSpeed);
        if (Input.GetKey(KeyCode.S)) newPosition -= cameraTransform.forward * (horizontalSpeed);
        if (Input.GetKey(KeyCode.D)) newPosition += cameraTransform.right * (horizontalSpeed);
        
        newPosition = Vector3.MoveTowards(transform.position, newPosition, Time.deltaTime);
        
        newPosition.y = heightFollowTarget.position.y + 1f;

        transform.position = newPosition;
    }
}
