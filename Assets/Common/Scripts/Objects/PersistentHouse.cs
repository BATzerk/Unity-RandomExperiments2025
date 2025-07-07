using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/** Put persistent-across-program stuff as a child in this GameObject and it'll persist across scenes! */
public class PersistentHouse : MonoBehaviour {
	// NOTE: Not sure WHO should own the responsibility of reloading scene when reloading scripts, so I'll do it here!
#if UNITY_EDITOR
	[UnityEditor.Callbacks.DidReloadScripts]
	private static void OnScriptsReloaded() {
		if (Application.isPlaying)
			SceneHelper.ReloadScene();
	}
#endif

	// There can only be one.
	private static PersistentHouse instance;

	private void Awake () {
		// T... two??
		if (instance != null) {
			// THERE CAN ONLY BE ONE.
			DestroyImmediate (this.gameObject);
			return;
		}
        // There's no instance already...
        else {
            if (Application.isEditor) { // In the UnityEditor?? Look for ALL PersistentHouses! We'll get duplicates when we reload our scripts.
                PersistentHouse[] allHouses = FindObjectsByType<PersistentHouse>(FindObjectsSortMode.None);
                for (int i=0; i<allHouses.Length; i++) {
                    if (allHouses[i] == this) { continue; } // Skip ourselves.
                    Destroy(allHouses[i].gameObject);
                }
            }
        }

		// There could only be one. :)
		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}


    private void Update() {
        // ENTER = Reload current scene.
        if (InputUtils.Down(Key.Enter)) {
			ReloadScene();
            return;
        }
    }

	public void ReloadScene() { SceneHelper.ReloadScene(); }
}
