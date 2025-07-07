using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put one of these A) In a level that precedes the audio clips you wanna load, or B) In a level where the audio clip isn't needed for at least 10 seconds.
/// </summary>
public class AsyncAudioPreloader : MonoBehaviour
{
    // References
    [SerializeField] private AudioClip[] clipsToLoad;


    private IEnumerator Start() {
        yield return new WaitForSecondsRealtime(2f); // Just in case?
        
        //Stopwatch stopwatch = new Stopwatch();
        //stopwatch.Start();

        // Load the clips asynchronously!
        for (int i=0; i<clipsToLoad.Length; i++) {
            AudioClip clip = clipsToLoad[i];
            if (clip != null) {
                if (!clip.loadInBackground)
                    UnityEngine.Debug.LogError("Hey, AsyncAudioPreloader has a clip that isn't set to loadInBackground! That flag needs to be true. Clip: " + clip.name);
                else
                    clip.LoadAudioData();
                //yield return StartCoroutine(IE_LoadClip(clip));
            }
        }

        //stopwatch.Stop();
        //UnityEngine.Debug.Log("AsyncAudioPreloader loaded " + clipsToLoad.Length + " clips in " + stopwatch.ElapsedMilliseconds + "ms.");
    }
    //private IEnumerator IE_LoadClip(AudioClip clip) {
    //    ResourceRequest request = Resources.LoadAsync<AudioClip>(clip.name);
    //    yield return request;
    //}
}
