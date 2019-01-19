using GoatRock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordClipsOnLoudness : MonoBehaviour {

    public float MinBand5;
    public float MinBand4;
    public float MinBand3;
    public float MinBand2;
    [Space(5)]
    public GameObject RecordedObjectPrefab;
    [Space(5)]
    public KeyCode ToggleRecordingDebug = KeyCode.R;
    public AudioClip CreatedAudioClip;
    [Space(5)]
    public AudioSource ThisAudioSource;
    public string MicrophoneName;
    [Space(5)]
    public float AudioRecordingCooldown = 0.5f;

    private float TimeSinceLastActivation;

    private float SamplesOnRecordingStart;
    private float SamplesOnRecordingEnd;
    public bool IsRecording = false;

    #region Unity API

    private void Update()
    {
        if (Input.GetKeyDown(ToggleRecordingDebug))
        {
            ToggleRecording();
        }

        if(AudioReactMicSourceBehavior.Instance)
        {
            if (ShouldRecordingHappen())
            {
                if (!IsRecording)
                {
                    BeginCreatingAudioClip();
                }
                TimeSinceLastActivation = Time.time;
            }
            else
            {
                if (IsRecording)
                {
                    if (!ShouldRecordingHappen())
                    {
                        if (Time.time > AudioRecordingCooldown + TimeSinceLastActivation)
                        {
                            EndAudioClipCreation();
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Recording Methods

    public bool ShouldRecordingHappen()
    {
        if(AudioReactMicSourceBehavior.Instance.FreqBand[4] > MinBand4
                || AudioReactMicSourceBehavior.Instance.FreqBand[3] > MinBand3
                || AudioReactMicSourceBehavior.Instance.FreqBand[2] > MinBand2
                || AudioReactMicSourceBehavior.Instance.FreqBand[5] > MinBand5)
        {
            return true;
        }
        return false;
    }

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

        var AudioClipGameObject = Instantiate(RecordedObjectPrefab, Camera.main.transform.position, Quaternion.identity);
        var newAudioSource = AudioClipGameObject.GetComponent<AudioSource>();
        newAudioSource.clip = newClip;
        newAudioSource.playOnAwake = true;
        newAudioSource.loop = true;
        newAudioSource.Play();

        IsRecording = false;
    }
    
    #endregion

}
