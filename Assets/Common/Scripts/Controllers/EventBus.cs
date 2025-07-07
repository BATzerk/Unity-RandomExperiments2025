using UnityEngine;

public class EventBus {
	// Common
	public delegate void NoParamAction ();
	public delegate void BoolAction (bool _bool);
	public delegate void BoolBoolAction(bool b1, bool b2);
	public delegate void BoolFloatAction (bool _bool, float _float);
	public delegate void FloatBoolAction(float _float, bool _bool);
	public delegate void FloatAction (float _float);
	public delegate void FloatFloatAction (float _float1, float _float2);
	public delegate void FloatIntIntAction (float _float, int _int1, int _int2);
	public delegate void IntAction (int _int);
	public delegate void IntBoolAction (int _int, bool _bool);
	public delegate void IntIntAction (int _int1, int _int2);
	public delegate void IntIntIntAction (int _int1, int _int2, int _int3);
	public delegate void ColorFloatBoolAction(Color c, float f, bool b);
	public delegate void ColorColorFloatBoolBoolAction(Color c1, Color c2, float f, bool b1, bool b2);
    public delegate void Vector3Action(Vector3 vector);

    public event BoolAction SetIsPausedEvent;
    public event NoParamAction FullScrim_HideEvent;
	public event NoParamAction FullScrim_HidePleaseWaitTextEvent;
	public event NoParamAction FullScrim_ShowPleaseWaitTextEvent;
	public event ColorColorFloatBoolBoolAction FullScrim_FadeFromAToBEvent;
	public event ColorFloatBoolAction FullScrim_ShowEvent;

    public void OnSetIsPaused(bool isPaused) { SetIsPausedEvent?.Invoke(isPaused); }
    public void FullScrim_Hide() { FullScrim_HideEvent?.Invoke(); }
	public void FullScrim_Show(Color color, float alpha, bool useHighestAlpha=true) { FullScrim_ShowEvent?.Invoke(color, alpha, useHighestAlpha); }
	public void FullScrim_HidePleaseWaitText() { FullScrim_HidePleaseWaitTextEvent?.Invoke(); }
	public void FullScrim_ShowPleaseWaitText() { FullScrim_ShowPleaseWaitTextEvent?.Invoke(); }
	public void FullScrim_FadeFromAtoB(Color startColor, Color endColor, float duration, bool useUnscaledTime, bool doOverrideCurrFade) { FullScrim_FadeFromAToBEvent?.Invoke(startColor,endColor,duration,useUnscaledTime,doOverrideCurrFade); }

    public event NoParamAction SetIsPassthroughEvent;
    public void OnSetIsPassthrough() { SetIsPassthroughEvent?.Invoke(); }




    // ----------------------------------------------------------------
    //  Instance
    // ----------------------------------------------------------------
    public static bool isInitializing { get; private set; } = false;
    static private EventBus instance;
    static public EventBus Instance {
        get {
            if (instance==null) {
                // We're ALREADY initializing?? Uh-oh. Return null, or we'll be caught in an infinite loop of recursion!
                if (isInitializing) {
                    Debug.LogError("EventManager access loop infinite recursion error! It's trying to access itself before it's done being initialized.");
                    return null; // So the program doesn't freeze.
                }
                else {
                    isInitializing = true;
                    instance = new EventBus();
                }
            }
            else {
                isInitializing = false; // Don't HAVE to update this value at all, but it's nice to for accuracy.
            }
            return instance;
        }
    }
}




