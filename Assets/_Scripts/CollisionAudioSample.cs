using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAudioSample : MonoBehaviour {


    public List<AudioClip> audioClips;
    public UnityEngine.Audio.AudioMixerGroup tineMixer;
    public float prob = 0.6f;
    bool willPlay;
    AudioSource audio;
    float falloffSpeed = .02f;

    private AudioClip tineSample;
    public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        audio = GetComponent<AudioSource>(); // GetComponent <AudioSource> ();
        audio.outputAudioMixerGroup = tineMixer;
        tineSample = audioClips[Random.Range(0, audioClips.Count)];
        willPlay = (UnityEngine.Random.Range(0, 1) > prob) ? false : true;
    }

    void OnCollisionEnter(Collision other)
    {
       
            audio.PlayOneShot(tineSample, Random.Range(3, 4));
       

    }


    void OnCollisionExit(Collision other)
    {
      

    }




    // Update is called once per frame
    void Update()
    {

    }
}




