using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class soundOnTouch : MonoBehaviour
{
    AudioSource theAudioSource;
    public AudioClip theTouchSound;

    // Start is called before the first frame update
    void Start()
    {
        theAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void touchSound()
    {
        theAudioSource.clip = theTouchSound;
        theAudioSource.Play();
    }
}
