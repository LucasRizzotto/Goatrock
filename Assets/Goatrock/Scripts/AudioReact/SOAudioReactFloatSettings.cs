using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoatRock
{
    [CreateAssetMenu(fileName = "Audio React Room Magic Settings", menuName = "Audio React Settings/Room Magic")]
    public class SOAudioReactRoomMagic : ScriptableObject
    {
        [Header("Scale what axis?")]
        [Range(0, 7f)]
        public int BandNumber = 2;
        [Range(1f, 100f)]
        public float IntensityMultiplier = 1f;
        public float IntensityPower = 1f;
        [Space(5)]
        public float MaxVolumeNumber = 20f;
        public float MinVolumeNumber = 0f;
    }
}