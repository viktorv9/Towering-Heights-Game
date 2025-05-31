using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialSteps;
    [SerializeField] private List<GameObject> rotationSteps;

    private enum TutorialType {
        StarterTutorial,
        RotationTutorial,
    }

    private TutorialType? currentTutorialType;
    private List<GameObject> currentSteps = new();
    private GameData gameData;
    
    private void Start() {
        CheckForUncompletedTutorials();
    }

    private void OnEnable() {
        Dialog.OnComplete += NextTutorialStep;
        UpgradeManager.OnUpgradeUnlocked += ReadUpgradeUnlocked;
    }

    private void OnDisable() {
        Dialog.OnComplete -= NextTutorialStep;
        UpgradeManager.OnUpgradeUnlocked -= ReadUpgradeUnlocked;
    }
    
    private void ReadUpgradeUnlocked(UpgradeManager.UpgradeType upgradeType) {
        CheckForUncompletedTutorials();
    }
    
    private void CheckForUncompletedTutorials() {
        if (currentTutorialType != null) return;
        gameData = SaveSystem.LoadGameData();
        
        if (!gameData.tutorialCompleted) {
            currentTutorialType = TutorialType.StarterTutorial;
            currentSteps.AddRange(tutorialSteps);
            currentSteps[0]?.SetActive(true);
        } else if (gameData.rotationUnlocked && !gameData.rotationTutorialCompleted) {
            currentTutorialType = TutorialType.RotationTutorial;
            currentSteps.AddRange(rotationSteps);
            currentSteps[0]?.SetActive(true);
        }
    }
    
    private void NextTutorialStep() {
        if (currentSteps.Count > 0) {
            Destroy(currentSteps[0]);
            currentSteps.RemoveAt(0);
            
            if (currentSteps.Count > 0) {
                currentSteps[0].SetActive(true);
            } else {
                switch (currentTutorialType) {
                    case (TutorialType.StarterTutorial):
                        gameData.tutorialCompleted = true;
                        break;
                    case (TutorialType.RotationTutorial):
                        gameData.rotationTutorialCompleted = true;
                        break;
                }
                currentTutorialType = null;
                SaveSystem.SaveGameData(gameData);
                CheckForUncompletedTutorials();
            }
        }
    }
}
