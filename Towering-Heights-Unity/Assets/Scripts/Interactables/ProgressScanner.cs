using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProgressScanner : MonoBehaviour
{
    
    [SerializeField] private BoxCollider scannerCollider;
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private HeightGoal heightGoal;

    [SerializeField] private float scanStepSize = 1;
    [SerializeField] private float progressBarAnimationSpeed = 2.50f;
    
    [SerializeField] private float scanCooldown = 0.3f;
    private float nextScanTime;
    
    private float maxProgressBarHeight;
    private float heightGoalHeight;
    private float targetProgressHeight;
    private float displayedProgressHeight;
    
    void Start()
    {
        var currentHeightGoalHeight = heightGoal.GetCurrentGoalHeight();
        if (!currentHeightGoalHeight.HasValue) return;
        
        heightGoalHeight = currentHeightGoalHeight.Value;
        maxProgressBarHeight = progressBar.rect.height;
        targetProgressHeight = 0f;
        displayedProgressHeight = 0f;
        progressBar.sizeDelta = new Vector2(progressBar.sizeDelta.x, 0);
        ResetScannerToGoalHeight();
    }

    void Update()
    {
        ScanForBlock();
        AnimateProgressBar();
    }

    private void ScanForBlock()
    {

        float nextScannerHeight = Mathf.Max(transform.position.y - scanStepSize, 0f);
        transform.position = new Vector3(transform.position.x, nextScannerHeight, transform.position.z);
        
        if (WouldCollideWithBlockAtOffset(0f))
        {
            UpdateTargetProgressHeight(transform.position.y);
            ResetScannerToGoalHeight();
            return;
        }

        if (transform.position.y <= 0f)
        {
            targetProgressHeight = 0f;
            ResetScannerToGoalHeight();
        }
    }

    private void AnimateProgressBar()
    {
        displayedProgressHeight = Mathf.MoveTowards(
            displayedProgressHeight,
            targetProgressHeight,
            progressBarAnimationSpeed * Time.deltaTime);
        
        
        displayedProgressHeight = Mathf.Lerp(displayedProgressHeight, targetProgressHeight, Time.deltaTime * 0.2f);

        progressBar.sizeDelta = new Vector2(
            progressBar.sizeDelta.x,
            Mathf.Clamp(displayedProgressHeight, 0f, maxProgressBarHeight));
    }

    private bool WouldCollideWithBlockAtOffset(float yOffset)
    {
        Bounds bounds = scannerCollider.bounds;
        Vector3 targetCenter = bounds.center + Vector3.up * yOffset;
        Collider[] overlaps = Physics.OverlapBox(targetCenter, bounds.extents, scannerCollider.transform.rotation);

        return overlaps.Any(hit => hit != null && hit.gameObject != gameObject && hit.CompareTag("Block"));
    }

    private void UpdateTargetProgressHeight(float detectedHeight)
    {
        if (Time.time < nextScanTime) return;
        nextScanTime = Time.time + scanCooldown;
        float clampedHeight = Mathf.Clamp(detectedHeight, 0f, heightGoalHeight);
        targetProgressHeight = maxProgressBarHeight * (clampedHeight / heightGoalHeight);
    }

    private void ResetScannerToGoalHeight()
    {
        transform.position = new Vector3(transform.position.x, heightGoalHeight, transform.position.z);
    }
}
