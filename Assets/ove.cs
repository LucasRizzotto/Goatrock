using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ove : MonoBehaviour {

	public int speed = 4;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.forward * (Time.deltaTime * speed));
	}
}
