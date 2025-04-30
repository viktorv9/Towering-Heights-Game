using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillerBlock;
    [SerializeField] private float maxFillerBlockWidth;

    private float barPercentage;

    public void SetProgressBarPercentage(float newPercentage) {
        barPercentage = newPercentage;
        fillerBlock.sizeDelta = new Vector2(maxFillerBlockWidth / 100 * newPercentage, fillerBlock.sizeDelta.y);
    }

    public float GetProgressBarPercentage() {
        return barPercentage;
    }
}
