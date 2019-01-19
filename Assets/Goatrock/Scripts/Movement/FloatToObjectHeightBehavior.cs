using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class FloatToObjectHeightBehavior : MonoBehaviour
    {
        public bool EnableBehavior = true;
        [Space(5)]
        public Transform TargetObject;
        public float SmoothTime = 1f;
        public float MinimumY = 0.3f;
        public float DifferenceBetweenTargetHeight = 0.3f;

        private void LateUpdate()
        {
            if (TargetObject != null)
            {
                Vector3 newPos = new Vector3(transform.position.x, TargetObject.position.y - DifferenceBetweenTargetHeight, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, newPos, SmoothTime * Time.deltaTime);
                if (transform.position.y < MinimumY)
                {
                    transform.position = new Vector3(transform.position.x, MinimumY, transform.position.z);
                }
            }
        }
    }
}