using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreTextArea;

    public void UpdateScore(int newScore) {
        string newText = "score :\n" + newScore + " / 50";
        if (newScore >= 50) newText += "\n\nadvanced\nshaped\nadded!";
        scoreTextArea.SetText(newText);
        if (newScore == 50) scoreTextArea.color = Color.yellow;
    }
}
