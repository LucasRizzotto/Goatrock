using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitchAnalysis : MonoBehaviour {

	public int pitchTone = 0;

	private float pitchMinimum = 150f;
	private float pitchMaximum = 250f;
	private int pitchIncrements = 20;
	private int[] pitches;
	private int spread = 20;
	private int meter = 0;

	// Use this for initialization
	void Start ()
	{
		pitches = new int[spread];
	}

	// Update is called once per frame
	void Update ()
	{
		SetTone();
		GetAverageTone();
	}

	public void GetAverageTone()
	{
		float pitchTemp = 0f;

		for(int i = 0; i < spread; i++)
		{
			pitchTemp += pitches[i];
		}

		pitchTone = (int)Mathf.Round(pitchTemp / spread);
	}

	public void SetTone()
	{
		float incrementSize = 0;

		incrementSize = (pitchMaximum - pitchMinimum)/(float)pitchIncrements;
		pitches[meter % spread] = (int)Mathf.Round((GetPitch() - pitchMinimum)/incrementSize);
		meter++;
	}

	public float GetPitch()
	{
		return transform.localPosition.x;
	}
}
