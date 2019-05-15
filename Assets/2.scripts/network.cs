using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network : MonoBehaviour
{
    public Material LineMaterial;
    string line1, line2, line3, line4;
    public GameObject sphere;
    GameObject monkey3_point, monkey2_point, monkey1_point;

   // public Object newPrefab;

    // Start is called before the first frame update
    void Start()  
    {
        var points1 = new Vector3[3]; //I should really refactor this now I have it working
        points1[0] = new Vector3(-10, 5, -2);
        points1[1] = new Vector3(-15, 5, 2);
        points1[2] = new Vector3(-15, 5, 2);

        SetupLine(points1, "line1");


        //now we put some nodes at the intersections of the line segments
        int i = 0;
        while (i < points1.Length)
        {
           GameObject s = Instantiate(sphere, points1[i], Quaternion.identity);
            s.name = string.Concat("point1_", i.ToString()); //give it a tag we can refer to later
            i++;
        }


        var points2 = new Vector3[3];
        points2[0] = new Vector3(-5, 5, 2);
        points2[1] = new Vector3(-15, 5, 2);
        points2[2] = new Vector3(-5, 0, 2);
        SetupLine(points2, "line2");

        int j = 0;
        while (j < points2.Length)
        {
            GameObject s = Instantiate(sphere, points2[j], Quaternion.identity);
            s.name = string.Concat("point2_", j.ToString());
            j++;
        }


        var points3 = new Vector3[3];
        points3[0] = new Vector3(-5, 2, 2);
        points3[1] = new Vector3(-5, 0, 2);
        points3[2] = new Vector3(-2, 0, 0);
        SetupLine(points3, "line3");

        int k = 0;
        while (k < points3.Length)
        {
            GameObject s = Instantiate(sphere, points3[k], Quaternion.identity);
            s.name = string.Concat("point3_", k.ToString());
            k++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //a line is a continuous sequence of points. For a discontinuous segment, you have to make a new line.
    void SetupLine(Vector3[] points, string line_name)
    {
        GameObject obj = new GameObject(line_name);
        var line = obj.AddComponent<LineRenderer>();
       
     //   line.sortingLayerName = "OnTop";
        line.sortingOrder = 5;
        line.positionCount = points.Length;
        line.SetPositions(points);
        line.widthMultiplier = 0.006f;
        line.useWorldSpace = true;
        line.material = LineMaterial;
    
    }

    //Can't figure out how to attach these correctly to HoverBegin if the parameter isn't passed until runtime.
    //which is why the specific node is hard coded here.
    public void EnlargeNode()
    {
       GameObject objectToChange = GameObject.Find("point2_2");
       objectToChange.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);

        Renderer rend = objectToChange.GetComponent<Renderer>();
        //Set the main Color of the Material to green
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.red);


    }

    public void ShrinkNode()
    {
        GameObject objectToChange = GameObject.Find("point2_2");
        objectToChange.transform.localScale -= new Vector3(0.5f, 0.5f, 0.5f);
        Renderer rend = objectToChange.GetComponent<Renderer>();
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.blue);

    }
   
    //hence this bullshit

   


}
