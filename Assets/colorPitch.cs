using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colorPitch : MonoBehaviour {

	public Color col;

	public pitchAnalysis pitchFinder;

	private int maxPitch = 500;
	private int minPitch = 0;
	private float range = 400.0f;

	private Renderer rend;


	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		range = maxPitch - minPitch;
	}

	// Update is called once per frame
	void Update ()
	{
		Colorize();
	}

	public void Colorize()
	{
		float pitch = (float)pitchFinder.pitchTone;
		pitch = (pitch - minPitch)/range;
		col = Color.HSVToRGB(pitch, 1f, 1f);
		rend.material.color = col;
	}
}
