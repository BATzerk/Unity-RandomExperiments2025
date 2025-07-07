using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeController : MonoBehaviour {
    // Properties
    static public float FrameTimeScale { get; private set; }
    static public float FrameTimeScaleUnscaled { get; private set; }

    private bool debug_doPauseNextFrame = false; // used to advance one frame at a time.


    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update () {
		FrameTimeScale = Application.targetFrameRate * Time.deltaTime;
		FrameTimeScaleUnscaled = Application.targetFrameRate * Time.unscaledDeltaTime;

        if (debug_doPauseNextFrame) {
            Time.timeScale = 0;
            debug_doPauseNextFrame = false;
        }

        // DEBUG INPUT!
        //if (Application.isEditor) {
            // Keyboard
            //if (InputUtils.Down(Key.F))
            //    Time.timeScale = InputUtils.Held(Key.LeftShift) ? 100 : 20;
            //else if (InputUtils.Up(Key.F))
            //    Time.timeScale = 1;
            if (InputUtils.Down(Key.G))
                Time.timeScale = 0.05f;
            else if (InputUtils.Up(Key.G))
                Time.timeScale = 1;
            else if (Time.timeScale == 0 && InputUtils.Down(Key.A)) {
                debug_doPauseNextFrame = true;
                Time.timeScale = 1;
            }
    }



}
