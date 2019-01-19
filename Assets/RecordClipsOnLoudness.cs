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
    public bool RecordingEnabled = true;
    public float MinimumAudioRecordingLength = 0.5f;
    public float MinimumTimeBetweenRecordings = 0.1f;
    [Space(5)]
    [Header("Color Settings")]
    public Color MinimumColor;
    public Color MaximumColor;
    [Space(5)]
    public Vector2 ColorLowFrequencyRange;
    public float ColorLowFrequencyRangeMultiplier;
    [Space(5)]
    public Vector2 ColorMediumFrequencyRange;
    public float ColorMediumFrequencyRangeMultiplier;
    [Space(5)]
    public Vector2 ColorHighFrequencyRange;
    public float ColorHighFrequencyRangeMultiplier;
    [Space(5)]
    public Vector2 ScaleRange;
    [Space(5)]
    public Color ColorOutput;
    public float ScaleMultiplier;

    private float TimeSinceLastActivation;
    private float TimeSinceLastRecordingEnd;

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
        
        float red = Helpers.ConvertLinearRange(AudioReactMicSourceBehavior.Instance.FreqBand[2] * ColorLowFrequencyRangeMultiplier, ColorLowFrequencyRange.x, ColorLowFrequencyRange.y, MinimumColor.r, MaximumColor.r);
        ColorOutput.r = red;

        float green = Helpers.ConvertLinearRange(AudioReactMicSourceBehavior.Instance.FreqBand[3] * ColorMediumFrequencyRangeMultiplier, ColorLowFrequencyRange.x, ColorLowFrequencyRange.y, MinimumColor.g, MaximumColor.g);
        ColorOutput.g = green;

        float blue = Helpers.ConvertLinearRange(AudioReactMicSourceBehavior.Instance.FreqBand[4] * ColorHighFrequencyRangeMultiplier, ColorLowFrequencyRange.x, ColorLowFrequencyRange.y, MinimumColor.b, MaximumColor.b);
        ColorOutput.b = blue;

        // ColorOutput.g = Helpers.ConvertLinearRange(MinBand3, ColorMediumFrequencyRange.x, ColorMediumFrequencyRange.y, 0, 1f);
        // ColorOutput.b = Helpers.ConvertLinearRange(MinBand4, ColorHighFrequencyRange.x, ColorHighFrequencyRange.y, 0, 1f);

        if (AudioReactMicSourceBehavior.Instance)
        {
            if(RecordingEnabled)
            {
                if (ShouldRecordingHappen())
                {
                    if (!IsRecording)
                    {
                        if (Time.time > MinimumTimeBetweenRecordings + TimeSinceLastRecordingEnd)
                        {
                            BeginCreatingAudioClip();
                        }
                    }
                    TimeSinceLastActivation = Time.time;
                }
                else
                {
                    if (IsRecording)
                    {
                        if (!ShouldRecordingHappen())
                        {
                            if (Time.time > MinimumAudioRecordingLength + TimeSinceLastActivation)
                            {
                                EndAudioClipCreation();
                                TimeSinceLastRecordingEnd = Time.time;
                            }
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
        if(AudioReactMicSourceBehavior.Instance.userIsTalking)
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
