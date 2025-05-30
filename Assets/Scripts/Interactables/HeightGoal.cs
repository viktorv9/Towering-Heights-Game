using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class HeightGoal : MonoBehaviour
{
    
    [Serializable]
    private struct Goal {
        public float goalHeight;
        public UpgradeManager.UpgradeType upgradeToUnlock;
    }

    [SerializeField] private List<Goal> goals;
    [SerializeField] private float goalCollisionDuration;
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI countdownText;

    private List<GameObject> collidersInTrigger = new ();
    private float timeCollisionStart = 0;
    
    private void Start() {
        transform.position = new Vector3(0, goals[0].goalHeight, 0);
    }

    private void Update() {
        var turnTowardsCamera = Camera.main.transform.forward;
        if (Math.Abs(turnTowardsCamera.x) > 0.001f || Math.Abs(turnTowardsCamera.z) > 0.001f) canvas.transform.forward = new Vector3(turnTowardsCamera.x, 0, turnTowardsCamera.z);

        if (timeCollisionStart != 0) {
            UpdateMessage((int)(5 - Math.Floor(Time.time - timeCollisionStart)));
            
            if (Time.time - timeCollisionStart > goalCollisionDuration) {
                timeCollisionStart = 0;
                NextGoalHeight();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Add(other.gameObject);
        
        if (collidersInTrigger.Count > 0) {
            countdownText.gameObject.SetActive(true);
            if (timeCollisionStart == 0) timeCollisionStart = Time.time;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Remove(other.gameObject);
        
        if (collidersInTrigger.Count == 0) {
            countdownText.gameObject.SetActive(false);
            timeCollisionStart = 0;
        }
    }
    
    public float GetCurrentGoalHeight() {
        return goals[0].goalHeight;
    }
    
    private void NextGoalHeight() {
        UpgradeManager.UnlockUpgrade(goals[0].upgradeToUnlock);
        goals.RemoveAt(0);
        if (goals.Count > 0) {
            transform.position = new Vector3(0, goals[0].goalHeight, 0);
        } else {
            Destroy(gameObject); // out of goals, destroy
        }
    }
    
    private void UpdateMessage(int remainingTime) {
        countdownText.text = "Height goal reached! Testing stability: " + remainingTime;
    }
}
