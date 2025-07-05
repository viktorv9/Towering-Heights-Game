using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class UndoUpgrade : MonoBehaviour {

    private ShapeDropper shapeDropper;
    private Controls playerControls;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) return;
        
        if (playerControls.Player.Undo.triggered) shapeDropper.LoadTowerState();
    }
}
