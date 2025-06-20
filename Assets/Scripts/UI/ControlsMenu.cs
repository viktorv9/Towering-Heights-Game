using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI rotationUpgradeText;
    [SerializeField] private TextMeshProUGUI holdUpgradeText;
    
    private void OnEnable() {
        rotationUpgradeText.text = UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.RotationUpgrade) ? "(hold) R" : "LOCKED";
        holdUpgradeText.text = UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.HoldUpgrade) ? "E" : "LOCKED";
    }
}
