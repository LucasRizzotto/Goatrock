using System;
using System.Collections.Generic;
using UnityEngine;


namespace GoatRock
{/// <summary>
 ///  This class makes the materials of a given list of Renderers audio reactive
 /// </summary>
    public class AudioReactMaterialPBRBehavior : AudioReactBehavior
    {
        [Header("Main References")]
        public List<Renderer> ReferenceRenderers = new List<Renderer>();
        [Space(5)]
        public SOAudioReactPBRMaterialSettings Settings;
        
        [SerializeField]
        protected Material AnimatedMaterial;
        [SerializeField]
        protected Material OriginalMaterial;

        #region Audio React APIs

        public override void StartAudioVisualization()
        {
            base.StartAudioVisualization();
            try
            {
                AnimatedMaterial = Instantiate(ReferenceRenderers[0].sharedMaterial) as Material;
            }
            catch(Exception error)
            {
                Debug.LogError("Couldn't cache a material for the AudioReactMaterial. Error: " + error.Message);
            }

            if(OriginalMaterial == null)
            {
                OriginalMaterial = ReferenceRenderers[0].sharedMaterial;
            }
                
            for (int i = 0; i < ReferenceRenderers.Count; i++)
            {
                ReferenceRenderers[i].sharedMaterial = AnimatedMaterial;
            }
        }

        public override void StopAudioVisualization()
        {
            base.StopAudioVisualization();
            for (int i = 0; i < ReferenceRenderers.Count; i++)
            {
                ReferenceRenderers[i].sharedMaterial = OriginalMaterial;
            }
            Destroy(AnimatedMaterial);
            Debug.Log("Material instance destroyed");
        }

        public override void UpdateAudioVisualization()
        {
            base.UpdateAudioVisualization();

            if (AnimatedMaterial != null) {
                if (Settings.ChangeColor) {
                    Color albedo = Color.Lerp(Settings.MinColor, Settings.MaxColor, ReferenceAudioReactSource.GetBandData(Settings.Band) * Settings.IntensityMultiplier);
                    AnimatedMaterial.color = albedo;
                }

                if (Settings.ChangeEmission) {
                    Color emission = Color.Lerp(Settings.MinEmission, Settings.MaxEmission, ReferenceAudioReactSource.GetBandData(Settings.Band) * Settings.IntensityMultiplier);
                    AnimatedMaterial.SetColor("_EmissionColor", emission);
                }
            }
        }

        #endregion

    }

}
