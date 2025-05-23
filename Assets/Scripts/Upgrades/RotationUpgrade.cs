using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RotationUpgrade : MonoBehaviour {

    [SerializeField] private GameObject RotationUpgradeUI;
    
    private ShapeDropper shapeDropper;
    private CinemachineInputProvider cinemachineInputProvider;
    private Controls playerControls;
    
    private void Start() {
        shapeDropper = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<ShapeDropper>();
        cinemachineInputProvider = GameObject.FindGameObjectsWithTag("VirtualCamera")[0].GetComponent<CinemachineInputProvider>();
        playerControls = new Controls();
        playerControls.Player.Enable();
    }

    private void Update() {
        transform.position = shapeDropper.transform.position;
        if (playerControls.Player.Rotate.triggered) {
            RotationUpgradeUI.SetActive(!RotationUpgradeUI.activeSelf);
            cinemachineInputProvider.enabled = !RotationUpgradeUI.activeSelf;
            if (RotationUpgradeUI.activeSelf) shapeDropper.AddDropsBlocksBlocker();
            else shapeDropper.RemoveDropsBlocksBlocker();
        }
    }
    
    public void ExecuteRotate(Vector3 rotationValue) {
        shapeDropper.RotateBlockTowards(rotationValue);
    }
}
