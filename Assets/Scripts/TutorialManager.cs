using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialSteps;

    public enum TutorialType {
        StarterTutorial,
        RotationTutorial,
    }

    private TutorialType? currentTutorialType;
    private GameData gameData;
    
    private void Start() {
        CheckForUncompletedTutorials();
    }

    private void OnEnable() {
        Dialog.OnComplete += NextTutorialStep;
    }

    private void OnDisable() {
        Dialog.OnComplete -= NextTutorialStep;
    }
    
    private void CheckForUncompletedTutorials() {
        gameData = SaveSystem.LoadGameData();

        if (!gameData.tutorialCompleted) {
            currentTutorialType = TutorialType.StarterTutorial;
            tutorialSteps[0].SetActive(true);
            return;
        }
        if (gameData.rotationUnlocked && !gameData.rotationTutorialCompleted) {
            currentTutorialType = TutorialType.RotationTutorial;
            // TODO: set future rotation tutorial active here
            // then when an upgrade is unlocked, make an event (like Upgrade.OnUnlock) and
            // make this class call this function to instantly show the relevant 
            return;
        }
    }
    
    private void NextTutorialStep() {
        if (tutorialSteps.Count > 0) {
            Destroy(tutorialSteps[0]);
            tutorialSteps.RemoveAt(0);
            
            if (tutorialSteps.Count > 0) {
                tutorialSteps[0].SetActive(true);
            } else {
                switch (currentTutorialType) {
                    case (TutorialType.StarterTutorial):
                        gameData.tutorialCompleted = true;
                        break;
                    case (TutorialType.RotationTutorial):
                        gameData.rotationTutorialCompleted = true;
                        break;
                }
                SaveSystem.SaveGameData(gameData);
                CheckForUncompletedTutorials();
            }
        }
    }
}
