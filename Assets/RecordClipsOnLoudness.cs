using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordClipsOnLoudness : MonoBehaviour {

    public float MinBand4;
    public float MinBand3;
    [Space(5)]
    public AudioClip CreatedAudioClip;
    public KeyCode ToggleRecordingDebug = KeyCode.R;
    public AudioSource ThisAudioSource;
    public float MaxClipLength;
    public string MicrophoneName;
    [Space(10)]
    public float SamplesOnRecordingStart;
    public float SamplesOnRecordingEnd;

    private bool IsRecording = false;

    private void Update()
    {
        if (Input.GetKeyDown(ToggleRecordingDebug))
        {
            ToggleRecording();
        }
    }

    #region Recording Methods


    public void ToggleRecording()
    {
        if (IsRecording)
        {
            EndAudioClipCreation();
        }
        else
        {
            BeginCreatingAudioClip();
        }
    }

    public void BeginCreatingAudioClip()
    {
        Debug.Log("Began recording");
        SamplesOnRecordingStart = Microphone.GetPosition(MicrophoneName);
        IsRecording = true;
    }

    public void EndAudioClipCreation()
    {
        SamplesOnRecordingEnd = Microphone.GetPosition(MicrophoneName);

        Debug.Log("Recording sample size: " + ((int)SamplesOnRecordingEnd - (int)SamplesOnRecordingStart));
        Debug.Log("Initial sample size: " + (ThisAudioSource.clip.samples));

        float[] samples = new float[(int)SamplesOnRecordingEnd - (int)SamplesOnRecordingStart];
        ThisAudioSource.clip.GetData(samples, (int)SamplesOnRecordingStart);

        var newClip = AudioClip.Create("Name", samples.Length, 1, AudioSettings.outputSampleRate, false);
        newClip.SetData(samples, 0);
        CreatedAudioClip = newClip;

        var newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = newClip;
        newAudioSource.playOnAwake = true;
        newAudioSource.loop = true;
        newAudioSource.Play();

        IsRecording = false;
    }

    #endregion

}
