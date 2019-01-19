using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaybackMicrophone : MonoBehaviour {

    public AudioSource ThisAudioSource;
    public string MicrophoneName;
    [Space(10)]
    public KeyCode ToggleRecordingDebug = KeyCode.R;
    public AudioClip CreatedAudioClip;

    [Space(10)]
    public float SamplesOnRecordingStart;
    public float SamplesOnRecordingEnd;

    private bool IsRecording = false;

    #region Unity API

    private void Start()
    {
        if(Microphone.devices.Length > 0)
        {
            MicrophoneName = Microphone.devices[0].ToString();
            ThisAudioSource.clip = Microphone.Start(MicrophoneName, true, 300, AudioSettings.outputSampleRate);
            ThisAudioSource.Play();
            Debug.Log("Getting Mic Input Bois. Playing? " + ThisAudioSource.isPlaying);
        }
    }

    #endregion
}
