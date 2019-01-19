using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GoatRock
{
    public class FloatToInitialPositionBehavior : FloatToVector3Behavior
    {
        [System.Serializable]
        public class FloatToVector3Event : UnityEvent { }
        [SerializeField]
        public FloatToVector3Event OnInitialPositionSet;

        protected override void Start()
        {
            if(WorldSpace)
            {
                InitialRotation = transform.eulerAngles;
                TargetPosition = transform.position;
            }
            else
            {
                InitialRotation = transform.localEulerAngles;
                TargetPosition = transform.localPosition;
            }

            if (CustomInitialPosition)
            {
                transform.position = CustomInitialPositionVector3;
            }

            if (StartInRandomSpot)
            {
                if(SpawnInRandomSpotScript != null)
                {
                    SpawnInRandomSpotScript.PlaceInRandomSpot();
                }
                else
                {
                    Debug.LogWarning("No SpawnInRandomSpot Script found");
                }
            }
            OnInitialPositionSet.Invoke();
        }
    }
}