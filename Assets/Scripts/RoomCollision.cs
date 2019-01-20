using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCollision : MonoBehaviour {

	public RoomMagic rm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter (Collision col) {


    	Vector3 pos = col.contacts[0].point;
    	rm.OnCollision(pos);

        // if(col.gameObject.name == "prop_powerCube")
        // {
        //     Destroy(col.gameObject);
        // }
    }	
}
