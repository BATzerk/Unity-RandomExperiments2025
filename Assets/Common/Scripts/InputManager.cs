using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour {
	// Constants
	private const float DEAD_ZONE_MIN = 0.15f; // any raw axis input less than this value is set to 0. For controllers that "drift".
	// Properties
	private float timeLastClickedMouseL = Mathf.NegativeInfinity; // in unscaled time.
	public bool isDoubleClickMouseL { get; private set; }
    private bool pisFastForwardHeld;
    public bool isFastForwardHeld { get; private set; }
    //public bool isFastForwardUp { get; private set; }
    public float gripL { get; private set; } // middle-finger hand triggers
	public float gripR { get; private set; } // middle-finger hand triggers
	public float triggerL { get; private set; } // pointer-finger hand triggers (either VR *or* Xbox)
	public float triggerR { get; private set; } // pointer-finger hand triggers
	float pgripL, pgripR; // prev frame's middle-finger hand triggers
	float ptriggerL, pTriggerR; // prev frame's pointer-finger hand triggers
	//public Collider CurrCursorCollider { get; private set; }
	//public CursorSource CurrCursorSource { get; private set; } = CursorSource.HandR; // so we can swap seamlessly between controlling with mouse, left hand, and right hand.
	//public Vector3 handLCursorPosGlobal { get; private set; } // where the VR hand controller's ray-hit is in global space.
	//public Vector3 handRCursorPosGlobal { get; private set; } // where the VR hand controller's ray-hit is in global space.
	//public Vector3 mouseCursorPosGlobal { get; private set; } // from the camera, where the mouse is in global space.
    public Vector2 joystickLAxis { get; private set; }
	public Vector2 joystickRAxis { get; private set; }
    public Vector2 joystickCardinalLAxis { get; private set; } // isolates input to EXCLUDE diagonal. Either only has x, or only has y.
    public Vector2 joystickCardinalRAxis { get; private set; } // isolates input to EXCLUDE diagonal. Either only has x, or only has y.

    // Getters
    public bool isSouthDown { get { return buttonSouth.triggered; } }
    public bool isEastDown { get { return buttonEast.triggered; } }
    public bool isWestDown { get { return buttonWest.triggered; } }
    public bool isNorthDown { get { return buttonNorth.triggered; } }
    public bool isActionDown { get { return buttonSouth.triggered; } } // A button on the gamepad or keyboard.
    public bool isCancelDown { get { return buttonEast.triggered; } } // B button on the gamepad or keyboard.
    public bool isDashDown { get { return buttonWest.triggered; } } // X button on the gamepad or keyboard.
    public bool isJumpUp { get { return buttonNorth.WasReleasedThisFrame(); } }
    public bool isJumpDown { get { return buttonNorth.triggered; } }
    public bool isPauseDown { get { return pauseAction.triggered; } } // Pause button on the gamepad or keyboard.
    public bool isChangeElfDown { get { return changeElfAction.triggered; } }
    public bool isRamUp => GetRTriggerUp();
    public bool isRamDown => GetRTriggerDown();
    public bool isRamHeld => GetRTriggerHeld();
    public bool isCameraRecenterDown => GetLTriggerDown();
    public bool isCameraRecenterHeld => GetLTriggerHeld();
    public bool isFastForwardDown => isFastForwardHeld && !pisFastForwardHeld;
    public bool isFastForwardUp => !isFastForwardHeld && pisFastForwardHeld;

    public bool LeftBumperDown { get { return leftBumper.triggered; } }
    public bool RightBumperDown { get { return rightBumper.triggered; } }
    public float LeftBumper { get { return leftBumper.ReadValue<float>(); } }
    public float RightBumper { get { return rightBumper.ReadValue<float>(); } }
    public float LeftTrigger => leftTrigger.ReadValue<float>();
    public float RightTrigger => rightTrigger.ReadValue<float>();

    public bool GetLGrabDown() { return GetLGripDown() || GetLTriggerDown(); }
	public bool GetRGrabDown() { return GetRGripDown() || GetRTriggerDown(); }
	public bool GetLGrabUp() { return (GetLGripUp() || GetLTriggerUp())  &&  !GetLGrabHeld(); }// && !GetHandLTriggerHeld(); }// && !GetHandLFistHeld() && !GetHandLPinchHeld(); }
	public bool GetRGrabUp() { return (GetRGripUp() || GetRTriggerUp())  &&  !GetRGrabHeld(); }
	public bool GetLGrabHeld() { return GetLGripHeld() || GetLTriggerHeld(); }
	public bool GetRGrabHeld() { return GetRGripHeld() || GetRTriggerHeld(); }

	public bool GetLGripUp() { return (pgripL>=0.1f && gripL<0.1f) || InputUtils.Up(Key.Space); } // DEBUG testing allowing Space in all these.
	public bool GetRGripUp() { return (pgripR>=0.1f && gripR<0.1f) || InputUtils.Up(Key.Space); }
	public bool GetLGripDown() { return (pgripL<0.1f && gripL>=0.1f) || InputUtils.Down(Key.Space); }
	public bool GetRGripDown() { return (pgripR<0.1f && gripR>=0.1f) || InputUtils.Down(Key.Space); }
	public bool GetLGripHeld() { return (gripL>0.1f && pgripL>0.1f) || InputUtils.Held(Key.Space); } // NOTE: Held will deliberately NOT return true until the frame AFTER we've been pulled down!
	public bool GetRGripHeld() { return (gripR>0.1f && pgripR>0.1f) || InputUtils.Held(Key.Space); }

	public bool GetLTriggerUp()   { return ptriggerL>=0.1f && triggerL<0.1f; }
	public bool GetRTriggerUp()   { return pTriggerR>=0.1f && triggerR<0.1f; }
	public bool GetLTriggerDown() { return ptriggerL<0.1f &&  triggerL>=0.1f; }
	public bool GetRTriggerDown() { return pTriggerR<0.1f &&  triggerR>=0.1f; }
	public bool GetLTriggerHeld() { return  triggerL>0.1f && ptriggerL>0.1f; } // NOTE: Held will deliberately NOT return true until the frame AFTER we've been pulled down!
	public bool GetRTriggerHeld() { return  triggerR>0.1f && pTriggerR>0.1f; }


    //   private float timeWhenLastClickedThumbstickL = Mathf.NegativeInfinity; // in unscaled time.
    //private float timeWhenLastClickedThumbstickR = Mathf.NegativeInfinity; // in unscaled time.
    public bool isThumbstickLDoubleClick { get; private set; }
	public bool isThumbstickRDoubleClick { get; private set; }

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction buttonSouth;
	private InputAction buttonEast;
    private InputAction buttonWest;
    private InputAction buttonNorth;
	private InputAction pauseAction;
    private InputAction leftBumper;
    private InputAction rightBumper;
    private InputAction leftTrigger;
    private InputAction rightTrigger;
    private InputAction changeElfAction;


    // ----------------------------------------------------------------
    //  Instance / Awake
    // ----------------------------------------------------------------
    public static InputManager Instance; // There can only be one.
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
                InputManager[] allOthers = FindObjectsByType<InputManager>(FindObjectsSortMode.None);
                for (int i = 0; i < allOthers.Length; i++) {
                    if (allOthers[i] == this) { continue; } // Skip ourselves.
                    Destroy(allOthers[i].gameObject);
                }
            }
        }

        // There could only be one. :)
        Instance = this;
        //DontDestroyOnLoad(this.gameObject);

        // Set up input actions!
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddBinding("<Gamepad>/leftStick").WithProcessor("stickDeadzone(min=0.1,max=0.85)");
        moveAction.Enable();

		lookAction = new InputAction("Look", InputActionType.Value);//, "<Mouse>/delta");
        lookAction.AddBinding("<Gamepad>/rightStick").WithProcessor("stickDeadzone(min=0.1,max=0.85)");
        lookAction.Enable();

		buttonSouth = new InputAction("A", InputActionType.Button);
        buttonSouth.AddBinding("<Gamepad>/buttonSouth");
        buttonSouth.AddBinding("<Keyboard>/e");
        buttonSouth.Enable();
        buttonEast = new InputAction("B", InputActionType.Button);
        buttonEast.AddBinding("<Gamepad>/buttonEast");
        buttonEast.AddBinding("<Keyboard>/q"); // sure, why not. (this might annoy me and I'll cut it in the future.)
        buttonEast.Enable();
        buttonWest = new InputAction("Dash", InputActionType.Button);
        buttonWest.AddBinding("<Gamepad>/buttonWest");
        buttonWest.AddBinding("<Keyboard>/shift");
        buttonWest.Enable();
        buttonNorth = new InputAction("Jump", InputActionType.Button);
        buttonNorth.AddBinding("<Gamepad>/buttonNorth");
        buttonNorth.AddBinding("<Keyboard>/space");
        buttonNorth.Enable();

        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Gamepad>/start");
        pauseAction.AddBinding("<Keyboard>/p");
        pauseAction.Enable();

        changeElfAction = new InputAction("ChangeElf", InputActionType.Button);
        changeElfAction.AddBinding("<Gamepad>/select");
        changeElfAction.AddBinding("<Keyboard>/tab");
        changeElfAction.Enable();

        leftBumper = new InputAction("LeftBumper", InputActionType.Button);
        rightBumper = new InputAction("RightBumper", InputActionType.Button);
        leftBumper.AddBinding("<Gamepad>/leftShoulder");
        rightBumper.AddBinding("<Gamepad>/rightShoulder");
        leftBumper.Enable();
        rightBumper.Enable();

        leftTrigger = new InputAction("LeftTrigger", InputActionType.Value);
        rightTrigger = new InputAction("RightTrigger", InputActionType.Value);
        leftTrigger.AddBinding("<Gamepad>/leftTrigger").WithProcessor("axisDeadzone(min=0.2,max=0.9)");
        rightTrigger.AddBinding("<Gamepad>/rightTrigger").WithProcessor("axisDeadzone(min=0.2,max=0.9)");
        //rightTrigger.AddBinding("<Keyboard>/control").WithProcessor("axisDeadzone(min=0.2,max=0.9)"); <- this won't work. must be a .Button.
        leftTrigger.Enable();
        rightTrigger.Enable();

        // Right away, make sure the prev values match the current ones! So we don't get down-events or anything the first frame.
        UpdatePrevValuesToCurrent();
    }



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    private void Update() {
        UpdatePrevValuesToCurrent();

        joystickLAxis = moveAction.ReadValue<Vector2>();
        joystickRAxis = lookAction.ReadValue<Vector2>();

        triggerL = leftTrigger.ReadValue<float>();
        triggerR = rightTrigger.ReadValue<float>();
        // Hacky adding control button
        if (InputUtils.Held(Key.LeftCtrl)) triggerR = 1;
        isFastForwardHeld = InputUtils.Held(Key.F) || (LeftBumper>0.5f && RightBumper>0.5f);

        // Mouse
        isDoubleClickMouseL = false;
		if (InputUtils.MouseDown(0)) {
			if (Time.unscaledTime < timeLastClickedMouseL + 0.5f) {
				isDoubleClickMouseL = true;
			}
            timeLastClickedMouseL = Time.unscaledTime;
        }

		// NOTE: Disabled all the below until we import Oculus packages!
		/*
        // Joysticks
        joystickLAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);
		joystickRAxis = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        joystickCardinalLAxis = Mathf.Abs(joystickLAxis.x) > Mathf.Abs(joystickLAxis.y)
            ? new Vector2(joystickLAxis.x, 0)
            : new Vector2(0, joystickLAxis.y);
        joystickCardinalRAxis = Mathf.Abs(joystickRAxis.x) > Mathf.Abs(joystickRAxis.y)
            ? new Vector2(joystickRAxis.x, 0)
            : new Vector2(0, joystickRAxis.y);

        // Trigger and grip
		gripL = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.LTouch);
		gripR = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch);
		triggerL = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
		triggerR = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);

        // Double-clicking thumbsticks.
        isThumbstickLDoubleClick = false;
		isThumbstickRDoubleClick = false;
        if (GetButtonDown_ThumbstickL()) {
			if (Time.unscaledTime < timeWhenLastClickedThumbstickL + 0.5f) {
				isThumbstickLDoubleClick = true;
            }
            timeWhenLastClickedThumbstickL = Time.unscaledTime;
		}
		if (GetButtonDown_ThumbstickR()) {
            if (Time.unscaledTime < timeWhenLastClickedThumbstickR + 0.5f) {
                isThumbstickRDoubleClick = true;
            }
            timeWhenLastClickedThumbstickR = Time.unscaledTime;
        }
		*/
	}
	private void UpdatePrevValuesToCurrent() {
		pgripL = gripL;
		pgripR = gripR;
		ptriggerL = triggerL;
		pTriggerR = triggerR;
        pisFastForwardHeld = isFastForwardHeld;
    }


    // ----------------------------------------------------------------
    //  Getters
    // ----------------------------------------------------------------
 //   public bool GetButtonDown_Cancel() {
	//	if (InputUtils.GetKeyDown(Key.Escape)) return true;
	//	if (InputUtils.GetKeyDown(Key.Backspace) || InputUtils.GetKeyDown(Key.Delete)) return true;
 //       ////if (OVRInput.GetDown(OVRInput.Button.Back)) return true; // NOTE: I don't think this button exists...
 //       if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Four)) return true;
 //       return false;
	//}
	//public bool GetButtonDown_Submit() {
	//	if (InputUtils.GetKeyDown(Key.Space)) return true;
	//	if (InputUtils.GetKeyDown(Key.KeypadEnter) || InputUtils.GetKeyDown(Key.Return)) return true;
	//	if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Three)) return true;
	//	return false;
	//}
	//public bool GetButton_Submit() {
	//	if (InputUtils.GetKey(Key.Space)) return true;
	//	if (InputUtils.GetKey(Key.KeypadEnter) || InputUtils.GetKey(Key.Return)) return true;
	//	if (OVRInput.Get(OVRInput.Button.One) || OVRInput.Get(OVRInput.Button.Three)) return true;
	//	return false;
	//}
	//public bool GetButtonDown_Pause() {
	//	if (InputUtils.GetKeyDown(Key.Escape) || InputUtils.GetKeyDown(Key.P)) return true;
	//	if (OVRInput.GetDown(OVRInput.Button.Start)) return true;
	//	return false;
	//}

	//public bool GetButtonDown_ThumbstickL() { return OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick); }
	//public bool GetButtonDown_ThumbstickR() { return OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick); }
 //   public bool GetButtonUp_ThumbstickL() { return OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick); }
 //   public bool GetButtonUp_ThumbstickR() { return OVRInput.GetUp(OVRInput.Button.SecondaryThumbstick); }




}

