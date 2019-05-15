using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMap : MonoBehaviour
{
    public GameObject red_dot;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetDot()
    { 
        red_dot.SetActive(true);
       
    }

    public void RemoveDot()
    {
        red_dot.SetActive(false);

    }
}
