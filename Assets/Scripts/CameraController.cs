using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private Slider cameraSensitivitySlider;
    [SerializeField] private Toggle horizontalInputInvertedToggle;
    [SerializeField] private Toggle verticalInputInvertedToggle;

    private CinemachinePOV cinemachinePOV;
    
    private void Start() {
        cinemachinePOV = cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        
        cameraSensitivitySlider.onValueChanged.AddListener(delegate {
            SetCameraMaxSpeed(cameraSensitivitySlider.value);
        });
        horizontalInputInvertedToggle.onValueChanged.AddListener(delegate {
            SetHorizontalInputInverted(horizontalInputInvertedToggle.isOn);
        });
        verticalInputInvertedToggle.onValueChanged.AddListener(delegate {
            SetVerticalInputInverted(verticalInputInvertedToggle.isOn);
        });
    }
    
    private void SetCameraMaxSpeed(float newMaxSpeed) {
        cinemachinePOV.m_HorizontalAxis.m_MaxSpeed = newMaxSpeed;
        cinemachinePOV.m_VerticalAxis.m_MaxSpeed = newMaxSpeed;
    }
    
    private void SetHorizontalInputInverted(bool isInverted) {
        cinemachinePOV.m_HorizontalAxis.m_InvertInput = isInverted;
    }
    
    private void SetVerticalInputInverted(bool isInverted) {
        cinemachinePOV.m_VerticalAxis.m_InvertInput = isInverted;
    }
}
