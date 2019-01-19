using UnityEngine;
using System.Collections;

namespace GoatRock
{
    /// <summary>
    /// Follows target object smoothly
    /// </summary>
    public class LookAtTargetBehavior : MonoBehaviour
    {
        public bool SmoothLook = true;
        public bool StartLookAt = false;
        public Transform Target; // An Object to lock on to
        public float Damping = 6.0f; // To control the rotation 
        public float MinDistance = 10.0f; // How far the target is from the camera
        [SerializeField, Tooltip("Rotation Offset in Euler Angles")]
        public Vector3 _rotationOffset;

        public virtual void Start()
        {
            if(Target == null)
            {
                Debug.LogWarning("You must define a target for LookAtTargetBehavior");
                return;
            }

            if (StartLookAt) {
                transform.LookAt(Target);
                transform.rotation *= Quaternion.Euler(_rotationOffset);
            }
        }

        void LateUpdate()
        {
            if (Target != null)
            {
                if(MinDistance == 0 || Vector3.Distance(transform.position, Target.position) < MinDistance)
                {
                    if (SmoothLook)
                    {
                        // Look at and dampen the rotation
                        Quaternion rotation = Quaternion.LookRotation(Target.position - transform.position) * Quaternion.Euler(_rotationOffset);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * Damping);
                    }
                    else
                    {
                        transform.LookAt(Target);
                        transform.rotation *= Quaternion.Euler(_rotationOffset);
                    }
                }
            }
        }
    }
}