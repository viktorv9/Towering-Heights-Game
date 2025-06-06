using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachinePOV cinemachinePOV;
    private CinemachineFramingTransposer cinemachineFramingTransposer;

    private void Awake() {
        cinemachinePOV = cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update() {
        // bugfix for cinemachine issue where camera keeps spinning on pause (only with SpeedMode.InputValueGain)
        if (PauseMenu.GameIsPaused && cinemachinePOV.m_HorizontalAxis.m_SpeedMode != AxisState.SpeedMode.MaxSpeed) {
            cinemachinePOV.m_HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
            cinemachinePOV.m_VerticalAxis.m_SpeedMode = AxisState.SpeedMode.MaxSpeed;
        } else if (!PauseMenu.GameIsPaused && cinemachinePOV.m_HorizontalAxis.m_SpeedMode != AxisState.SpeedMode.InputValueGain) {
            cinemachinePOV.m_HorizontalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
            cinemachinePOV.m_VerticalAxis.m_SpeedMode = AxisState.SpeedMode.InputValueGain;
        }
    }
    
    private void OnEnable() {
        Settings.OnUpdateSettings += SetCameraSettings;
    }

    private void OnDisable() {
        Settings.OnUpdateSettings -= SetCameraSettings;
    }
    
    private void SetCameraSettings(Settings.GameSettings gameSettings) {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = gameSettings.cameraSensitivity;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = gameSettings.cameraSensitivity;
        cinemachinePOV.m_HorizontalAxis.m_InvertInput = gameSettings.invertCameraX;
        cinemachinePOV.m_VerticalAxis.m_InvertInput = gameSettings.invertCameraY;
    }

    public void ZoomCamera(float zoomAmount) {
        cinemachineFramingTransposer.m_CameraDistance += zoomAmount;
        if (cinemachineFramingTransposer.m_CameraDistance < 0.1f) cinemachineFramingTransposer.m_CameraDistance = 0.1f;
        if (cinemachineFramingTransposer.m_CameraDistance > 30) cinemachineFramingTransposer.m_CameraDistance = 30;
    }
}
