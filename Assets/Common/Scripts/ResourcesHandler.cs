using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour {
	/* Note: Some housekeeping tips I can use anywhere in Unity if I want! Putting the reminder here, arbitrarily.
  [Header("Optional")]
  [SerializeField, Tooltip("Dada doopie doo")] private bool informedBool;
  [SerializeField, HideInInspector] private bool hiddenBool;
	*/
	// ----------------------------------------------------------------
	//  Instance
	// ----------------------------------------------------------------
	public static ResourcesHandler Instance; // There can only be one.
	private void Awake() {
		// T... two??
		if (Instance != null) {
			// THERE CAN ONLY BE ONE.
			DestroyImmediate(this.gameObject);
			return;
		}
		// There's no instance already...
		else {
			if (Application.isEditor) { // In the UnityEditor?? Look for ALL InputManagers! We'll get duplicates when we reload our scripts.
				ResourcesHandler[] allOthers = FindObjectsByType<ResourcesHandler>(FindObjectsSortMode.None);
				for (int i=0; i<allOthers.Length; i++) {
					if (allOthers[i] == this) { continue; } // Skip ourselves.
					Destroy(allOthers[i].gameObject);
				}
			}
		}

		// There could only be one. :)
		Instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    // Common
    [Header("Common")]
    [SerializeField] public GameObject go_CubeLine;
	[SerializeField] public GameObject go_SpriteLine;

    // FidgetSnake
    [Header("FidgeSnake")]
	[SerializeField] public GameObject FidgetSnakeVisualSegment;

}
