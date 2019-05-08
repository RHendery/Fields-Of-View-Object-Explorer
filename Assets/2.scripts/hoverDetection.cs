using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hoverDetection : MonoBehaviour {

    Material material;

    // Use this for initialization
    void Start () {
        material = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void doit()
    {
        print("over");
        material.color = new Color(1, 1, 1, 0.5f);
    }

    public void undoit()
    {
        material.color = new Color(1, 1, 1, 1);
    }
}
