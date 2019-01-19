using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhereThoughtsGo.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class ApproachPlayerOnceBehavior : MonoBehaviour
    {
        public Rigidbody ThisRigidbody;
        public bool ApproachOnStart = false;
        [Space(5)]
        public float ApproachForce = 20f;
        public float MaxApproachMagnitude;
        [Space(5)]
        public bool RandomizeInitialY = true;
        public float YDifferenceUp = 0.15f;
        public float YDifferenceDown = 0.25f;
        [Space(5)]
        public Vector2 DistanceThreshold = new Vector2(0, 2f);

        protected Vector3 DirectionToCenter;
        protected float DistanceFromCenter;
        protected Vector2 VerticalBounds = new Vector2(0f, 0f);

        private Vector3 CenterOfPlayArea;
        private float InitialY;
        private bool ApproachingPlayer = false;

        #region Unity API

        private void Start()
        {
            if(ApproachOnStart)
            {
                EnableAttraction();
            }
        }

        private void Reset()
        {
            ThisRigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(ApproachingPlayer)
            {
                ApproachPlayer();
            }
        }

        protected void OnDrawGizmos()
        {
            // Creates a sphere with the final destination of an object
            if (ApproachingPlayer)
            {
                Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
                Gizmos.DrawSphere(transform.position, 0.05f);
            }

            // Draws point between the center of the playa area and the object
            if (Vector3.Distance(transform.position, CenterOfPlayArea) < DistanceThreshold.y)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            // Creates a line between the starting point of a thought and its final initial destination
            Gizmos.DrawLine(transform.position, CenterOfPlayArea);
            Gizmos.DrawSphere(CenterOfPlayArea, 0.02f);

            // Draw the play area gizmo if you are the first one to do it
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(CenterOfPlayArea, (DistanceThreshold.y / 2f));
        }

        #endregion

        #region Attraction Methods

        public virtual void EnableAttraction()
        {
            ApproachingPlayer = true;
            ThisRigidbody.isKinematic = false;
            InitialY = RandomizeInitialY ? Random.Range(Camera.main.transform.position.y - YDifferenceDown, Camera.main.transform.position.y + YDifferenceUp) : Camera.main.transform.position.y;
            CenterOfPlayArea = new Vector3(0f, InitialY, 0f);
            VerticalBounds = new Vector2(Camera.main.transform.position.y - YDifferenceDown, Camera.main.transform.position.y + YDifferenceUp);
            CenterOfPlayArea = new Vector3(0f, InitialY, 0f);
            DirectionToCenter = Helpers.FindDirectionToPoint(transform.position, CenterOfPlayArea);
        }

        protected virtual void ApproachPlayer()
        {
            if (ThisRigidbody.velocity.magnitude < MaxApproachMagnitude)
            {
                ThisRigidbody.AddForce(-DirectionToCenter * ApproachForce);
            }

            DirectionToCenter = Helpers.FindDirectionToPoint(transform.position, CenterOfPlayArea);

            // Once the object has entered the bounds for the first time, deactivate appearing
            if (Vector3.Distance(transform.position, CenterOfPlayArea) < DistanceThreshold.y)
            {
                ApproachingPlayer = false;
            }
        }

        #endregion

        #region Testing

        public void TestBehavior()
        {
            EnableAttraction();
        }

        #endregion

    }
}