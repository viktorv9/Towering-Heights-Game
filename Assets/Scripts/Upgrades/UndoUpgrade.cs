using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UndoUpgrade : MonoBehaviour {

    private ShapeDropper shapeDropper;
    private Controls playerControls;

    private int undoCooldown = 0;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) return;
        if (playerControls.Player.DropShape.triggered) undoCooldown--; // BUG: tijdens drop cooldown kan je dit spammen en dus de cooldown skippen
        
        if (playerControls.Player.Undo.triggered && undoCooldown <= 0) {
            undoCooldown = 5;
            shapeDropper.LoadTowerState();
        }
    }
}
