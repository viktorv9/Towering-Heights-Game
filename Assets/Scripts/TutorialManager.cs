using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> tutorialSteps;

    private void OnEnable() {
        Dialog.OnComplete += NextTutorialStep;
    }

    private void OnDisable() {
        Dialog.OnComplete -= NextTutorialStep;
    }
    
    private void NextTutorialStep() {
        if (tutorialSteps.Count > 0) {
            Destroy(tutorialSteps[0]);
            tutorialSteps.RemoveAt(0);
            if (tutorialSteps.Count > 0) tutorialSteps[0].SetActive(true);
        }
    }
}
