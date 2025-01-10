using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private CinemachinePOV cinemachinePOV;
    
    private void Start() {
        cinemachinePOV = cinemachineVirtualCamera.GetCinemachineComponent<CinemachinePOV>();
    }
    
    public void SetHorizontalInputInverted(bool isInverted) {
        cinemachinePOV.m_HorizontalAxis.m_InvertInput = isInverted;
    }
    
    public void SetVerticalInputInverted(bool isInverted) {
        cinemachinePOV.m_VerticalAxis.m_InvertInput = isInverted;
    }
}
