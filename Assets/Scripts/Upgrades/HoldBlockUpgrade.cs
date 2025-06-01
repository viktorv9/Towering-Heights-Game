using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class HoldBlockUpgrade : MonoBehaviour {

    [SerializeField] private List<Sprite> shapePreviewSprites;
    [SerializeField] private Image shapePreviewImage;
    
    private ShapeDropper shapeDropper;
    private Controls playerControls;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) return;
    
        if (playerControls.Player.HoldBlock.triggered) {
            int shapePreviewIndex = shapeDropper.HoldBlock();
            shapePreviewImage.sprite = shapePreviewSprites[shapePreviewIndex];
        }
    }
}
