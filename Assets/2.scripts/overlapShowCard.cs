using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overlapShowCard : MonoBehaviour
{
    float sphereRadius;
    public float lowerSchemaRange;
    public float upperSchemaRange;

    void Start()
    {
        sphereRadius = transform.localScale.x / 2;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "museumObject")
        {
            print(other.name);
            other.gameObject.SendMessage("attachTheCard", lowerSchemaRange + "~" + upperSchemaRange, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "museumObject")
        {
            print(other.name);
            other.gameObject.SendMessage("detachTheCard", SendMessageOptions.DontRequireReceiver);
        }
    }


}
