using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoatRock
{
    public class FloatToCameraHeightBehavior : FloatToObjectHeightBehavior
    {
        private void Start()
        {
            if(Camera.main != null)
            {
                TargetObject = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning("Can't find Main Camera. FloatToCameraHeightBehavior won't work.");
            }
        }
    }
}