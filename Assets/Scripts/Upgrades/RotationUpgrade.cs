using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RotationUpgrade : MonoBehaviour {
    
    public enum RotationDirection {
        None,
        Left,
        Right,
        Up,
        Down
    }

    public RotationDirection selectedRotationDirection = RotationDirection.None;

    [SerializeField] private GameObject RotationUpgradeUI;
    
    private ShapeDropper shapeDropper;
    private CinemachinePOV cinemachinePOV;
    private CinemachineInputProvider cinemachineInputProvider;
    private Controls playerControls;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        cinemachinePOV = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachinePOV>();
        cinemachineInputProvider = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].GetComponent<CinemachineInputProvider>();
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) {
            if (RotationUpgradeUI.activeSelf) {
                if (RotationUpgradeUI.activeSelf) SetRotationUIState(false);
                Cursor.lockState = InputHandler.CurrentInputMode == InputHandler.InputMode.Keyboard ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = InputHandler.CurrentInputMode == InputHandler.InputMode.Keyboard;
            }
            return;
        }

        if (playerControls.Player.Rotate.triggered || playerControls.Player.Rotate.WasReleasedThisFrame()) {
            SetRotationUIState(true);
            if (playerControls.Player.Rotate.WasReleasedThisFrame()) {
                ExecuteRotate(selectedRotationDirection);
            }
        }
        
        if (RotationUpgradeUI.activeSelf && !playerControls.Player.Rotate.IsPressed()) {
            SetRotationUIState(false);
        }
    }
    
    private void SetRotationUIState(bool newState) {
        RotationUpgradeUI.SetActive(newState);
        cinemachineInputProvider.enabled = !newState;
        if (newState) shapeDropper.AddDropsBlocksBlocker();
        else shapeDropper.RemoveDropsBlocksBlocker();
    }
    
    private void ExecuteRotate(RotationDirection rotationDirection) {
        Vector3 rotationValue = new Vector3();
        if (rotationDirection == RotationDirection.Left) rotationValue.y = 90;
        if (rotationDirection == RotationDirection.Right) rotationValue.y = -90;
        if (rotationDirection == RotationDirection.Up || rotationDirection == RotationDirection.Down) {
            if (cinemachinePOV.m_HorizontalAxis.Value > 45 && cinemachinePOV.m_HorizontalAxis.Value <= 135) {
                if (rotationDirection == RotationDirection.Up) rotationValue.z = -90;
                if (rotationDirection == RotationDirection.Down) rotationValue.z = 90;
            } else if (cinemachinePOV.m_HorizontalAxis.Value > 135 && cinemachinePOV.m_HorizontalAxis.Value <= 225) {
                if (rotationDirection == RotationDirection.Up) rotationValue.x = -90;
                if (rotationDirection == RotationDirection.Down) rotationValue.x = 90;
            } else if (cinemachinePOV.m_HorizontalAxis.Value > 225 && cinemachinePOV.m_HorizontalAxis.Value <= 315) {
                if (rotationDirection == RotationDirection.Up) rotationValue.z = 90;
                if (rotationDirection == RotationDirection.Down) rotationValue.z = -90;
            } else { 
                if (rotationDirection == RotationDirection.Up) rotationValue.x = 90;
                if (rotationDirection == RotationDirection.Down) rotationValue.x = -90;
            }
        }
        shapeDropper.RotateBlockTowards(rotationValue);
    }
}
