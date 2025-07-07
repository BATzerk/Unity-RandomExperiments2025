using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneHelper {
    // Getters (Public)
    public static bool IsGameScene() { return SceneManager.GetActiveScene().name == SceneNames.GameScene; }


    // ----------------------------------------------------------------
    //  Doers
    // ----------------------------------------------------------------
    static public void ReloadScene() { OpenSceneAsync(SceneManager.GetActiveScene().name); }
    static public void OpenSceneAsync(string sceneName) { SceneManager.LoadSceneAsync(sceneName); }


}
