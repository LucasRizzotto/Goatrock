using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMagic : MonoBehaviour {


	int colIdx = 0;
	int colMax = 3; 	
	float[] colStart;// = 10000;
	Vector3[] colCenter;// = Vector3.zero;

	public Transform posDebug0;
	public Transform posDebug1;
	public Transform posDebug2;


	// Use this for initialization
	void Start () {
		
		colStart = new float[colMax];
	 	colCenter = new Vector3[colMax];

		//test
		if(posDebug0) OnCollision(posDebug0.position);
		if(posDebug1) OnCollision(posDebug1.position);
		if(posDebug2) OnCollision(posDebug2.position);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		

		Shader.SetGlobalVector("_mdpColCenter1", colCenter[0]);
		Shader.SetGlobalVector("_mdpColCenter2", colCenter[1]);
		Shader.SetGlobalVector("_mdpColCenter3", colCenter[2]);
		Shader.SetGlobalFloat("_mdpColTime1", Time.time - colStart[0] );
		Shader.SetGlobalFloat("_mdpColTime2", Time.time - colStart[1] );
		Shader.SetGlobalFloat("_mdpColTime3", Time.time - colStart[2] );
		// float _mdpTime = ( Time.time - colStart ) ;

	}


	// public float[] colSpeed = 1;
	void OnCollision(Vector3 pos){

		colStart[colIdx] = Time.time;
		colCenter[colIdx] = pos;

		colIdx++;
		if(colIdx>=colMax) colIdx = 0;

	}
}
