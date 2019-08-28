using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudioOnTrigger : MonoBehaviour
{
    AudioSource theAttachedAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        theAttachedAudioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "museumObject" && theAttachedAudioSource.clip != null)
        {
            theAttachedAudioSource.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "museumObject" && theAttachedAudioSource.clip != null)
        {
            theAttachedAudioSource.Stop();
        }
    }
}
