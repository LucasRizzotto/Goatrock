using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaybackMicrophone : MonoBehaviour {

    public AudioSource ThisAudioSource;
    public string MicrophoneName;

    private void Start()
    {
        if(Microphone.devices.Length > 0)
        {
            MicrophoneName = Microphone.devices[0].ToString();
            ThisAudioSource.clip = Microphone.Start(MicrophoneName, true, 10, AudioSettings.outputSampleRate);
            ThisAudioSource.Play();
            Debug.Log("Getting Mic Input Bois. Playing? " + ThisAudioSource.isPlaying);
        }
    }

}
