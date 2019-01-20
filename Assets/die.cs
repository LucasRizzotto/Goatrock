using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour {

	public float aliveForSeconds = 1.6f;
	// Use this for initialization
	void Start () {

		StartCoroutine(delayedDeath());
	}

	IEnumerator delayedDeath()
	{
		yield return new WaitForSeconds (aliveForSeconds);
		Destroy(gameObject);
	}
}
