using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour {
    [SerializeField] private UpgradeManager.UpgradeType upgradeType;
    [SerializeField] private GameObject upgradeGameObject;
    
    private void OnEnable() {
        UpgradeManager.OnUpgradeUnlocked += SetUpgradeActive;
    }
    
    private void OnDisable() {
        UpgradeManager.OnUpgradeUnlocked -= SetUpgradeActive;
    }
    
    private void SetUpgradeActive(UpgradeManager.UpgradeType unlockedUpgradeType) {
        if (upgradeType == unlockedUpgradeType) upgradeGameObject.SetActive(true);
    }
}
