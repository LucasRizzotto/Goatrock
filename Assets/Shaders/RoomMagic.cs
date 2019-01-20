using GoatRock;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMagic : MonoBehaviour {

	public  int colIdx = 0;
    public  int colMax = 3;
    public  float[] colStart;// = 10000;
    public  Vector3[] colCenter;// = Vector3.zero;

    public bool isDebugPositions = true;
    public Transform p1;
    public Transform p2;
    public Transform p3;

    [HideInInspector]
    [Range(0,1)] public float micVolume;

	// Use this for initialization
	void Start () {
		colStart = new float[colMax];
	 	colCenter = new Vector3[colMax];

	 	if(isDebugPositions){
	 		OnCollision(p1.position);
	 		OnCollision(p2.position);
	 		OnCollision(p3.position);
	 	}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
		Shader.SetGlobalVector("_mdpColCenter1", colCenter[0]);
		Shader.SetGlobalVector("_mdpColCenter2", colCenter[1]);
		Shader.SetGlobalVector("_mdpColCenter3", colCenter[2]);
		Shader.SetGlobalFloat("_mdpColTime1", Time.time - colStart[0] );
		Shader.SetGlobalFloat("_mdpColTime2", Time.time - colStart[1] );
		Shader.SetGlobalFloat("_mdpColTime3", Time.time - colStart[2] );
		// Shader.SetGlobalFloat("_mdpMicVolume", micVolume );
		// float _mdpTime = ( Time.time - colStart ) ;

	}


	// public float[] colSpeed = 1;
	public  void OnCollision(Vector3 pos){

		colStart[colIdx] = Time.time;
		colCenter[colIdx] = pos;

		colIdx++;
		if(colIdx>=colMax) colIdx = 0;

	}
}
