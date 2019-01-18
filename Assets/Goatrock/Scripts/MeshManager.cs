using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.MagicLeap;

namespace GoatRock
{
    public class MeshManager : MonoBehaviour
    {
        public KeyCode ToggleMeshingDebugKey;

        [Space(10)]
    
        public MLSpatialMapper SpatialMapper;

        public class MeshManagerEvent : UnityEvent { }
        [Space(10)]
        public MeshManagerEvent OnStartMeshing;
        public MeshManagerEvent OnStopMeshing;

        public delegate void StartMeshBuildingEvent();
        public static event StartMeshBuildingEvent OnMeshBuildingStart;
        public static void StartMeshBuilding()
        {
            if (OnMeshBuildingStart != null)
            {
                Debug.Log("Started building the mesh!");
                OnMeshBuildingStart();
            }
        }

        public delegate void StopMeshBuildingEvent();
        public static event StopMeshBuildingEvent OnMeshBuildingStop;
        public static void StopMeshBuilding()
        {
            if (OnMeshBuildingStop != null)
            {
                Debug.Log("Stopped building the mesh.");
                OnMeshBuildingStop();
            }
        }


        #region Unity APIs

        private void Awake()
        {
            MeshManager.OnMeshBuildingStart += BuildMesh;
            MeshManager.OnMeshBuildingStop += StopBuild;
        }

        private void Update()
        {
            if(Input.GetKeyDown(ToggleMeshingDebugKey))
            {
                ToggleMeshing();
            }
        }

        #endregion

        #region Main Methods

        public void ToggleMeshing()
        {
            if(SpatialMapper.enabled)
            {
                StopMeshBuilding();
            }
            else
            {
                StartMeshBuilding();
            }
        }

        public void BuildMesh()
        {
            SpatialMapper.enabled = true;
            OnStartMeshing.Invoke();
        }

        public void StopBuild()
        {
            SpatialMapper.enabled = false;
            OnStopMeshing.Invoke();
        }

        #endregion
    }
}