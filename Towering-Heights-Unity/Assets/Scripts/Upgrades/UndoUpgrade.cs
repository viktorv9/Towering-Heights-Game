using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UndoUpgrade : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private GameObject cooldownContainer;
    [SerializeField] private EventReference undoSound;

    private ShapeDropper shapeDropper;
    private Controls playerControls;
    private Transform cameraTransform;

    private int undoCooldown = 0;
    private float lastDropShapeTriggerTime = 0;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        cameraTransform = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].transform;
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) return;
        if (Time.time - lastDropShapeTriggerTime <= 1.0f) return; // prevents spamming undo while in dropshape cooldown
        
        if (playerControls.Player.DropShape.triggered && undoCooldown > 0) {
            lastDropShapeTriggerTime = Time.time;
            SetCooldown(undoCooldown-1);
        }
        
        if (playerControls.Player.Undo.triggered && undoCooldown <= 0) {
            SetCooldown(5);
            shapeDropper.LoadTowerState();
            AudioManager.instance.PlayOneShot(undoSound, cameraTransform.position);
        }
    }
    
    private void SetCooldown(int newCooldown) {
        undoCooldown = newCooldown;
        
        if (undoCooldown > 0) {
            cooldownContainer.SetActive(true);
            cooldownText.text = undoCooldown.ToString();
            inputText.text = "";
        } else {
            cooldownContainer.SetActive(false);
            inputText.text = "Z";
        }
    }
}
