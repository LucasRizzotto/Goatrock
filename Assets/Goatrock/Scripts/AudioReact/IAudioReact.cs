using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    /// <summary>
    /// This interface establishes all the actions that you should have set haptics to 
    /// </summary>
    public interface IAudioReact
    {
        void SetupVisualization();
        void StartAudioVisualization();
        void StopAudioVisualization();
        void UpdateAudioVisualization();
    }

}