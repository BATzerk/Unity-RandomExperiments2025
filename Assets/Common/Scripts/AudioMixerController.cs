using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour {
    // Instance
    static private AudioMixerController _instance;
    // Static Refs
    static public AudioMixerGroup MixerGroup_Music { get; private set; }
    static public AudioMixerGroup MixerGroup_Sfx { get; private set; }
    static public AudioMixerGroup MixerGroup_Voice { get; private set; }
    // Properties
    public float MusicVolume { get; private set; } // 0-1
    public float SfxVolume   { get; private set; } // 0-1
    public float VoiceVolume { get; private set; } // 0-1
    // References
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private AudioMixerGroup mixerGroup_Music;
    [SerializeField] private AudioMixerGroup mixerGroup_Sfx;
    [SerializeField] private AudioMixerGroup mixerGroup_Voice;
    // Music Ducking
    private const float MusicDuckWhileCharTalking = 0.4f;
    private float timeLastCharacterTalkingPing; // Time.unscaledTime. When this is greater than about 0.5 seconds, we'll unduck back in.
    private float timeLastMusicSilentPing;
    private float musicDuck = 1; // how much volume to duck by. 1 is no ducking. 0 is full ducking.


    // Getters / Setters
    public static AudioMixerController Instance { get { return _instance; } }
    public bool IsMusicMuted {
        get {
            return MusicVolume <= 0.0001f;
        }
        private set {
            MusicVolume = value ? 0 : 1;
        }
    }
    //public float MusicVolume {
    //    get { return GetVolumeVal("MusicVolume"); }
    //    set { SetVolumeVal("MusicVolume", value); }
    //}
    //public float SfxVolume {
    //    get { return GetVolumeVal("SfxVolume"); }
    //    set { SetVolumeVal("SfxVolume", value); }
    //}
    //public float VoiceVolume {
    //    get { return GetVolumeVal("VoiceVolume"); }
    //    set { SetVolumeVal("VoiceVolume", value); }
    //}

    private float GetVolumeVal(string mixerParamName) {
        float val;
        masterMixer.GetFloat(mixerParamName, out val);
        return Mathf.Pow(10, val / 20); // Un-log10 it!
    }
    private void SetVolumeVal(string mixerParamName, float value) {
        value = Mathf.Clamp(value, 0.00001f, 1); // Note: 0 would give -Infinity, which the Mixer can't handle. This'll just give us a inaudibly quiet volume.
        masterMixer.SetFloat(mixerParamName, Mathf.Log10(value) * 20);
    }

    private void ApplyMusicVolume() {
        SetVolumeVal("MusicVolume", MusicVolume * musicDuck);
    }
    private void ApplySfxVolume() {
        SetVolumeVal("SfxVolume", SfxVolume);
    }
    private void ApplyVoiceVolume() {
        SetVolumeVal("VoiceVolume", VoiceVolume);
    }

    public void SetMusicVolume(float val) {
        MusicVolume = val;
        SaveStorage.SetFloat(SaveKeys.MusicVolume, val);
        ApplyMusicVolume();
    }
    public void SetSfxVolume(float val) {
        SfxVolume = val;
        SaveStorage.SetFloat(SaveKeys.SfxVolume, val);
        ApplySfxVolume();
    }
    public void SetVoiceVolume(float val) {
        VoiceVolume = val;
        SaveStorage.SetFloat(SaveKeys.VoiceVolume, val);
        ApplyVoiceVolume();
    }



    // ----------------------------------------------------------------
    //  Awake / Destroy
    // ----------------------------------------------------------------
    void Awake() {
        // T... two??
        if (_instance != null) {
            // THERE CAN ONLY BE ONE.
            DestroyImmediate(this.gameObject);
            return;
        }
        // There's no instance already...
        else {
            if (Application.isEditor) { // In the UnityEditor?? Look for ALL duplicates of me! We'll get duplicates when we reload our scripts.
                AudioMixerController[] allOthers = FindObjectsByType<AudioMixerController>(FindObjectsSortMode.None);
                for (int i = 0; i < allOthers.Length; i++) {
                    if (allOthers[i] == this) { continue; } // Skip ourselves.
                    Destroy(allOthers[i].gameObject);
                }
            }
        }

        // There could only be one. :)
        _instance = this;
        //DontDestroyOnLoad(this.gameObject);

        // Assign static refs.
        MixerGroup_Music = mixerGroup_Music;
        MixerGroup_Sfx = mixerGroup_Sfx;
        MixerGroup_Voice = mixerGroup_Voice;

        SetMusicVolume(0.4f);// SaveStorage.GetFloat(SaveKeys.MusicVolume, 0.8f));
        SetSfxVolume(SaveStorage.GetFloat(SaveKeys.SfxVolume, 0.8f));
        SetVoiceVolume(SaveStorage.GetFloat(SaveKeys.VoiceVolume, 0.8f));

        // Add event listeners!
        //EventBus.Instance.CharacterStillTalkingEvent += OnCharacterStillTalking;
        //EventBus.Instance.SongDuckToSilentPingEvent += OnSongDuckToSilentPing;
    }
    private void OnDestroy() {
        // Remove event listeners!
        //EventBus.Instance.CharacterStillTalkingEvent -= OnCharacterStillTalking;
        //EventBus.Instance.SongDuckToSilentPingEvent -= OnSongDuckToSilentPing;
    }


    // ----------------------------------------------------------------
    //  Ping Events
    // ----------------------------------------------------------------
    //private void OnCharacterStillTalking() {
    //    timeLastCharacterTalkingPing = Time.unscaledTime;
    //}
    //private void OnSongDuckToSilentPing() {
    //    timeLastMusicSilentPing = Time.unscaledTime;
    //}



    // ----------------------------------------------------------------
    //  Update
    // ----------------------------------------------------------------
    void Update() {
        // -- MUSIC Ducking --
        // Duck to silent?
        if (Time.unscaledTime < timeLastMusicSilentPing + 0.6f) { // HARDCODED time threshold.
            musicDuck = Mathf.MoveTowards(musicDuck, 0, 1.6f*Time.unscaledDeltaTime);
        }
        //// Duck to talking?
        //else if (Time.unscaledTime < timeLastCharacterTalkingPing + TalkingCharacter.StillTalkingPingFrequency+0.1f) {
        //    musicDuck = Mathf.MoveTowards(musicDuck, MusicDuckWhileCharTalking, 1.4f*Time.unscaledDeltaTime);
        //}
        // Duck if we've been talking recently.
        else {
            musicDuck = Mathf.MoveTowards(musicDuck, 1, 0.4f*Time.unscaledDeltaTime);
        }

        ApplyMusicVolume();
    }


}
