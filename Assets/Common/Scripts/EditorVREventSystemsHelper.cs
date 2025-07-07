/* TEMPORARILY DISABLED UNTIL WE IMPORT OCULUS STUFF!
using Oculus.Interaction;
using UnityEngine;

/**
 * Add this to a Canvas with PointableCanvasModule.
 * This editor-only class enables/disables PointableCanvasModule when our headset is unmounted, so we can use the mouse for UI.
 * /
public class EditorVREventSystemsHelper : MonoBehaviour
{
#if UNITY_EDITOR
    // Components
    private PointableCanvasModule pointableCanvasModule;

    // ---------------------------------------------------------
    //  Awake / Destroy
    // ---------------------------------------------------------
    void Awake() {
        pointableCanvasModule = GetComponent<PointableCanvasModule>();

        // Call the not-auto-called Unmounted event.
        OnHMDUnmounted();

        // Add event listeners!
        OVRManager.HMDUnmounted += OnHMDUnmounted;
        OVRManager.HMDMounted += OnHMDMounted;
    }
    private void OnDestroy() {
        // Remove event listeners!
        OVRManager.HMDUnmounted -= OnHMDUnmounted;
        OVRManager.HMDMounted -= OnHMDMounted;
    }

    // ---------------------------------------------------------
    //  Events
    // ---------------------------------------------------------
    private void OnHMDUnmounted() {
        pointableCanvasModule.enabled = false;
    }
    private void OnHMDMounted() {
        pointableCanvasModule.enabled = true;
    }


#endif
}
*/