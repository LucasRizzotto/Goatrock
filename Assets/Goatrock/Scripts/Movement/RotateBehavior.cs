using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class RotateBehavior : MonoBehaviour
    {
        public bool RandomRotateOnStart = false;
        public bool RotateContinuously = false;
        [Space(5)]
        public bool RotateX = false;
        public float XAngle = 0f;
        [Space(5)]
        public bool RotateY = false;
        public float YAngle = 0f;
        [Space(5)]
        public bool RotateZ = false;
        public float ZAngle = 0f;
        [Space(5)]
        public float SpeedMultiplier = 1;

        private Vector3 RotationVector;

        private void Start()
        {
            RotationVector = Vector3.zero;
            if (RandomRotateOnStart)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Random.Range(0, 180f), transform.localEulerAngles.z);
            }
        }

        void LateUpdate()
        {
            if (RotateContinuously)
            {
                if(RotateX)
                    RotationVector.x = XAngle;

                if(RotateY)
                    RotationVector.y = YAngle;

                if(RotateZ)
                    RotationVector.z = ZAngle;

                transform.Rotate(RotationVector * Time.deltaTime * SpeedMultiplier, Space.World);
            }
        }

    }

}