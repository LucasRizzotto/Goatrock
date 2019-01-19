using UnityEngine;
using System.Collections;

namespace GoatRock
{
    /// <summary>
    /// Follows target object smoothly
    /// </summary>
    public class LookAtCameraBehavior : LookAtTargetBehavior
    {
        public override void Start()
        {
            Target = Camera.main.transform;

            if (StartLookAt) {
                transform.LookAt(Target);
                transform.rotation *= Quaternion.Euler(_rotationOffset);
            }
        }
    }
}