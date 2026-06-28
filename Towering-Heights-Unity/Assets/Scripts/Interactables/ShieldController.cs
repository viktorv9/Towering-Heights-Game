using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private GameObject shieldObject;

    private Controls playerControls;
    
    void Start() {
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        
        if (playerControls.Player.Shield.IsPressed() && !shieldObject.activeSelf) {
            shieldObject.SetActive(true);
        }
        if (!playerControls.Player.Shield.IsPressed() && shieldObject.activeSelf) {
            shieldObject.SetActive(false);
        }
    }
}
