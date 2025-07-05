using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData {
    public bool tutorialCompleted;
    public bool rotationTutorialCompleted;
    public bool holdBlockTutorialCompleted;
    public bool undoTutorialCompleted;

    public float highestHeightGoalHeight;

    // with a run based system these would have to be saved into a separate run-specific save
    public bool rotationUnlocked;
    public bool holdBlockUnlocked;
    public bool undoBlockUnlocked;
    
    public GameData (
        bool tutorialIsCompleted = false,
        bool rotationTutorialIsCompleted = false,
        bool holdBlockTutorialIsCompleted = false,
        bool undoTutorialIsCompleted = false,
        bool rotationIsUnlocked = false,
        bool holdBlockIsUnlocked = false,
        bool undoIsUnlocked = false,
        float highestHeightGoal = 0
    ) {
        tutorialCompleted = tutorialIsCompleted;
        rotationTutorialCompleted = rotationTutorialIsCompleted;
        holdBlockTutorialCompleted = holdBlockTutorialIsCompleted;
        undoTutorialCompleted = undoTutorialIsCompleted;
        
        highestHeightGoalHeight = highestHeightGoal;

        rotationUnlocked = rotationIsUnlocked;
        holdBlockUnlocked = holdBlockIsUnlocked;
        undoBlockUnlocked = undoIsUnlocked;
    }
}
