using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationController : MonoBehaviour {
    // ----------------------------------------------------------------
    //  Instance / Start
    // ----------------------------------------------------------------
    public static VibrationController Instance; // There can only be one.
    private void Awake() {
        // Instance already set? Destroy me. I'm the OG.
        if (Instance != null) {
            DestroyImmediate(this.gameObject);
            return;
        }
        // There's no instance already...
        else {
            if (Application.isEditor) { // In the UnityEditor?? We'll get duplicates when we reload our scripts.
                VibrationController[] allOthers = FindObjectsByType<VibrationController>(FindObjectsSortMode.None);
                for (int i=0; i<allOthers.Length; i++) {
                    if (allOthers[i] == this) { continue; } // Skip ourselves.
                    Destroy(allOthers[i].gameObject);
                }
            }
        }

        // I am the one!
        Instance = this;
    }


    private Coroutine c_vibL, c_vibR;

    // ----------------------------------------------------------------
    //  Vibrate!
    // ----------------------------------------------------------------
    /* DISABLED UNTIL WE IMPORT OCULUS STUFF!
    public void Vibrate(WhichHand whichHand, float amplitude, float duration, float frequency=0.5f) {
        bool isLeft = whichHand == WhichHand.Left;
        Vibrate(isLeft, amplitude, duration, frequency);
    }
    public void Vibrate(OVRInput.Controller controller, float amplitude, float duration, float frequency=0.5f) {
        bool isLeft = controller == OVRInput.Controller.LTouch || controller == OVRInput.Controller.LHand;
        Vibrate(isLeft, amplitude, duration, frequency);
    }
    public void Vibrate(bool isLeft, float amplitude, float duration, float frequency=0.5f) {
        // Cancel existing planned stops.
        if (isLeft && c_vibL!=null)
            StopCoroutine(c_vibL);
        if (!isLeft && c_vibR!=null)
            StopCoroutine(c_vibR);

        // Vvibrate!
        OVRInput.SetControllerVibration(frequency, amplitude, isLeft ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch);

        // Plan to stop it after the duration.
        if (isLeft)
            c_vibL = StartCoroutine(IE_StopVibrationL(duration));
        else
            c_vibR = StartCoroutine(IE_StopVibrationR(duration));
    }

    private IEnumerator IE_StopVibrationL(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
    }
    private IEnumerator IE_StopVibrationR(float delay) {
        yield return new WaitForSecondsRealtime(delay);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
    */

}
