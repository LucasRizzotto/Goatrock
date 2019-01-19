using UnityEngine;

namespace GoatRock
{
    public class StayInFrontOfCameraBehavior : StayInFrontOfObjectBehavior
    {
        private void Start()
        {
            if(Camera.main != null)
            {
                TargetObject = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("No Main Camera found. Cannot use StayInFrontOfCameraBehavior.");
            }
        }
    }
}