using UnityEngine;
using System.Collections;

/** This class does ONE thing-- set our base application properties right away, so we can start getting into the fun game side of things! */
public class ProgramPropertySetter : MonoBehaviour {

	private void Awake() {
		SetProgramProperties();
	}

	private void SetProgramProperties() {
		//// Program properties
		//SetSpaceWarp(true); // yes, please!
		//foveatedRenderingLevel = FoveatedRenderingLevel.High; // TEST
		//useDynamicFoveatedRendering = true;
		//GameManagers.Instance.LocalizationManager.SetCurrentLanguageFromSave();

		QualitySettings.vSyncCount = 0;
		Application.runInBackground = true; // a hack for Oculus, so Universal Menu doesn't suspend our rendering of the game.
	}


}
