using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private RectTransform RotationUpgradeCursorTransform;
    [SerializeField] private float RotationUpgradeUIDeadzoneSize;

    [Header("Element references")]
    [SerializeField] private Button UpButton;
    [SerializeField] private Button RightButton;
    [SerializeField] private Button DownButton;
    [SerializeField] private Button LeftButton;
    [SerializeField] private GameObject ReleaseTip;
    
    private ShapeDropper shapeDropper;
    private CinemachinePOV cinemachinePOV;
    private CinemachineInputProvider cinemachineInputProvider;
    private Controls playerControls;

    private Vector2 mouseRelative;

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

        if (RotationUpgradeUI.activeSelf) {
            mouseRelative += playerControls.Player.Look.ReadValue<Vector2>();
            RotationUpgradeCursorTransform.localPosition = new Vector3(mouseRelative.x, mouseRelative.y);
            
            if (mouseRelative.magnitude < RotationUpgradeUIDeadzoneSize) {
                selectedRotationDirection = RotationDirection.None;
                ReleaseTip.SetActive(false);
            } else {
                ReleaseTip.SetActive(true);
                if (mouseRelative.Abs().x > mouseRelative.Abs().y) {
                    if (mouseRelative.x > 0) selectedRotationDirection = RotationDirection.Right;
                    else selectedRotationDirection = RotationDirection.Left;
                } else {
                    if (mouseRelative.y > 0) selectedRotationDirection = RotationDirection.Up;
                    else selectedRotationDirection = RotationDirection.Down;
                }
            }

            SetButtonHoverStates(selectedRotationDirection);
        }

        if (playerControls.Player.Rotate.triggered || playerControls.Player.Rotate.WasReleasedThisFrame()) {
            SetRotationUIState(true);
            if (!playerControls.Player.Rotate.WasReleasedThisFrame()) {
                // if rotate was triggered (pressed) and not yet released, reset cursor position
                mouseRelative = new Vector2(0, 0);
                shapeDropper.AddDropsBlocksBlocker();
            } else {
                shapeDropper.RemoveDropsBlocksBlocker();
                if (!shapeDropper.getIsRotating()) {
                    ExecuteRotate(selectedRotationDirection);
                }
            }
        }
        
        if (RotationUpgradeUI.activeSelf && !playerControls.Player.Rotate.IsPressed()) {
            SetRotationUIState(false);
        }
    }
    
    private void SetRotationUIState(bool newState) {
        RotationUpgradeUI.SetActive(newState);
        cinemachineInputProvider.enabled = !newState;
    }
    
    private void SetButtonHoverStates(RotationDirection rotationDirection) {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        
        switch (rotationDirection) {
            case RotationDirection.Up:
                UpButton.Select();
                break;
            case RotationDirection.Right:
                RightButton.Select();
                break;
            case RotationDirection.Down:
                DownButton.Select();
                break;
            case RotationDirection.Left:
                LeftButton.Select();
                break;
        }
    }
    
    private void ExecuteRotate(RotationDirection rotationDirection) {
        if (rotationDirection == RotationDirection.None) return;
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
        
        StartCoroutine(shapeDropper.RotateBlockTowards(rotationValue, 0.3f));
    }
}
