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

    private void OnEnable()
    {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        cinemachineVirtualCamera = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].GetComponent<CinemachineVirtualCamera>();
        
        playerControls = new Controls();
        playerControls.Player.Enable();
        
        switch (tutorialType)
        {
            case (TutorialType.PressA):
                shapeDropper.SetDropsBlocks(false);
                break;
            case (TutorialType.HorizontalMovement):
                shapeDropper.SetDropsBlocks(false);
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                shapeDropperPosition = shapeDropper.transform.position;
                break;
            case (TutorialType.VerticalMovement):
                shapeDropper.SetDropsBlocks(false);
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                shapeDropperPosition = shapeDropper.transform.position;
                break;
            case (TutorialType.CameraMove):
                shapeDropper.SetDropsBlocks(false);
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                cinemachineVirtualCameraRotation = cinemachineVirtualCamera.transform.rotation;
                break;
            case (TutorialType.PlaceBlock):
                shapeDropper.SetDropsBlocks(true);
                progressPercentage = 0;
                progressBar.SetProgressBarPercentage(progressPercentage);
                break;
        }
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;

        switch (tutorialType)
        {
            case (TutorialType.PressA):
                if (playerControls.Player.DropShape.IsPressed()) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.HorizontalMovement):
                if (shapeDropperPosition.x != shapeDropper.transform.position.x || shapeDropperPosition.z != shapeDropper.transform.position.z) {
                    shapeDropperPosition = shapeDropper.transform.position;
                    progressPercentage += 0.2f;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.VerticalMovement):
                if (shapeDropperPosition.y != shapeDropper.transform.position.y) {
                    shapeDropperPosition = shapeDropper.transform.position;
                    progressPercentage += 2f;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                }
                break;
            case (TutorialType.CameraMove):
                if (cinemachineVirtualCameraRotation != cinemachineVirtualCamera.transform.rotation) {
                    cinemachineVirtualCameraRotation = cinemachineVirtualCamera.transform.rotation;
                    progressPercentage += 0.5f;
                    progressBar.SetProgressBarPercentage(progressPercentage);
                }
                if (progressPercentage >= 100) {
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
        }
    }
}
