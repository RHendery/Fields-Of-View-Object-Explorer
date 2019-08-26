using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundOnOverlap : MonoBehaviour
{
    AudioSource theAudioSource;
    public AudioClip theTouchSound;
    public float audioRangeParameter;

    // Start is called before the first frame update
    void Start()
    {
        theAudioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound(string lower_upper)
    {
        string[] theParams = lower_upper.Split('~');
        print(theParams[0]);
        print(theParams[1]);
        if(audioRangeParameter >= float.Parse(theParams[0]) && audioRangeParameter <= float.Parse(theParams[1]))
        {
            theAudioSource.clip = theTouchSound;
            theAudioSource.Play();
        }

    }

    public void stopPlayingSound()
    {
        theAudioSource.clip = theTouchSound;
        theAudioSource.Stop();
    }
}
