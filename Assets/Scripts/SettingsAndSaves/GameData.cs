using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    public bool tutorialCompleted;
    public bool rotationTutorialCompleted;
    public float highestHeightGoalHeight;

    // with a run based system these would have to be saved into a separate run-specific save
    public bool rotationUnlocked;
    
    public GameData (
        bool tutorialIsCompleted = false,
        bool rotationTutorialIsCompleted = false,
        bool rotationIsUnlocked = false,
        float highestHeightGoal = 0
    ) {
        tutorialCompleted = tutorialIsCompleted;
        rotationTutorialCompleted = rotationTutorialIsCompleted;
        highestHeightGoalHeight = highestHeightGoal;

        rotationUnlocked = rotationIsUnlocked;
    }
}
