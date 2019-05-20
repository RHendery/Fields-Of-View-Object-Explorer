using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashNodes : MonoBehaviour
{

    // public GameObject node;
    GameObject objectToChange;
    float incrementSize = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void EnlargeNode()
    {
        objectToChange = GameObject.Find(string.Concat(gameObject.name, "_node"));

     //   Debug.Log(string.Concat("The name of the objectToChange gameObject is: ", objectToChange.name));

        //make the node bigger
        objectToChange.transform.localScale += new Vector3(incrementSize, incrementSize, incrementSize);
        Debug.Log(string.Concat("The name of the objectToChange gameObject that the EnlargeNode is actually operating on is: ", objectToChange.name));

        Renderer rend = objectToChange.GetComponent<Renderer>();
        //Set the main Color of the Material to green
        rend.material.SetColor("_Color", Color.green);

  


    }

    public void ShrinkNode()
    {
        objectToChange = GameObject.Find(string.Concat(gameObject.name, "_node"));

      //  Debug.Log(string.Concat("The name of the objectToChange gameObject is: ", objectToChange.name));

        //reset the node size
        objectToChange.transform.localScale -= new Vector3(incrementSize, incrementSize, incrementSize);
        Renderer rend = objectToChange.GetComponent<Renderer>();
        //set it back to blue
        rend.material.SetColor("_Color", Color.blue);

    }
}
