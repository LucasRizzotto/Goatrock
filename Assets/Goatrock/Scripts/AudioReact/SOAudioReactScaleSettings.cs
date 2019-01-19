using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoatRock
{
    [CreateAssetMenu(fileName = "Audio React Scale Settings", menuName = "Audio React Settings/Scale")]
    public class SOAudioReactScaleSettings : ScriptableObject
    {
        [Header("Scale what axis?")]
        public bool ScaleX = true;
        public bool ScaleY = true;
        public bool ScaleZ = true;
        [Space(5)]
        [Range(0, 7f)]
        public int BandNumber = 2;
        public float MinimumValue = 0;
        [Range(1f, 100f)]
        public float IntensityMultiplier = 20f;
        public float MaxScalePercentageIncrease = 0.2f;
    }
}