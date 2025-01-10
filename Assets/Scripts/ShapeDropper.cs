using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShapeDropper : MonoBehaviour {

    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject gameOverMenu;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    
    [SerializeField] private List<GameObject> shapes;
    [SerializeField] private List<GameObject> shapePreviews;
    [SerializeField] private List<GameObject> advancedShapes;
    [SerializeField] private List<GameObject> AdvancedShapePreviews;
    
    private bool gameOver = false;
    private int currentScore;
    
    private ShapePreview nextShapePreview;
    private int nextShapeIndex;
    private Vector3 nextRotation;
    private int nextRotationIndex;
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) HandleDrop();
        if (Input.GetKeyDown(KeyCode.R)) RotateBlock();
    }

    void HandleDrop() {
        if (Time.time - timeLastDrop < 1.0f) return;
        
        GameObject dropShape = shapes[nextShapeIndex];
        Quaternion nextRotationQuaternion = Quaternion.Euler(nextRotation);
        Instantiate(dropShape, transform.position, nextRotationQuaternion);
        
        currentScore++;
        if (currentScore == 50) AddAdvancedShapes();
        gameUI.UpdateScore(currentScore);

        timeLastDrop = Time.time;
        Destroy(nextShapePreview.gameObject);
        GenerateNextShape();
    }

    void RotateBlock() {
        nextRotationIndex += 1;
        if (nextRotationIndex > nextShapePreview.GetPossibleRotations.Count-1) nextRotationIndex = 0;
        
        nextRotation = nextShapePreview.GetPossibleRotations[nextRotationIndex];
    }
    
    void GenerateNextShape() {
        nextShapeIndex = Random.Range(0, shapes.Count);
        nextShapePreview = Instantiate(shapePreviews[nextShapeIndex].GetComponent<ShapePreview>());
        nextRotationIndex = 0;
        nextRotation = nextShapePreview.GetPossibleRotations[nextRotationIndex];
        
        nextShapePreview.gameObject.SetActive(false);
        StartCoroutine(SetActiveAfterCooldown(nextShapePreview.gameObject));
    }
    
    void AddAdvancedShapes() {
        shapes.AddRange(advancedShapes);
        shapePreviews.AddRange(AdvancedShapePreviews);
    }
    
    IEnumerator SetActiveAfterCooldown(GameObject inactiveGameObject) {
        yield return new WaitForSeconds(1f);
        inactiveGameObject.SetActive(true);
    }
    
    void UpdatePreviewPosition() {
        nextShapePreview.transform.position = transform.position;
        nextShapePreview.transform.eulerAngles = nextRotation;
    }
    
    void UpdatePosition() {
        Vector3 currentPos = transform.position;
        Vector3 newXPosition = transform.position;
        
        Transform flattenedCameraTransform = cameraTransform;
        flattenedCameraTransform.eulerAngles =
            new Vector3(0, flattenedCameraTransform.eulerAngles.y, flattenedCameraTransform.eulerAngles.z);
        
        if (Input.GetKey(KeyCode.W)) newXPosition += cameraTransform.forward;
        if (Input.GetKey(KeyCode.A)) newXPosition -= cameraTransform.right;
        if (Input.GetKey(KeyCode.S)) newXPosition -= cameraTransform.forward;
        if (Input.GetKey(KeyCode.D)) newXPosition += cameraTransform.right;
        
        float newHeight = transform.position.y + Input.mouseScrollDelta.y * verticalSpeed;
        if (newHeight < 0) newHeight = 0;
        
        transform.position = Vector3.MoveTowards(transform.position, newXPosition, Time.deltaTime * horizontalSpeed);
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
    }
}
