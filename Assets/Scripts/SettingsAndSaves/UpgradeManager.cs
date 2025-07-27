using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UpgradeManager
{
    public enum UpgradeType {
        None,
        RotationUpgrade,
        HoldUpgrade,
        StabilityBlocksUpgrade,
        UndoUpgrade,
    }
    
    public delegate void UpgradeUnlocked(UpgradeType upgradeType);
    public static event UpgradeUnlocked OnUpgradeUnlocked;

    public static readonly List<UpgradeType> UnlockedUpgrades = new();
    
    public static List<UpgradeType> LoadUnlockedUpgrades() {
        GameData gameData = SaveSystem.LoadGameData();
        if (gameData.rotationUnlocked) {
            OnUpgradeUnlocked?.Invoke(UpgradeType.RotationUpgrade);
            UnlockedUpgrades.Add(UpgradeType.RotationUpgrade);
        }
        if (gameData.holdBlockUnlocked) {
            OnUpgradeUnlocked?.Invoke(UpgradeType.HoldUpgrade);
            UnlockedUpgrades.Add(UpgradeType.HoldUpgrade);
        }
        if (gameData.undoBlockUnlocked) {
            OnUpgradeUnlocked?.Invoke(UpgradeType.UndoUpgrade);
            UnlockedUpgrades.Add(UpgradeType.UndoUpgrade);
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
            case (UpgradeType.HoldUpgrade):
                UnlockedUpgrades.Add(UpgradeType.HoldUpgrade);
                gameData.holdBlockUnlocked = true;
                break;
            case (UpgradeType.UndoUpgrade):
                UnlockedUpgrades.Add(UpgradeType.UndoUpgrade);
                gameData.undoBlockUnlocked = true;
                break;
        }
        SaveSystem.SaveGameData(gameData);
        OnUpgradeUnlocked?.Invoke(upgradeType);
    }
}
