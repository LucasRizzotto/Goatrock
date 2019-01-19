using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudioEcho : MonoBehaviour {


    public List<UnityEngine.Audio.AudioMixerGroup> MixerList;
    public int mixerNum;
    public float loudness;


    //private AudioClip tineSample;
    //public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    // Use this for initialization
    void Start()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        MixerList[0].audioMixer.SetFloat("GlobalLowpass", loudness);
    }


    void OnCollisionExit(Collision other)
    {
      

    }




    // Update is called once per frame
    void Update()
    {
        
    }
}




