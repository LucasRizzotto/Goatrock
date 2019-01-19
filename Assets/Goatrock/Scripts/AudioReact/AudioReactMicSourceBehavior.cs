using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace GoatRock
{
    public class AudioReactMicSourceBehavior : Singleton<AudioReactMicSourceBehavior>
    {
        public bool ReactToMicrophoneInput = true;
        [Space(10)]
        public AudioSource ThisAudioSource;
        public AudioMixerGroup MixerGroupMicrophone;
        [Space(5)]
        public string SelectedMicrophone = "";

        public float Amplitude;
        public float AmplitudeBuffer;
        public float[] Samples = new float[512];
        public float[] FreqBand = new float[8];
        public float[] InstanceAudioBandBuffer = new float[8];
        public float[] FreqBandHighest = new float[8];
        public float[] AudioBand = new float[8];
        public float[] AudioBandBuffer = new float[8];

        public float TimeBetweenMeans = 0.025f;
        public float TimeBetweenCalculations = 0.3f;

        //____________________MY ADDITIONS -- NOT POLISHED_______________________________________
        public float averageMeans = 0;
        private float grandtotalMeans = 0;
        private int count = 0;

        private float talkMean = 0.1f;
        private float noiseMean = 0.03f;

        [Serializable]
        public class TalkingEvent : UnityEvent { }

        public TalkingEvent OnTalkingStart;
        public TalkingEvent OnTalkingEnd;

        public bool userIsTalking = false;
        //________________________________________________________________________________________

        private float TopAmplitude;
        private float[] decreaseBuffer = new float[8];

        private bool ActivateAudioReactSource = false;

        private float MinMean;
        private float MaxMean;

        #region Unity API

        public void Update()
        {
            //______________________________________________________________________________
            //______________________________________________________________________________

            if (Input.GetKeyDown(KeyCode.Space))
            {
                ResetMean();
            }

            if (ActivateAudioReactSource)
            {
                GetSpectrum();
                GenerateBands();
                BufferBands();
                GetAmplitude();
                GenerateAudioBands();
            }


        }

        /// <summary>
        /// Whenever this script is enabled, audio visualization starts
        /// </summary>
        public void Start()
        {
#if !UNITY_WEBGL
            if (Microphone.devices.Length > 0)
            {
                ActivateAudioReactSource = true;
                SelectedMicrophone = Microphone.devices[0].ToString();
                ThisAudioSource.clip = Microphone.Start(SelectedMicrophone, true, 10, AudioSettings.outputSampleRate);
                ThisAudioSource.outputAudioMixerGroup = MixerGroupMicrophone;
                ThisAudioSource.Play();
                Debug.Log("Getting Mic Input Bois. Playing? " + ThisAudioSource.isPlaying);
                StartCoroutine(RecalibrateMeans());
            }

#else
            Debug.LogWarning("Microphone not officially supported with WebGL");
#endif 
        }

        #endregion

        #region Audio Data Collection

        //______________________________________________________________________________

        public float AverageMeanOfSection = 0f;

        public IEnumerator RecalibrateMeans()
        {
            while (true)
            {
                float TimeAtBeginning = Time.time;
                float tempMean;

                grandtotalMeans = 0;
                count = 0;
                averageMeans = 0;

                while (Time.time < TimeAtBeginning + TimeBetweenCalculations)
                {
                    tempMean = GetMean();
                    grandtotalMeans += tempMean;
                    count++;
                    averageMeans = grandtotalMeans / count;

                    yield return new WaitForSeconds(TimeBetweenMeans);
                }

                AverageMeanOfSection = grandtotalMeans / count;

                Debug.Log("Finished Time Between Means");
                DetectTalking();
            }
        }

        public void DetectTalking()
        {
            float sampledMean = AverageMeanOfSection;

            if (sampledMean > talkMean)
            {
                userIsTalking = true;
                OnTalkingStart.Invoke();
            }
            if (sampledMean < noiseMean)
            {
                userIsTalking = false;
                OnTalkingEnd.Invoke();
            }

        }
        //______________________________________________________________________________


        public void ResetMean()
        {
            MinMean = -1f;
            MaxMean = -1f;
        }

        public float GetMean()
        {
            float Total = 0f;
            for (int i = 0; i < Samples.Length; i++)
            {
                Total += Samples[i];
            }

            float Mean = (Total / Samples.Length) * 200f;

            if (Mean > MaxMean)
            {
                MaxMean = Mean;
                Debug.Log("New MAX Mean: " + MaxMean);
            }

            if (MinMean <= 0)
            {
                MinMean = Mean;
            }

            if (Mean < MinMean)
            {
                MinMean = Mean;
                Debug.Log("New MIN " + MinMean);
            }

            return Mean;
        }

        private void GetSpectrum()
        {
            ThisAudioSource.GetSpectrumData(Samples, 0, FFTWindow.Blackman);
        }

        private void GenerateAudioBands()
        {
            for (int i = 0; i < 8; i++)
            {
                if (FreqBand[i] > FreqBandHighest[i])
                {
                    FreqBandHighest[i] = FreqBand[i];
                }
                AudioBand[i] = (FreqBand[i] / FreqBandHighest[i]);
                AudioBandBuffer[i] = (InstanceAudioBandBuffer[i] / FreqBandHighest[i]);
            }
        }

        private void BufferBands()
        {
            for (int g = 0; g < 8; ++g)
            {
                if (FreqBand[g] > InstanceAudioBandBuffer[g])
                {
                    InstanceAudioBandBuffer[g] = FreqBand[g];
                }

                if (FreqBand[g] < InstanceAudioBandBuffer[g])
                {
                    InstanceAudioBandBuffer[g] -= decreaseBuffer[g];
                    decreaseBuffer[g] *= 1.2f;
                }
            }
        }

        private void GenerateBands()
        {
            int count = 0;

            for (int i = 0; i < 8; i++)
            {
                float average = 0;
                int sampleCount = (int)Mathf.Pow(2, i) * 2;

                if (i == 7)
                {
                    sampleCount += 2;
                }

                for (int j = 0; j < sampleCount; j++)
                {
                    average += Samples[count] * (count * 1);
                    count++;
                }

                average /= count;
                FreqBand[i] = average * 10;
            }
        }

        private void GetAmplitude()
        {
            float currentAmplitude = 0;
            float currentAmplitudeBuffer = 0;

            for (int i = 0; i < 8; i++)
            {
                currentAmplitude += AudioBand[i];
                currentAmplitudeBuffer += AudioBandBuffer[i];
            }

            if (currentAmplitude > TopAmplitude)
            {
                TopAmplitude = currentAmplitude;
            }

            Amplitude = currentAmplitude / TopAmplitude;
            AmplitudeBuffer = currentAmplitudeBuffer / TopAmplitude;
        }

        public float GetBandData(int bandNumber, bool useBuffer = false)
        {
            if (bandNumber == -1)
            {
                return Amplitude;
            }

            if (bandNumber < 8)
            {
                return useBuffer ? InstanceAudioBandBuffer[bandNumber] : FreqBand[bandNumber];
            }

            return 0;
        }

        #endregion

    }
}