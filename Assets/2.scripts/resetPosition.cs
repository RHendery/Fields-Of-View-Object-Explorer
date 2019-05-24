using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetPosition : MonoBehaviour
{
    Vector3 startPos;
    Quaternion startRot;
    Rigidbody theAttachedRigidbody;
    

    // Start is called before the first frame update
    void Start()
    {

        startPos = transform.position;
        startRot = transform.rotation;
        theAttachedRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    
        if (transform.position.y < -1)
        {
            theAttachedRigidbody.isKinematic = true;
            transform.position = startPos;
            transform.rotation = startRot;
            theAttachedRigidbody.isKinematic = false;
        };

      

    }
}
