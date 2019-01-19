using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{/// <summary>
 ///  This class makes the materials of a given list of Renderers audio reactive
 /// </summary>
    public class AudioReactMaterialTintBehavior : AudioReactBehavior
    {

        [Header("Main References")]
        public List<Renderer> ReferenceRenderers = new List<Renderer>();
        [Space(5)]
        public SOAudioReactTintMaterialSettings Settings;

        protected Material AnimatedMaterial;
        protected Material OriginalMaterial;

        #region Audio React APIs

        public override void SetupVisualization()
        {
            base.SetupVisualization();
            try
            {
                AnimatedMaterial = Instantiate(ReferenceRenderers[0].sharedMaterial) as Material;
            }
            catch
            {
                Debug.LogError("Couldn't cache a material for the AudioReactMaterial");
            }
            OriginalMaterial = AnimatedMaterial;
            for(int i = 0; i < ReferenceRenderers.Count; i++)
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
        }

        public override void UpdateAudioVisualization()
        {
            base.UpdateAudioVisualization();
            Color tintColor = Color.Lerp(Settings.MinColor, Settings.MaxColor, ReferenceAudioReactSource.GetBandData(Settings.Band) * Settings.Multiplier);
            foreach (Renderer rend in ReferenceRenderers)
            {
                rend.sharedMaterial.SetColor("_TintColor", tintColor);
            }
        }

        #endregion

    }

}
