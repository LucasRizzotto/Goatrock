using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour{  
    //A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;  
  
    //The maximum and minimum available recording frequencies  
    private int minFreq;  
    private int maxFreq;  
  
    //A handle to the attached AudioSource  
    private AudioSource goAudioSource;

    public GameObject voiceBubble;


    //Use this for initialization  
    void Start()
    {
        //Check if there is at least one microphone connected  
        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present  
        {
            //Set 'micConnected' to true  
            micConnected = true;

            //Get the default microphone recording capabilities  
            Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

            //According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
            if (minFreq == 0 && maxFreq == 0)
            {
                //...meaning 44100 Hz can be used as the recording sampling rate  
                maxFreq = 44100;
            }

            //Get the attached AudioSource component  
            goAudioSource = this.GetComponent<AudioSource>();
        }
    }



    void Update()   
    {




        //If there is a microphone  
        if (micConnected)  
        {  
            //If the audio from any microphone isn't being captured  
            if(!Microphone.IsRecording(null))  
            {  
                //Case the 'Record' button gets pressed  
                if(Input.GetMouseButtonDown(0))  
                {  
                    //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
                    goAudioSource.clip = Microphone.Start(null, true, 20, maxFreq);
                    Debug.Log("Record");
                }  
            }  
            else //Recording is in progress  
            {  
                //Case the 'Stop and Play' button gets pressed  
                if(Input.GetMouseButtonDown(1))  
                {  
                   
                    Debug.Log("Stop");
                    GameObject pop = Instantiate(voiceBubble, transform.position + transform.forward * 0.05f, transform.rotation);
                }  
  
                 
            }  
        }  
        else // No microphone  
        {  

        }  
  
    }  
}  