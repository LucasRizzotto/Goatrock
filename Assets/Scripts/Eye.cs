using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {

	public Transform cam  ;
	float velA;
	// public bool extraShift = true;
	public Vector3 shift = Vector3.zero;
	public float setVel = -1f; 

	public bool isMoving = false;
	public float minDist = .25f; 
	public float maxDist = 2f; 
	public float moveVel = 100f; 


	// Use this for initialization
	void Start () {

		float min = 20f;
		float max = 150f;
		velA = Mathf.Pow(Random.value, 3f)*max + min; //angle velocity / angles a second

		if(setVel>0f) velA = setVel;
	}
	
	// Update is called once per frame
	void Update () {

		Quaternion rot; //new rotation
		rot =  Quaternion.LookRotation(cam.position, Vector3.up);
		rot *= Quaternion.Euler(shift.x, shift.y, shift.z); // this adds a 90 degrees Y rotation
		// if(extraShift) rot *= Quaternion.Euler(90, 0, 0); // this adds a 90 degrees Y rotation
		// transform.rotation = rot; // this adds a 90 degrees Y rotation

		var angle = Quaternion.Angle( transform.rotation, rot );
		// var maxA = 10f;

		
		float dT = Time.deltaTime;
		float dA = velA * dT; 

		float dAr = dA/angle; //relative angle
		transform.rotation = Quaternion.Slerp(transform.rotation, rot, dAr );


		if(isMoving){
			float oY = transform.position.y;
			float dX = moveVel * dT;
			Vector3 dist = (cam.position- transform.position);
			float len = dist.magnitude;
			Vector3 dir = dist.normalized;
			Vector3 pos = transform.position; //destination pos
			// print(len);
			// else return;
			// if(len<minDist) pos =  transform.position + dir*minDist;
			// if(len>maxDist) pos =  transform.position + dir*maxDist;
			// transform.position = Vector3.Lerp(transform.position, pos, dX );
			if(len<minDist) transform.position =  transform.position - dir*dX;
			if(len>maxDist) transform.position =  transform.position + dir*dX;

			transform.position = new Vector3(transform.position.x, oY, transform.position.z); //lock Y
		}

	}
}
