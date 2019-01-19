using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class AttractObjectInTriggerToTransform : MonoBehaviour
    {
        public Transform TargetTransform;
        public int NumberOfObjects = 1;
        public float AttractionForce = 10f;
        public LayerMask AttractionLayerMask;
        public Rigidbody CurrentRigidbody;

        #region Unity APIs

        private void OnTriggerStay(Collider other)
        {
            if(CurrentRigidbody != null)
            {
                return;
            }

            // Check if the object we want to pull is in the correct layer
            if (Helpers.IsInLayerMask(other.gameObject.layer, AttractionLayerMask))
            {
                CurrentRigidbody = other.GetComponent<Rigidbody>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check if this object is in the correct layer
            if (Helpers.IsInLayerMask(other.gameObject.layer, AttractionLayerMask))
            {
                // Check if the leaving rigidbody is the same as the one that's being pulled
                Rigidbody leavingRigidbody = other.GetComponent<Rigidbody>();
                if (leavingRigidbody == CurrentRigidbody)
                {
                    // Clear it so we can pull other objects
                    CurrentRigidbody = null;
                }
            }
        }

        private void FixedUpdate()
        {
            // Pull the current rigidbody after calculating physics
            if(CurrentRigidbody != null)
            {
                CurrentRigidbody.AddForce(
                        Helpers.FindDirectionToPoint(CurrentRigidbody.transform.position, TargetTransform.position) * -AttractionForce * Time.deltaTime,
                        ForceMode.Force
                    );
            }
        }

        #endregion
    }
}