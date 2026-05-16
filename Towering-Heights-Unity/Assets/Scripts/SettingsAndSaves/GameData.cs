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

    // with a run based system these would have to be saved into a separate run-specific save
    public bool rotationUnlocked;
    public bool holdBlockUnlocked;
    public bool undoBlockUnlocked;

    public List<string> completedLevels;
    
    public GameData (
        bool tutorialIsCompleted = false,
        bool rotationTutorialIsCompleted = false,
        bool holdBlockTutorialIsCompleted = false,
        bool undoTutorialIsCompleted = false,
        bool rotationIsUnlocked = false,
        bool holdBlockIsUnlocked = false,
        bool undoIsUnlocked = false,
        List<string> levelsCompleted = null
    ) {
        tutorialCompleted = tutorialIsCompleted;
        rotationTutorialCompleted = rotationTutorialIsCompleted;
        holdBlockTutorialCompleted = holdBlockTutorialIsCompleted;
        undoTutorialCompleted = undoTutorialIsCompleted;
        
        rotationUnlocked = rotationIsUnlocked;
        holdBlockUnlocked = holdBlockIsUnlocked;
        undoBlockUnlocked = undoIsUnlocked;

        completedLevels = levelsCompleted ?? new List<string>();
    }
}
