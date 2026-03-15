using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    // don't re-order these! just add to the bottom to preserve serialized/prefab selections
    private enum TutorialType {
        PressA,
        HorizontalMovement,
        VerticalMovement,
        CameraMove,
        CameraZoom,
        PlaceBlock,
        Rotate,
        HoldBlock,
        None,
        Undo,
    }

    public delegate void DialogCompleted();
    public static event DialogCompleted OnComplete;

    [SerializeField] private TutorialType tutorialType;
    
    [SerializeField] private ProgressBar progressBar;

    private Controls playerControls;

    private float progressPercentage;
    
    private ShapeDropper shapeDropper;
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    private Vector3 shapeDropperPosition;
    private Quaternion cinemachineVirtualCameraRotation;
    private int rotationsDone;
    private Quaternion? rotationPreviousFrame;
    private int holdBlockPresses;

    private void OnEnable()
    {
        if (tutorialType != TutorialType.None) {
            shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
            cinemachineVirtualCamera = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].GetComponent<CinemachineVirtualCamera>();
        }
        
        playerControls = new Controls();
        playerControls.Player.Enable();
        
        switch (tutorialType)
        {
            case (TutorialType.PressA):
                shapeDropper.AddDropsBlocksBlocker();
                break;
            case (TutorialType.HorizontalMovement):
                shapeDropper.AddDropsBlocksBlocker();
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                shapeDropperPosition = shapeDropper.transform.position;
                break;
            case (TutorialType.VerticalMovement):
                shapeDropper.AddDropsBlocksBlocker();
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                shapeDropperPosition = shapeDropper.transform.position;
                break;
            case (TutorialType.CameraMove):
                shapeDropper.AddDropsBlocksBlocker();
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                cinemachineVirtualCameraRotation = cinemachineVirtualCamera.transform.rotation;
                break;
            case (TutorialType.PlaceBlock):
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                break;
            case (TutorialType.Rotate):
                rotationsDone = 0;
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(0);
                break;
            case (TutorialType.HoldBlock):
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(0);
                break;
        }
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        switch (tutorialType)
        {
            case (TutorialType.None): return;
            case (TutorialType.PressA):
                if (playerControls.Player.DropShape.IsPressed()) {
                    shapeDropper.RemoveDropsBlocksBlocker();
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.HorizontalMovement):
                if (shapeDropperPosition.x != shapeDropper.transform.position.x || shapeDropperPosition.z != shapeDropper.transform.position.z) {
                    shapeDropperPosition = shapeDropper.transform.position;
                    progressPercentage += 40f * Time.deltaTime;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
                    shapeDropper.RemoveDropsBlocksBlocker();
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.VerticalMovement):
                if (shapeDropperPosition.y != shapeDropper.transform.position.y) {
                    shapeDropperPosition = shapeDropper.transform.position;
                    progressPercentage += 500f * Time.deltaTime;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
                    shapeDropper.RemoveDropsBlocksBlocker();
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.CameraMove):
                if (cinemachineVirtualCameraRotation != cinemachineVirtualCamera.transform.rotation) {
                    cinemachineVirtualCameraRotation = cinemachineVirtualCamera.transform.rotation;
                    progressPercentage += 80f * Time.deltaTime;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
                    shapeDropper.RemoveDropsBlocksBlocker();
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.PlaceBlock):
                progressPercentage = shapeDropper.GetCurrentScore() * 100 / 3;
                progressBar.SetProgressBarPercentage(progressPercentage);
                if (progressPercentage >= 100) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.Rotate):
                if (rotationPreviousFrame == null) {
                    rotationPreviousFrame = shapeDropper.GetBlockRotation();
                    break;
                }
                if (rotationPreviousFrame != shapeDropper.GetBlockRotation()) {
                    rotationPreviousFrame = shapeDropper.GetBlockRotation();
                    rotationsDone++;
                }
                progressPercentage = rotationsDone * 100 / 4;
                progressBar.SetProgressBarPercentage(progressPercentage);
                if (progressPercentage >= 100) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.HoldBlock):
                if (playerControls.Player.HoldBlock.triggered) {
                    holdBlockPresses++;
                }
                progressPercentage = holdBlockPresses * 100 / 2;
                progressBar.SetProgressBarPercentage(progressPercentage);
                if (progressPercentage >= 100) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.Undo):
                if (playerControls.Player.Undo.triggered) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
        }
    }
}
