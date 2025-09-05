using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class HoldBlockUpgrade : MonoBehaviour {

    [SerializeField] private List<Sprite> shapePreviewSprites;
    [SerializeField] private Image shapePreviewImage;
    [SerializeField] private EventReference holdBlockSound;

    private ShapeDropper shapeDropper;
    private Controls playerControls;
    private Transform cameraTransform;

    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        cameraTransform = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].transform;
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        if (PauseMenu.GameIsPaused) return;
    
        if (playerControls.Player.HoldBlock.triggered) {
            int shapePreviewIndex = shapeDropper.HoldBlock();
            shapePreviewImage.sprite = shapePreviewSprites[shapePreviewIndex];
            AudioManager.instance.PlayOneShot(holdBlockSound, cameraTransform.position);
        }
    }
}
