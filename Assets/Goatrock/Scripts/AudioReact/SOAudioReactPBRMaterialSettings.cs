using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoatRock
{
    [CreateAssetMenu(fileName = "Audio React PBR Material Settings", menuName = "Audio React Settings/PBR Material")]
    public class SOAudioReactPBRMaterialSettings : ScriptableObject
    { 
        [Header("What to change?")]
        public bool ChangeColor = true;
        public bool ChangeEmission = true;
        [Range(-1, 7)]
        public int Band = 4;
        public float IntensityMultiplier = 4f;

        [Header("Color Settings")]
        public Color MinColor;
        public Color MaxColor;
        [Space(5)]
        public Color MinEmission;
        public Color MaxEmission;
    }
}