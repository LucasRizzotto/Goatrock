using UnityEngine;

namespace GoatRock
{
    public class StayInFrontOfObjectBehavior : MonoBehaviour
    {
        public bool EnableBehavior = true;
        [Space(5)]
        public Transform TargetObject;
        public bool FixY = false;
        [Space(5)]
        public float SmoothTime = 0.6f;
        public float Distance = 1f;
        [Space(10)]
        private float InitialY = 0f;
        private Vector3 velocity = Vector3.zero;

        #region Unity APIs

        private void Start()
        {
            InitialY = transform.position.y;
        }

        void FixedUpdate()
        {
            if (EnableBehavior)
            {
                if (!FixY)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, Helpers.GetFrontOfObject(TargetObject, Distance), ref velocity, SmoothTime);
                }
                else
                {
                    Vector3 temp = Vector3.SmoothDamp(transform.position, Helpers.GetFrontOfObject(TargetObject, Distance), ref velocity, SmoothTime);
                    transform.position = new Vector3(temp.x, InitialY, temp.z);
                }
            }
        }

        #endregion
        
        public void StartTracking()
        {
            EnableBehavior = true;
        }

        public void StopTracking()
        {
            EnableBehavior = false;
        }

    }

}