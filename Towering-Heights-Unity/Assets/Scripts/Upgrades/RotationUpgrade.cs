using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private Button upButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Button downButton;
    [SerializeField] private Button leftButton;
    [SerializeField] private GameObject releaseTip;
    [SerializeField] private GameObject rotationPreview;
    
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
                releaseTip.SetActive(false);
                rotationPreview.SetActive(false);
            } else {
                releaseTip.SetActive(true);
                rotationPreview.SetActive(true);
                rotationPreview.transform.position = shapeDropper.transform.position;
                var xDir = Convert.ToSingle(Math.Round(cinemachinePOV.m_VerticalAxis.Value / 90) % 4);
                var yDir = Convert.ToSingle(Math.Round(cinemachinePOV.m_HorizontalAxis.Value / 90) % 4);
                Vector3 arrowAngles = new Vector3(xDir * 90, yDir * 90, 0);
                if (mouseRelative.Abs().x > mouseRelative.Abs().y) {
                    if (mouseRelative.x > 0) {
                        selectedRotationDirection = RotationDirection.Right;
                        arrowAngles.z = 180;
                    } else {
                        selectedRotationDirection = RotationDirection.Left;
                    }
                } else {
                    if (mouseRelative.y > 0) {
                        selectedRotationDirection = RotationDirection.Up;
                        arrowAngles.z = 270;
                    } else {
                        selectedRotationDirection = RotationDirection.Down;
                        arrowAngles.z = 90;
                    }
                }
                rotationPreview.transform.localEulerAngles = arrowAngles;
            }

            SetButtonHoverStates(selectedRotationDirection);
        } else {
            if (rotationPreview.activeSelf) rotationPreview.SetActive(false);
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
                upButton.Select();
                break;
            case RotationDirection.Right:
                rightButton.Select();
                break;
            case RotationDirection.Down:
                downButton.Select();
                break;
            case RotationDirection.Left:
                leftButton.Select();
                break;
        }
    }
    
    private void ExecuteRotate(RotationDirection rotationDirection) {
        if (rotationDirection == RotationDirection.None) return;
        Vector3 rotationValue = new Vector3();
        
        switch (cinemachinePOV.m_HorizontalAxis.Value) {
            case > 45 and <= 135: {
                // Looking to: +X
                if (rotationDirection == RotationDirection.Up) rotationValue.z = -90;
                if (rotationDirection == RotationDirection.Down) rotationValue.z = 90;
            
                if (cinemachinePOV.m_VerticalAxis.Value > 45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.x = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.x = -90;
                } else if (cinemachinePOV.m_VerticalAxis.Value < -45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.x = -90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.x = 90;
                } else {
                    if (rotationDirection == RotationDirection.Left) rotationValue.y = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.y = -90;
                }

                break;
            }
            case > 135 and <= 225: {
                // Looking to: -Z
                if (rotationDirection == RotationDirection.Up) rotationValue.x = -90;
                if (rotationDirection == RotationDirection.Down) rotationValue.x = 90;
            
                if (cinemachinePOV.m_VerticalAxis.Value > 45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.z = -90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.z = 90;
                } else if (cinemachinePOV.m_VerticalAxis.Value < -45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.z = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.z = -90;
                } else {
                    if (rotationDirection == RotationDirection.Left) rotationValue.y = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.y = -90;
                }

                break;
            }
            case > 225 and <= 315: {
                // Looking to: -X
                if (rotationDirection == RotationDirection.Up) rotationValue.z = 90;
                if (rotationDirection == RotationDirection.Down) rotationValue.z = -90;
            
                if (cinemachinePOV.m_VerticalAxis.Value > 45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.x = -90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.x = 90;
                } else if (cinemachinePOV.m_VerticalAxis.Value < -45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.x = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.x = -90;
                } else {
                    if (rotationDirection == RotationDirection.Left) rotationValue.y = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.y = -90;
                }

                break;
            }
            default: {
                // Looking to: +Z
                if (rotationDirection == RotationDirection.Up) rotationValue.x = 90;
                if (rotationDirection == RotationDirection.Down) rotationValue.x = -90;
            
                if (cinemachinePOV.m_VerticalAxis.Value > 45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.z = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.z = -90;
                } else if (cinemachinePOV.m_VerticalAxis.Value < -45) {
                    if (rotationDirection == RotationDirection.Left) rotationValue.z = -90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.z = 90;
                } else {
                    if (rotationDirection == RotationDirection.Left) rotationValue.y = 90;
                    if (rotationDirection == RotationDirection.Right) rotationValue.y = -90;
                }

                break;
            }
        }
        
        StartCoroutine(shapeDropper.RotateBlockTowards(rotationValue, 0.3f));
    }
}
