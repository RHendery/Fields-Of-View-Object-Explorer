using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlapPlayAudio : MonoBehaviour
{
    float sphereRadius;

    void Start()
    {
        sphereRadius = transform.localScale.x / 2;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "museumObject")
        {
            print(other.name);
            other.gameObject.SendMessage("playSound", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "museumObject")
        {
            print(other.name);
            other.gameObject.SendMessage("stopPlayingSound", SendMessageOptions.DontRequireReceiver);
        }
    }


}
