using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ControlsMenu : MonoBehaviour {
    
    [SerializeField] private TextMeshProUGUI rotationUpgradeText;
    [SerializeField] private TextMeshProUGUI holdUpgradeText;
    [SerializeField] private TextMeshProUGUI undoUpgradeText;
    
    private void OnEnable() {
        if (rotationUpgradeText != null) rotationUpgradeText.text = UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.RotationUpgrade) ? "(hold) R" : "LOCKED";
        if (holdUpgradeText != null) holdUpgradeText.text = UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.HoldUpgrade) ? "E" : "LOCKED";
        if (undoUpgradeText != null) undoUpgradeText.text = UpgradeManager.UnlockedUpgrades.Contains(UpgradeManager.UpgradeType.UndoUpgrade) ? "Z" : "LOCKED";
    }
}
