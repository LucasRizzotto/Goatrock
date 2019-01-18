using System.Collections;
using UnityEngine;

namespace GoatRock
{
    public class AudioReactScaleBehavior : AudioReactBehavior
    {
        public Transform ScaleTarget;
        [Space(5)]
        public SOAudioReactScaleSettings Settings;
        [HideInInspector]
        public Vector3 initialScale;

        protected float MaxScale;

		#region Audio React APIs

		public override void SetupVisualization()
        {
            base.SetupVisualization();
            if (ScaleTarget == null)
            {
                ScaleTarget = transform;
            }
            initialScale = ScaleTarget.transform.localScale;
            MaxScale = (initialScale.x) + (initialScale.x * Settings.MaxScalePercentageIncrease);
        }

        public override void StartAudioVisualization()
        {
            base.StartAudioVisualization();
            if (initialScale.x > MaxScale)
            {
                VisualizationBehavior = false;
            }
        }

        public override void UpdateAudioVisualization()
        {
            base.UpdateAudioVisualization();

            float bandDataMultiplier = ReferenceAudioReactSource.GetBandData(Settings.BandNumber);

            ScaleTarget.localScale = new Vector3(
                ReturnTargetScale(Settings.ScaleX, initialScale.x, bandDataMultiplier, Settings.IntensityMultiplier, MaxScale),
                ReturnTargetScale(Settings.ScaleY, initialScale.y, bandDataMultiplier, Settings.IntensityMultiplier, MaxScale),
                ReturnTargetScale(Settings.ScaleZ, initialScale.z, bandDataMultiplier, Settings.IntensityMultiplier, MaxScale)
            );
        }

        float ReturnTargetScale(bool activeAxis, float initialScale, float bandMultiplier, float intensityMultiplier, float maxScale)
        {
            if(!activeAxis)
            {
                return initialScale;
            }

            float totalMultiplier = Mathf.Log10(bandMultiplier * Settings.IntensityMultiplier);
            if(totalMultiplier < 0)
            {
                totalMultiplier = 0;
            }

            float finalValue = initialScale * (1 + totalMultiplier);

            if (finalValue > maxScale)
            {
                return maxScale;
            }
            else
            {
                return finalValue;
            }
            
        }

        public override void StopAudioVisualization()
        {
            base.StopAudioVisualization();
            ScaleTarget.localScale = initialScale;
        }

        #endregion

    }
}