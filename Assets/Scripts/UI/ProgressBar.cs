using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillerBlock;

    private float maxFillerBlockWidth;
    private float barPercentage;

    private void Start() {
        RectTransform barBlock = gameObject.GetComponent<RectTransform>();
        maxFillerBlockWidth = barBlock.rect.width;
    }

    public void SetProgressBarPercentage(float newPercentage) {
        barPercentage = newPercentage;
        fillerBlock.sizeDelta = new Vector2(maxFillerBlockWidth / 100 * barPercentage, fillerBlock.sizeDelta.y);
    }

    public float GetProgressBarPercentage() {
        return barPercentage;
    }
}
