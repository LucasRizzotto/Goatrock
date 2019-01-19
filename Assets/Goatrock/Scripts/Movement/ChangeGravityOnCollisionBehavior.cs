using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class ChangeGravityOnCollisionBehavior : MonoBehaviour
    {
        public bool GravityOnCollision = false;
        public LayerMask TargetLayers;

        private void OnCollisionEnter(Collision collision)
        {
            if (TargetLayers == (TargetLayers | (1 << collision.gameObject.layer)))
            {
                Rigidbody rigidbodyReference = GetComponent<Rigidbody>();
                rigidbodyReference.useGravity = GravityOnCollision;
            }
        }
    }
}