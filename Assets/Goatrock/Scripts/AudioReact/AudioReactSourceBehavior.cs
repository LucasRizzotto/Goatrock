using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GoatRock
{
    public class AudioReactSourceBehavior : MonoBehaviour
    {
        public bool ReactToMicrophoneInput = false;
        public bool ActivateAudioReactsOnStart = false;
        public AudioSource ThisAudioSource;
        public AudioReactBehavior[] AudioReactionTargets;
        
        public float Amplitude;
        public float AmplitudeBuffer;
        public float[] Samples = new float[512];
        public float[] FreqBand = new float[8];
        public float[] InstanceAudioBandBuffer = new float[8];
        public float[] FreqBandHighest = new float[8];
        public float[] AudioBand = new float[8];
        public float[] AudioBandBuffer = new float[8];

        private float TopAmplitude;
        private float[] decreaseBuffer = new float[8];

        private bool ActivateAudioReactSource = false;

        #region Unity API

        void Reset()
        {
            SetupVisualization();
        }

        public void Update()
        {
            if(ActivateAudioReactSource)
            {
                GetSpectrum();
                GenerateBands();
                BufferBands();
                GetAmplitude();
                GenerateAudioBands();
            }
        }

        void GetAudioReactionTargets()
        {
            AudioReactionTargets = GetComponents<AudioReactBehavior>();
        }

        /// <summary>
        /// Whenever this script is enabled, audio visualization starts
        /// </summary>
        public void Start()
        {
#if !UNITY_WEBGL
            if (ReactToMicrophoneInput)
            {
                ThisAudioSource.clip = Microphone.Start("", true, 300, 44100);
                ThisAudioSource.loop = true;
                ThisAudioSource.Play();
            }
#else
            Debug.LogWarning("Microphone not officially supported with WebGL");
#endif
            if (ActivateAudioReactsOnStart)
            {
                StartAllAudioVisualizations();
            }
        }

        #endregion

        #region Audio Data Collection

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

        #region AudioReact API

        /// <summary>
        /// Initial Setup of the Audio Visualization
        /// </summary>
        public void SetupVisualization()
        {
            GetAudioReactionTargets();
        }

        /// <summary>
        /// Enables Audio Visualization
        /// </summary>
        public void StartAllAudioVisualizations()
        {
            Debug.Log("AudioReactSource is attempting to activate all AudioReact targets...");
            for (int i = 0; i < AudioReactionTargets.Length; i++)
            {
                AudioReactionTargets[i].StartAudioVisualization();
            }
            ActivateAudioReactSource = true;
        }

        /// <summary>
        /// Stops Audio Visualization
        /// </summary>
        public void StopAllAudioVisualizations()
        {
            for (int i = 0; i < AudioReactionTargets.Length; i++)
            {
                AudioReactionTargets[i].StopAudioVisualization();
            }
            ActivateAudioReactSource = false;
        }

        #endregion

    }
}