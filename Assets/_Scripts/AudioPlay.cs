using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public GameObject playerRecord;
    private AudioSource audioSource;
    public float interval = 0.5f;
    public float force = 10f;
    private float acc_time = 0f;

    // Use this for initialization
    void Start()
    {
        playerRecord = GameObject.FindGameObjectWithTag("Player");
        audioSource = playerRecord.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.Play();
        audioSource.loop = true;
        acc_time += Time.deltaTime;
        if (acc_time > interval)
        {
            acc_time = 0;
         
            this.GetComponent<Rigidbody>().velocity = this.transform.forward * force;
        }
    }
}
