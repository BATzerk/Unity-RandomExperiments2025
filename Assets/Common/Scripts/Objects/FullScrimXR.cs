using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullScrimXR : MonoBehaviour {
    // Components
    [SerializeField] private SpriteRenderer sr_scrim = null;
    [SerializeField] private TextMeshPro t_pleaseWait;
    // Properties
    private bool useUnscaledTime; // if this is true, I'll fade IRRELEVANT of Time.timeScale
    private Color startColor;
    private Color endColor;
    private float fadeDuration; // in SECONDS, how long it'll take to get from startColor to endColor.
    private float timeUntilFinishFade = -1; // fadeDuration, but counts down.

    // Getters
    public bool IsFading { get { return timeUntilFinishFade >= 0; } }


    // ================================================================
    //	Awake / Destroy
    // ================================================================
    private void Awake() {
        Hide();
        t_pleaseWait.sortingOrder = 1001; // just above the scrim.
        // Add event listeners!
        EventBus.Instance.FullScrim_FadeFromAToBEvent += FadeFromAtoB;
        EventBus.Instance.FullScrim_HideEvent += Hide;
        EventBus.Instance.FullScrim_ShowEvent += Show;
        EventBus.Instance.FullScrim_HidePleaseWaitTextEvent += HidePleaseWaitText;
        EventBus.Instance.FullScrim_ShowPleaseWaitTextEvent += ShowPleaseWaitText;
    }
    private void OnDestroy() {
        // Remove event listeners!
        EventBus.Instance.FullScrim_FadeFromAToBEvent -= FadeFromAtoB;
        EventBus.Instance.FullScrim_HideEvent -= Hide;
        EventBus.Instance.FullScrim_ShowEvent -= Show;
        EventBus.Instance.FullScrim_HidePleaseWaitTextEvent -= HidePleaseWaitText;
        EventBus.Instance.FullScrim_ShowPleaseWaitTextEvent -= ShowPleaseWaitText;
    }


    // ================================================================
    //	Update
    // ================================================================
    private void Update() {
        // If I'm visible AND fading colors!...
        if (sr_scrim.enabled && timeUntilFinishFade>0) {
            // Update timeUntilFinishFade!
            timeUntilFinishFade -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            // Update color!
            sr_scrim.color = Color.Lerp(startColor, endColor, 1-timeUntilFinishFade/fadeDuration);
            // DONE fading??
            if (timeUntilFinishFade <= 0) {
                timeUntilFinishFade = -1;
                // Faded to totally clear?? Then go ahead and HIDE the image completely. :)
                if (endColor.a == 0) {
                    Hide();
                }
            }
        }
    }


    // ================================================================
    //	Doers
    // ================================================================
    private void HidePleaseWaitText() { t_pleaseWait.enabled = false; }
    private void ShowPleaseWaitText() { t_pleaseWait.enabled = true; }
    public void Show(float blackAlpha) {
        Show(Color.black, blackAlpha, false);
    }
    /** useHighestAlpha: If TRUE, then I'll only set my alpha to something HIGHER than it already is. */
    public void Show(Color color, float alpha, bool useHighestAlpha = true) {
        sr_scrim.enabled = true;
        if (useHighestAlpha) {
            alpha = Mathf.Max(sr_scrim.color.a, alpha);
        }
        sr_scrim.color = new Color(color.r, color.g, color.b, alpha);
    }
    public void Hide() {
        sr_scrim.enabled = false;
        t_pleaseWait.enabled = false;
        timeUntilFinishFade = -1;
    }

    public void FadeFromAtoB(Color _startColor, Color _endColor, float _fadeDuration, bool _useUnscaledTime, bool _doOverrideCurrFade) {
        if (!_doOverrideCurrFade && IsFading) return; // Don't do anything if I'm already fading, and NOT supposed to override any current fading.
        startColor = _startColor;
        endColor = _endColor;
        fadeDuration = timeUntilFinishFade = _fadeDuration;
        useUnscaledTime = _useUnscaledTime;
        // Prep sr_scrim!
        sr_scrim.color = startColor;
        sr_scrim.enabled = true;
    }
    ///** This fades from exactly where we ARE to a target color. */
    //public void FadeToB (Color _endColor, float _fadeDuration, bool _useUnscaledTime) {
    //	FadeFromAtoB (sr_scrim.color, _endColor, _fadeDuration, _useUnscaledTime);
    //}


}




