using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Toggle horizontalInputInvertedToggle;
    [SerializeField] private Toggle verticalInputInvertedToggle;

    private CinemachinePOV cinemachinePOV;
    private CinemachineFramingTransposer cinemachineFramingTransposer;

    private void Start() {
        cinemachinePOV = cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        cinemachineFramingTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        
        LoadPlayerSettings();
        
        cameraSensitivitySlider.onValueChanged.AddListener(delegate {
            SetCameraSpeed(cameraSensitivitySlider.value);
        });
        horizontalInputInvertedToggle.onValueChanged.AddListener(delegate {
            SetHorizontalInputInverted(horizontalInputInvertedToggle.isOn);
        });
        verticalInputInvertedToggle.onValueChanged.AddListener(delegate {
            SetVerticalInputInverted(verticalInputInvertedToggle.isOn);
        });
    }

    private void LoadPlayerSettings()
    {
        float savedCameraSensitivity = PlayerPrefs.GetFloat("CameraSensitivity", cameraSensitivitySlider.value);
        SetCameraSpeed(savedCameraSensitivity);
        cameraSensitivitySlider.value = savedCameraSensitivity;
        
        bool savedHorizontalInputInverted = Convert.ToBoolean(
            PlayerPrefs.GetInt("HorizontalInputInverted", horizontalInputInvertedToggle.isOn ? 1 : 0));
        SetHorizontalInputInverted(savedHorizontalInputInverted);
        horizontalInputInvertedToggle.isOn = savedHorizontalInputInverted;
        
        bool savedVerticalInputInverted = Convert.ToBoolean(
            PlayerPrefs.GetInt("VerticalInputInverted", verticalInputInvertedToggle.isOn ? 1 : 0));
        SetVerticalInputInverted(savedVerticalInputInverted);
        verticalInputInvertedToggle.isOn = savedVerticalInputInverted;
    }
    
    private void SetCameraSpeed(float newSpeed) {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = newSpeed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = newSpeed;
        PlayerPrefs.SetFloat("CameraSensitivity", newSpeed);
    }
    
    private void SetHorizontalInputInverted(bool isInverted) {
        cinemachinePOV.m_HorizontalAxis.m_InvertInput = isInverted;
        PlayerPrefs.SetInt("HorizontalInputInverted", isInverted ? 1 : 0);
    }
    
    private void SetVerticalInputInverted(bool isInverted) {
        cinemachinePOV.m_VerticalAxis.m_InvertInput = isInverted;
        PlayerPrefs.SetInt("VerticalInputInverted", isInverted ? 1 : 0);
    }

    public void ZoomCamera(float zoomAmount) {
        cinemachineFramingTransposer.m_CameraDistance += zoomAmount;
        if (cinemachineFramingTransposer.m_CameraDistance < 0.1f) cinemachineFramingTransposer.m_CameraDistance = 0.1f;
        if (cinemachineFramingTransposer.m_CameraDistance > 30) cinemachineFramingTransposer.m_CameraDistance = 30;
    }
}
