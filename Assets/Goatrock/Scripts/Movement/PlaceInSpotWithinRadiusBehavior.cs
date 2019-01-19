using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceInSpotWithinRadiusBehavior : MonoBehaviour {

    public bool RandomizeOnStart = true;
    public float Radius = 5f;
    public bool RandomizeAngles = true;
    
	void Start () {
        if (RandomizeOnStart)
        {
            PlaceInRandomSpot();
        }
    }

    public void PlaceInRandomSpot()
    {
        transform.localPosition = Vector3.zero + Random.insideUnitSphere * Radius;

        if (RandomizeAngles)
        {
            transform.localEulerAngles = new Vector3(
                Random.Range(-90f, 90f),
                Random.Range(-90f, 90f),
                Random.Range(-90f, 90f)
            );
        }
    }
}
