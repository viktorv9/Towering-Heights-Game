using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour {

    public void LoadLevel(string sceneName) {
        Resources.UnloadUnusedAssets();
        SceneManager.LoadScene(sceneName);
    }
}
