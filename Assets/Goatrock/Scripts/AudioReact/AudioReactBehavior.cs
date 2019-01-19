using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public abstract class AudioReactBehavior : MonoBehaviour, IAudioReact
    {
        public bool VisualizationBehavior = false; // Is the visualization enabled?
        public bool SetupOnStart = true;
        public bool StartOnStart = true;
        [Space(5)]
        public AudioReactSourceBehavior ReferenceAudioReactSource; // What's the source of the visualization?
        public AudioReactMicSourceBehavior ReferenceAudioReactSourceMic; // What's the source of the visualization?

        protected bool Ready = false; // Has lazy loading been completed?

        #region Unity APIs

        protected virtual void Start()
        {
            if(SetupOnStart)
            {
                SetupVisualization();
            }

            if(StartOnStart)
            {
                StartAudioVisualization();
            }
        }

        protected virtual void Reset()
        {
            SetupVisualization();
        }
        
        protected void LateUpdate()
        {
            if (VisualizationBehavior)
            {
                UpdateAudioVisualization();
            }
        }

        protected void OnDisable()
        {
            if(VisualizationBehavior)
            {
                StopAudioVisualization();
            }
        }

        #endregion

        #region Audio React APIs

        /// <summary>
        /// Initial Setup of the Audio Visualization
        /// </summary>
        public virtual void SetupVisualization() {
            if (ReferenceAudioReactSource == null)
            {
                try
                {
                    if(GetComponent<AudioReactSourceBehavior>() != null)
                    {
                        ReferenceAudioReactSource = GetComponent<AudioReactSourceBehavior>();
                    }

                    if(GetComponent<AudioReactMicSourceBehavior>() != null)
                    {
                        ReferenceAudioReactSourceMic = GetComponent<AudioReactMicSourceBehavior>();
                    }
                }
                catch
                {
                    Debug.LogError("No Reference AudioReactSource set");
                }
            }
            if (GetComponent<AudioReactSourceBehavior>() != null)
            {
                ReferenceAudioReactSource = GetComponent<AudioReactSourceBehavior>();
            }

            if (GetComponent<AudioReactMicSourceBehavior>() != null)
            {
                ReferenceAudioReactSourceMic = GetComponent<AudioReactMicSourceBehavior>();
            }
        }

        /// <summary>
        /// Enables Audio Visualization
        /// </summary>
        public virtual void StartAudioVisualization() {
            if (!VisualizationBehavior)
            {
                VisualizationBehavior = true;
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Stops Audio Visualization
        /// </summary>
        public virtual void StopAudioVisualization() {
            if(VisualizationBehavior)
            {
                VisualizationBehavior = false;
            }
            else
            {
                Debug.LogWarning("Can't stop audio visualization. Nothing is ongoing");
                return;
            }
        }

        /// <summary>
        /// Updates the audio visualization once per frame
        /// </summary>
        public virtual void UpdateAudioVisualization() {
            if(!VisualizationBehavior)
            {
                return;
            }
        }

        #endregion

    }

}