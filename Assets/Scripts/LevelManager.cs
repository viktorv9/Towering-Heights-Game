using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private void OnEnable() {
    }

    public void LoadLevel(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
