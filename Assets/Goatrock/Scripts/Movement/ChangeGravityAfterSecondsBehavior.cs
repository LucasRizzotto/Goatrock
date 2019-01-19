using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MEC;

namespace GoatRock
{
    public class ChangeGravityAfterSecondsBehavior : MonoBehaviour
    {
        public bool RunOnStart = false;
        public float SecondsBefore = 2f;

        private void Start()
        {
            if(RunOnStart)
            {
                StartChangeGravityTimer();
            }
        }
        
        public void ChangeGravity()
        {
            Rigidbody rigidbodyReference = GetComponent<Rigidbody>();
            rigidbodyReference.useGravity = !rigidbodyReference.useGravity;
        }

        void StartChangeGravityTimer()
        {
            Timing.RunCoroutine(_StartChangeGravityTimer());
        }

        IEnumerator<float> _StartChangeGravityTimer()
        {
            yield return Timing.WaitForSeconds(SecondsBefore);
            ChangeGravity();
        }
    }
}