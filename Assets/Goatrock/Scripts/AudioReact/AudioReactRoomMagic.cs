using System.Collections;
using UnityEngine;

namespace GoatRock
{
    public class AudioReactRoomMagic : AudioReactBehavior
    {
        public RoomMagic roomMagic;
        [Space(5)]
        public SOAudioReactRoomMagic Settings;
        [HideInInspector]
        public Vector3 initialScale;

        protected float MaxScale;

        #region Audio React APIs

        public override void SetupVisualization()
        {
            base.SetupVisualization();
        }

        public override void StartAudioVisualization()
        {
            base.StartAudioVisualization();
        }

        public override void UpdateAudioVisualization()
        {
            base.UpdateAudioVisualization();

            float bandDataMultiplier = 0f;
            if(ReferenceAudioReactSource == null)
            {
                if(ReferenceAudioReactSourceMic != null)
                {
                    bandDataMultiplier = ReferenceAudioReactSourceMic.GetBandData(Settings.BandNumber);
                }
            }
            else
            {
                bandDataMultiplier = ReferenceAudioReactSource.GetBandData(Settings.BandNumber);
            }

            bandDataMultiplier = Mathf.Pow(bandDataMultiplier, Settings.IntensityPower);
            bandDataMultiplier *= Settings.IntensityMultiplier;

            roomMagic.micVolume = Helpers.ConvertLinearRange(bandDataMultiplier, Settings.MinVolumeNumber, Settings.MaxVolumeNumber, 0f, 1f);
        }

        public override void StopAudioVisualization()
        {
            base.StopAudioVisualization();
        }

        #endregion

    }
}