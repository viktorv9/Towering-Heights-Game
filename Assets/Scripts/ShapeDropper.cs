using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class ShapeDropper : MonoBehaviour {
    
    [Header("UI links")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverMenuFirstSelected;
    [SerializeField] private GameObject hasWonMenu;
    [SerializeField] private GameObject hasWonMenuFirstSelected;

    [Header("Scene related links")]
    [SerializeField] private GameObject blocksHolder;
    [SerializeField] private GameObject heightGoal;
    [SerializeField] private Transform cameraTransform;
    
    [Header("Control settings")]
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float holdSpeedUpMultiplier;
    
    [Header("Shapes")]
    [SerializeField] private List<GameObject> shapes;
    [SerializeField] private List<GameObject> shapePreviews;
    [SerializeField] private List<GameObject> advancedShapes;
    [SerializeField] private List<GameObject> advancedShapePreviews;
    
    [Header("Other settings")]
    [SerializeField] private bool showScore;

    private Controls playerControls;
    private int dropShapeBlockers = 0;
    
    private CameraController cameraController;

    private bool gameOver;
    private bool hasWon;
    private int currentScore;
    
    private ShapePreview nextShapePreview;
    private int nextShapeIndex;
    // private Vector3 nextRotation;
    // private int nextRotationIndex;
    private float timeLastDrop;

    private int? heldBlockIndex;
    private bool canUndo;
    
    private GameObject lastSavedTowerState;
    private ShapePreview lastNextShapePreview;
    private int lastNextShapeIndex;
    private int? lastHeldBlockIndex;

    void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();

        UpgradeManager.LoadUnlockedUpgrades();
        
        cameraController = cameraTransform.gameObject.GetComponent<CameraController>();
        
        gameUI.SetShowScore(showScore);

        timeLastDrop = Time.time - 1.0f;
        GenerateNextShape(null);
    }

    void Update() {
        if (PauseMenu.GameIsPaused) return;
        
        UpdatePosition();
        UpdatePreviewPosition();
        
        if (IsGameOver()) return;
        if (playerControls.Player.DropShape.triggered && dropShapeBlockers == 0) HandleDrop();
        // if (playerControls.Player.Rotate.triggered) RotateBlock(); replaced by rotation upgrade.
    }
    
    public void AddDropsBlocksBlocker() {
        dropShapeBlockers++;
    }
    
    public void RemoveDropsBlocksBlocker() {
        dropShapeBlockers--;
    }

    public bool IsGameOver() {
        return gameOver;
    }

    public int GetCurrentScore() {
        return currentScore;
    }

    public void SetGameOver(bool newState) {
        gameOver = newState;
        if (gameOver) {
            gameOverMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(gameOverMenuFirstSelected);
        }
    }

    public void SetHasWon(bool newState) {
        hasWon = newState;
        if (hasWon) {
            hasWonMenu.SetActive(true);
            eventSystem.SetSelectedGameObject(gameOverMenuFirstSelected);
        }
    }

    public bool GetHasWon() {
        return hasWon;
    }

    void HandleDrop() {
        if (Time.time - timeLastDrop < 1.0f) return;
        
        SaveTowerState();

        GameObject dropShape = shapes[nextShapeIndex];
        // Quaternion nextRotationQuaternion = Quaternion.Euler(nextRotation);
        Instantiate(dropShape, transform.position, nextShapePreview.transform.rotation, blocksHolder.transform);
        
        currentScore++;
        if (showScore) {
            if (currentScore == 50) AddAdvancedShapes();
            gameUI.UpdateScore(currentScore);
        }

        canUndo = true;
        timeLastDrop = Time.time;
        Destroy(nextShapePreview.gameObject);
        GenerateNextShape(null);
    }

    // void RotateBlock() {
    //     nextRotationIndex += 1;
    //     if (nextRotationIndex > nextShapePreview.GetPossibleRotations.Count-1) nextRotationIndex = 0;
    //     
    //     nextRotation = nextShapePreview.GetPossibleRotations[nextRotationIndex];
    // }
    
    public void RotateBlockTowards(Vector3 rotationValue) {
        nextShapePreview.transform.Rotate(rotationValue, Space.World);
    }
    
    public Quaternion? GetBlockRotation() {
        if (nextShapePreview == null) return null;
        return nextShapePreview.transform.rotation;
    }
    
    public int HoldBlock() {
        if (heldBlockIndex == null) {
            heldBlockIndex = nextShapeIndex;
            Destroy(nextShapePreview.gameObject);
            nextShapeIndex = Random.Range(0, shapes.Count);
            while (nextShapeIndex == heldBlockIndex) {
                nextShapeIndex = Random.Range(0, shapes.Count); // it's not fun to re-roll into the same shape
            }
            nextShapePreview = Instantiate(shapePreviews[nextShapeIndex]).GetComponent<ShapePreview>();
            return (int)heldBlockIndex;
        } else {
            int tempNextShapeIndex = nextShapeIndex;
            nextShapeIndex = (int)heldBlockIndex;
            heldBlockIndex = tempNextShapeIndex;
            Destroy(nextShapePreview.gameObject);
            nextShapePreview = Instantiate(shapePreviews[nextShapeIndex]).GetComponent<ShapePreview>();
            return (int)heldBlockIndex;
        }
    }
    
    void GenerateNextShape(int? shapeIndex) {
        if (shapeIndex == null) nextShapeIndex = Random.Range(0, shapes.Count);
        else nextShapeIndex = shapeIndex.Value;
        nextShapePreview = Instantiate(shapePreviews[nextShapeIndex]).GetComponent<ShapePreview>();
        
        // nextShapePreview = Instantiate(shapePreviews[nextShapeIndex].GetComponent<ShapePreview>());
        // nextRotationIndex = 0;
        // nextRotation = nextShapePreview.GetPossibleRotations[nextRotationIndex];

        nextShapePreview.gameObject.SetActive(false);
        StartCoroutine(SetActiveAfterCooldown(nextShapePreview.gameObject));
    }
    
    void AddAdvancedShapes() {
        shapes.AddRange(advancedShapes);
        shapePreviews.AddRange(advancedShapePreviews);
    }
    
    IEnumerator SetActiveAfterCooldown(GameObject inactiveGameObject) {
        yield return new WaitForSeconds(1f);
        if (inactiveGameObject) inactiveGameObject.SetActive(true);
    }

    void SaveTowerState() {
        if (lastSavedTowerState) Destroy(lastSavedTowerState);
        lastSavedTowerState = Instantiate(blocksHolder);
        lastSavedTowerState.SetActive(false);
    }
    
    public void LoadTowerState() {
        if (lastSavedTowerState && canUndo) {
            Destroy(blocksHolder);
            GameObject newBlocksHolder = Instantiate(lastSavedTowerState);
            newBlocksHolder.SetActive(true);
            blocksHolder = newBlocksHolder;
            
            Destroy(nextShapePreview.gameObject);
            nextShapePreview = Instantiate(shapePreviews[nextShapeIndex]).GetComponent<ShapePreview>();
            currentScore--;
            canUndo = false;
        }
    }
    
    void UpdatePreviewPosition() {
        nextShapePreview.transform.position = transform.position;
        // nextShapePreview.transform.eulerAngles = nextRotation;
    }
    
    void UpdatePosition() {
        Transform flattenedCameraTransform = cameraTransform;
        flattenedCameraTransform.eulerAngles = new Vector3(0, flattenedCameraTransform.eulerAngles.y, flattenedCameraTransform.eulerAngles.z);
        Vector3 newHorizontalPosition = transform.position;
        float newHeight = transform.position.y;

        Vector2 playerMovementHorizontalInput = playerControls.Player.MoveHorizontal.ReadValue<Vector2>();
        playerMovementHorizontalInput *= Time.deltaTime * horizontalSpeed;
        if (playerControls.Player.MoveSpeedUp.IsPressed()) playerMovementHorizontalInput *= holdSpeedUpMultiplier;
        
        if (playerMovementHorizontalInput.x != 0) newHorizontalPosition += cameraTransform.right * playerMovementHorizontalInput.x;
        if (playerMovementHorizontalInput.y != 0) newHorizontalPosition += cameraTransform.forward * playerMovementHorizontalInput.y;

        Vector2 playerMovementVerticalInput = playerControls.Player.MoveVertical.ReadValue<Vector2>();
        newHeight += playerMovementVerticalInput.y * verticalSpeed;
        if (newHeight < 0) newHeight = 0;
        
        Vector2 playerZoomInput = playerControls.Player.Zoom.ReadValue<Vector2>();
        cameraController.ZoomCamera(-playerZoomInput.y * zoomSpeed);
        
        transform.position = new Vector3(newHorizontalPosition.x, newHeight, newHorizontalPosition.z);
    }
}
