using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class ApplyRandomForceBehavior : ApplyForceBehavior
    {
        [Space(5)]
        public float MinForce = 5f;
        public float MaxForce = 20f;

        protected override void Start()
        {
            DefaultForce = Random.Range(MinForce, MaxForce);
            base.Start();
        }

    }

}