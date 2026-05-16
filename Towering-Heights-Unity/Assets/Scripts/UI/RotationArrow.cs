using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotationArrow : MonoBehaviour, IPointerEnterHandler {
    
    [SerializeField] private RotationUpgrade rotationUpgrade;
    [SerializeField] private RotationUpgrade.RotationDirection rotationDirection;

    public void OnPointerEnter(PointerEventData eventData) {
        rotationUpgrade.selectedRotationDirection = rotationDirection;
    }
}
