using System.Collections;
using UnityEngine;

namespace GoatRock
{
    public class FloatToVector3Behavior : MonoBehaviour
    {
        public bool IsFloatingEnabled = true;
        [Space(5)]
        public float PositionSpeed = 8f;
        [Space(5)]
        public bool LerpAngles = true;
        public float AngleSpeed = 0.6f;
        [Space(5)]
        public bool WorldSpace = true;
        public Vector3 TargetPosition;
        [Space(5)]
        public bool StartInRandomSpot = false;
        public PlaceInSpotWithinRadiusBehavior SpawnInRandomSpotScript;
        [Space(5)]
        public bool CustomInitialPosition = false;
        public Vector3 CustomInitialPositionVector3;

        public bool CustomTarget = false;
        public Transform CustomTargetObject;

        protected Vector3 InitialRotation;

        [SerializeField]
        private Vector3 velocity = Vector3.zero;

        protected virtual void Start()
        {
            InitialRotation = transform.localEulerAngles;

            if(CustomInitialPosition)
            {
                transform.position = CustomInitialPositionVector3;
            }

            if (CustomTarget && CustomTargetObject != null) {
                TargetPosition = CustomTargetObject.position;
            } else {
                if (TargetPosition == null)
                TargetPosition = Vector3.zero;
            }

            if(StartInRandomSpot)
            {
                if (SpawnInRandomSpotScript != null)
                {
                    SpawnInRandomSpotScript.PlaceInRandomSpot();
                }
                else
                {
                    Debug.LogWarning("No SpawnInRandomSpot Script found");
                }
            }
        }

        protected virtual void FixedUpdate()
        {
            if (IsFloatingEnabled)
            {
                if(WorldSpace)
                {
                    transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref velocity, PositionSpeed);
                    if (LerpAngles)
                    {
                        transform.localEulerAngles = new Vector3(
                        Mathf.LerpAngle(transform.eulerAngles.x, InitialRotation.x, PositionSpeed * Time.deltaTime),
                        Mathf.LerpAngle(transform.eulerAngles.y, InitialRotation.y, PositionSpeed * Time.deltaTime),
                        Mathf.LerpAngle(transform.eulerAngles.z, InitialRotation.z, PositionSpeed * Time.deltaTime)
                        );
                    }
                }
                else
                {
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition, TargetPosition, ref velocity, PositionSpeed);
                    if (LerpAngles)
                    {
                        transform.localEulerAngles = new Vector3(
                        Mathf.LerpAngle(transform.localEulerAngles.x, InitialRotation.x, AngleSpeed * Time.deltaTime),
                        Mathf.LerpAngle(transform.localEulerAngles.y, InitialRotation.y, AngleSpeed * Time.deltaTime),
                        Mathf.LerpAngle(transform.localEulerAngles.z, InitialRotation.z, AngleSpeed * Time.deltaTime)
                        );
                    }
                }
            }
        }

        public void PlaceInRandomSpot()
        {
            SpawnInRandomSpotScript.PlaceInRandomSpot();
        }

    }
}