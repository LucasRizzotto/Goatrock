using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    [RequireComponent(typeof(Rigidbody))]
    public class ApplyForceBehavior : MonoBehaviour
    {
        public float DefaultForce = 10f;
        public Vector3 DefaultDirection;
        [Space(5)]
        public Rigidbody ThisRigidbody;
        public bool CameraForward = false;
        [Space(5)]
        public bool ApplyContinuously = false;
        public bool ApplyOnStart = false;

        protected virtual void Start()
        {
            if(ApplyOnStart)
            {
                ApplyForce(ThisRigidbody, DefaultDirection, DefaultForce);
            }
            
            
        }

        private void Reset()
        {
            ThisRigidbody = GetComponent<Rigidbody>();
            ThisRigidbody.useGravity = false;
        }

        protected virtual void Update()
        {
            if(ApplyContinuously)
            {
                if (CameraForward)
                {
                    DefaultDirection = Camera.main.transform.forward;
                }

                ApplyForce(ThisRigidbody, DefaultDirection, DefaultForce * Time.deltaTime);
            }
        }

        public void ApplyForce(Rigidbody reference, Vector3 direction, float force)
        {
            reference.AddForce(Vector3.Normalize(direction) * force);
        }

        public void StartContinuousForce()
        {
            ApplyContinuously = true;
        }

        public void StopContinuousForce()
        {
            ApplyContinuously = false;
        }

    }

}