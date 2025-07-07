using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Used for playing button sfx. So each button doesn't need its own AudioSource components.
/// </summary>
public class RogueSfxPlayer : MonoBehaviour {
    // Instance
    static private RogueSfxPlayer _instance;
    // Components
    List<AudioSource> audioSources = new List<AudioSource>(); // When we want to play a sound, we pick the first one in the list that ISN'T currently playing a sound.
    // References
    [SerializeField] private AudioClip ac_buttonDown;
    [SerializeField] private AudioClip ac_buttonUp;

    // Getters
    private AudioSource GetUnoccupiedAudioSource() {
        for (int i = 0; i<audioSources.Count; i++) {
            if (!audioSources[i].isPlaying) {
                return audioSources[i];
            }
        }
        if (audioSources.Count < 16) {
            AudioSource newSource = new GameObject().AddComponent<AudioSource>();
            audioSources.Add(newSource);
            newSource.name = "SfxAudioSource_" + audioSources.Count;
            newSource.transform.parent = this.transform;
            newSource.playOnAwake = false;
            newSource.spatialBlend = 1;
            //newSource.minDistance = 0.7f;
            //newSource.maxDistance = 5000;
            newSource.outputAudioMixerGroup = AudioMixerController.MixerGroup_Sfx;
            return newSource;
        }
        return null; // Optional safety check to ensure we don't add too many audio sources.
    }

    // Getters
    public static RogueSfxPlayer Instance { get { return _instance; } }


    // ----------------------------------------------------------------
    //  Awake
    // ----------------------------------------------------------------
    private void Awake() {
        // T... two??
        if (_instance != null) {
            // THERE CAN ONLY BE ONE.
            DestroyImmediate(this.gameObject);
            return;
        }
        // There's no instance already...
        else {
            if (Application.isEditor) { // In the UnityEditor?? Look for ALL duplicates of me! We'll get duplicates when we reload our scripts.
                RogueSfxPlayer[] allOthers = FindObjectsByType<RogueSfxPlayer>(FindObjectsSortMode.None);
                for (int i = 0; i < allOthers.Length; i++) {
                    if (allOthers[i] == this) { continue; } // Skip ourselves.
                    Destroy(allOthers[i].gameObject);
                }
            }
        }

        // There could only be one. :)
        _instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }



    // ----------------------------------------------------------------
    //  Play
    // ----------------------------------------------------------------
    public void PlaySound(AudioClip clip, Vector3 pos, float volume=1f, float pitch=1f) {
        AudioSource source = GetUnoccupiedAudioSource();
        if (source == null) { return; } // Safety check.
        source.transform.position = pos;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
    }

    public void PlayButtonDown(Vector3 buttonPos) {
        PlaySound(ac_buttonDown, buttonPos);
    }
    public void PlayButtonUp(Vector3 buttonPos) {
        PlaySound(ac_buttonUp, buttonPos);
    }




}