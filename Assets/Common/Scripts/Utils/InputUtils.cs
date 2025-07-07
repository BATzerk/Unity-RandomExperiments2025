using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public static class InputUtils {
    public static bool Down(Key key) => Keyboard.current[key].wasPressedThisFrame;//UnityEditor.SceneView.lastActiveSceneView?.hasFocus==true ?  : false; // NOTE: This will ignore input in Editor when Scene window ISN'T focused.
    public static bool Up(Key key) => Keyboard.current[key].wasReleasedThisFrame;
    public static bool Held(Key key) => Keyboard.current[key].isPressed;
    // Mouse Input
    public static bool MouseDown(int button) => GetMouseButton(button)?.wasPressedThisFrame ?? false;
    public static bool MouseUp(int button) => GetMouseButton(button)?.wasReleasedThisFrame ?? false;
    public static bool MouseHeld(int button) => GetMouseButton(button)?.isPressed ?? false;

    public static Vector2 mousePos => Mouse.current.position.ReadValue();
    public static Vector3 mousePos3 => Mouse.current.position.ReadValue(); // just in case we need a Vector3 for math.
    public static Vector2 mouseScroll => Mouse.current.scroll.ReadValue();

    // Helper to get the correct mouse button
    private static ButtonControl GetMouseButton(int button) {
        return button switch {
            0 => Mouse.current.leftButton,
            1 => Mouse.current.rightButton,
            2 => Mouse.current.middleButton,
            _ => null
        };
    }
}