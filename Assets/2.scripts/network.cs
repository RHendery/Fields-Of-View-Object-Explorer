using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network : MonoBehaviour
{
    public Material LineMaterial;
    string line1, line2, line3, line4;

    // Start is called before the first frame update
    void Start()
    {
        var points1 = new Vector3[3];
        points1[0] = new Vector3(0, 0, 0);
        points1[1] = new Vector3(-10, 10, 10);
        points1[2] = new Vector3(-10, 10, 0);

        SetupLine(points1, "line1");

        var points2 = new Vector3[3];
        points2[0] = new Vector3(10, 10, 10);
        points2[1] = new Vector3(-10, 10, 10);
        points2[2] = new Vector3(10, 0, 10);


        SetupLine(points2, "line2");
        //line3 = SetupLine();
        //line4 = SetupLine();
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



}
