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

        if(other.tag == "museumObject" && transform.childCount > 0)
        {
            transform.GetChild(0).transform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "museumObject" && theAttachedAudioSource.clip != null)
        {
            theAttachedAudioSource.Stop();
        }

        if (other.tag == "museumObject" && transform.childCount > 0)
        {
            transform.GetChild(0).transform.gameObject.SetActive(false);
        }
    }
}
