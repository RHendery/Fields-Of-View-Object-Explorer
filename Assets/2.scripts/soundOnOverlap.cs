using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundOnOverlap : MonoBehaviour
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

    public void playSound()
    {
        theAudioSource.clip = theTouchSound;
        theAudioSource.Play();
    }

    public void stopPlayingSound()
    {
        theAudioSource.clip = theTouchSound;
        theAudioSource.Stop();
    }
}
