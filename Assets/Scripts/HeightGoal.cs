using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightGoal : MonoBehaviour
{
    [SerializeField] private List<float> goalHeights;
    [SerializeField] private float goalCollisionDuration;

    [SerializeField] private Canvas canvas;

    private List<GameObject> collidersInTrigger = new ();
    private float timeCollisionStart = 0;
    
    private void Start() {
        transform.position = new Vector3(0, goalHeights[0], 0);
    }

    private void Update() {
        canvas.transform.forward = Camera.main.transform.forward;

        if (timeCollisionStart != 0) Debug.Log(Time.time - timeCollisionStart);
        
        if (timeCollisionStart != 0) {
            if (Time.time - timeCollisionStart > goalCollisionDuration) {
                Debug.Log("Reached high goal!! New height reached: " + transform.position.y + "m.");
                timeCollisionStart = 0;
                NextGoalHeight();
            }
        }
    }
    
    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Add(other.gameObject);
        
        if (collidersInTrigger.Count > 0) {
            if (timeCollisionStart == 0) timeCollisionStart = Time.time;
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (!other.gameObject.CompareTag("Block")) return;
        collidersInTrigger.Remove(other.gameObject);
        
        if (collidersInTrigger.Count == 0) {
            timeCollisionStart = 0;
        }
    }
    
    private void NextGoalHeight() {
        goalHeights.RemoveAt(0);
        if (goalHeights.Count > 0) {
            var newGoalHeight = goalHeights[0];
            transform.position = new Vector3(0, newGoalHeight, 0);
        } else {
            Destroy(gameObject); // out of goals, destroy
        }
    }
}
