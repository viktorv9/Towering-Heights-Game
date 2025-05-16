using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeManager
{
    public enum UpgradeType {
        RotationUpgrade,
        HoldUpgrade,
        StabilityBlocksUpgrade
    }
    
    public delegate void UpgradeUnlocked(UpgradeType upgradeType);
    public static event UpgradeUnlocked OnUpgradeUnlocked;

    public static readonly List<UpgradeType> UnlockedUpgrades = new();
    
    public static List<UpgradeType> LoadUnlockedUpgrades() {
        GameData gameData = SaveSystem.LoadGameData();
        if (gameData.rotationUnlocked) {
            OnUpgradeUnlocked(UpgradeType.RotationUpgrade);
            UnlockedUpgrades.Add(UpgradeType.RotationUpgrade);
        }
        return UnlockedUpgrades;
    }
    
    public static void UnlockUpgrade(UpgradeType upgradeType) {
        GameData gameData = SaveSystem.LoadGameData();
        switch (upgradeType) {
            case (UpgradeType.RotationUpgrade):
                UnlockedUpgrades.Add(UpgradeType.RotationUpgrade);
                gameData.rotationUnlocked = true;
                break;
        }
        SaveSystem.SaveGameData(gameData);
        OnUpgradeUnlocked?.Invoke(upgradeType);
    }
}
