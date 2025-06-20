using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    public bool tutorialCompleted;
    public bool rotationTutorialCompleted;
    public bool holdBlockTutorialCompleted;

    public float highestHeightGoalHeight;

    // with a run based system these would have to be saved into a separate run-specific save
    public bool rotationUnlocked;
    public bool holdBlockUnlocked;
    
    public GameData (
        bool tutorialIsCompleted = false,
        bool rotationTutorialIsCompleted = false,
        bool holdBlockTutorialIsCompleted = false,
        bool rotationIsUnlocked = false,
        bool holdBlockIsUnlocked = false,
        float highestHeightGoal = 0
    ) {
        tutorialCompleted = tutorialIsCompleted;
        rotationTutorialCompleted = rotationTutorialIsCompleted;
        holdBlockTutorialCompleted = holdBlockTutorialIsCompleted;
        
        highestHeightGoalHeight = highestHeightGoal;

        rotationUnlocked = rotationIsUnlocked;
        holdBlockUnlocked = holdBlockIsUnlocked;
    }
}
