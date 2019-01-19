using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionInFrontOfCameraOnStart : MonoBehaviour {

    public float Distance;

    private void OnEnable()
    {
        transform.position = Helpers.GetFrontOfObject(transform, Distance, Camera.main.transform.position.y);
    }
}
