using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashNodes : MonoBehaviour
{

    // public GameObject node;
    GameObject objectToChange;


    // Start is called before the first frame update
    void Start()
    {
       objectToChange  = GameObject.Find(string.Concat(gameObject.name, "_node"));
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnlargeNode()
    {
        objectToChange.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
        Renderer rend = objectToChange.GetComponent<Renderer>();
        //Set the main Color of the Material to red
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.red);

   
    }

    public void ShrinkNode()
    {
        objectToChange.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Renderer rend = objectToChange.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.blue);

    }
}
