using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    [RequireComponent(typeof(ParticleSystem))]
    public class AudioReactParticleStreamBehavior : AudioReactBehavior
    {
        public ParticleSystem ThisParticleSystem;

        [Header("What Band Number?")]
        [Range(-1, 7)]
        public int BandNumber;

        [Header("Scale Settings")]
        [Range(1f, 100000f)]
        public float IntensityMultiplier = 1f;
        public float MaximumEmissionAtAnyGivenFrame = 5f;

        protected override void Reset()
        {
            base.Reset();
            ThisParticleSystem = GetComponent<ParticleSystem>();
        }

        #region Audio React APIs

        public override void StartAudioVisualization()
        {
            base.StartAudioVisualization();
            ThisParticleSystem.Play();
        }

        public override void StopAudioVisualization()
        {
            base.StopAudioVisualization();
            ThisParticleSystem.Stop();
        }

        public override void UpdateAudioVisualization()
        {
            base.UpdateAudioVisualization();
            float EmissionRate = IntensityMultiplier * ReferenceAudioReactSource.GetBandData(BandNumber);
            
            if(EmissionRate > MaximumEmissionAtAnyGivenFrame)
            {
                EmissionRate = MaximumEmissionAtAnyGivenFrame;
            }

            ThisParticleSystem.emissionRate = EmissionRate;
        }

        #endregion

    }
}   
