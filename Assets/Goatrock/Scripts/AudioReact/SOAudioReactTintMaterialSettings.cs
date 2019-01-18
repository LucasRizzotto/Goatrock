using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoatRock
{
    [CreateAssetMenu(fileName = "Audio React Tint Material Settings", menuName = "Audio React Settings/Tint Material")]
    public class SOAudioReactTintMaterialSettings : ScriptableObject
    {
        [Header("What to change?")]
        [Range(-1, 7)]
        public int Band = 3;
        public float Multiplier = 4f;

        [Header("Color Settings")]
        public Color MinColor;
        public Color MaxColor;
    }
}