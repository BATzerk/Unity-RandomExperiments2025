#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteAlways]
public class PrefabEditModeChecker : MonoBehaviour {
    //public static bool IsPrefabBeingEdited = false;

    [InitializeOnLoadMethod]
    private static void Initialize() {
        PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        PrefabStage.prefabStageClosing += OnPrefabStageClosing;
    }

    // NOTE: I save this (instead of having a static bool) so it persists after reloading all scripts (which is the whole point of me making this class in the first place).
    private static void OnPrefabStageOpened(PrefabStage stage) {
        SaveStorage.SetBool(SaveKeys.Editor_IsPrefabBeingEdited, true);
    }
    private static void OnPrefabStageClosing(PrefabStage stage) {
        SaveStorage.SetBool(SaveKeys.Editor_IsPrefabBeingEdited, false);
    }
}
#endif