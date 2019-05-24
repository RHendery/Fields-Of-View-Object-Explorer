using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudio : MonoBehaviour
{
    float sphereRadius;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Sphere2")
        {
            sphereRadius = other.transform.localScale.x / 2;
            LoopAudio();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.name == "Sphere2")
        {
            print(other.name);

        }

    }


    void LoopAudio()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();

        AudioClip clip = (AudioClip)Resources.Load("audio1");
        if (clip != null)
        {
            audio.PlayOneShot(clip, 1.0f);
        }
        else
        {
            Debug.Log("Attempted to play missing audio");
        }
    }
}