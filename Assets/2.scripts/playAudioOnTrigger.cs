using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
            //transform.GetChild(0).transform.gameObject.SetActive(true);
            foreach (Transform child in transform)
            {
                //child.gameObject.SetActive(true);
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1,1,1,1);
                if (child.gameObject.GetComponent<TextMeshPro>() != null)
                {
                    child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 1);
                }
                child.gameObject.transform.localScale = child.gameObject.transform.localScale * 2;
            }
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
            //transform.GetChild(0).transform.gameObject.SetActive(false);
            foreach (Transform child in transform)
            {
                //child.gameObject.SetActive(false);
                child.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.25f);
                if (child.gameObject.GetComponent<TextMeshPro>() != null)
                {
                    child.gameObject.GetComponent<TextMeshPro>().color = new Color(1, 1, 1, 0.15f);
                }

                child.gameObject.transform.localScale = child.gameObject.transform.localScale / 2;
            }
        }
    }
}
