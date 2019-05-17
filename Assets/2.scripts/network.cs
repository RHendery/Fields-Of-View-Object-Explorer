using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network : MonoBehaviour
{
   
    string line1, line2, line3, line4;
    public GameObject sphere;
    GameObject monkey3_point, monkey2_point, monkey1_point;
    System.IO.StreamReader objectfile = new System.IO.StreamReader(@"C: \Users\DHRG\Desktop\objects.csv");
    System.IO.StreamReader edgesfile = new System.IO.StreamReader(@"C: \Users\DHRG\Desktop\edgelist.csv");

    struct DataPoint //creating a new thing here so that we can easily add other properties for whatever columns of the spreadsheet we end up having
    {
       public string id;
       public string label;
    }

    struct Edge //apparently Unity hasn't heard of Tuples so I'm rolling my own
    {
        public string source;
        public string target;
    }

    // Start is called before the first frame update
    void Start()  
    {

        List<DataPoint> objectList = readObjectFile(); //get list of museum object names and ids from csv file on Desktop (can change location above) 

        //for each item in the datapoints list, add a sphere.

        for(int i =0; i<objectList.Count; i++)
        {
            
            GameObject temp = Instantiate(sphere, new Vector3(Random.Range(-2, -15), Random.Range(0, 5), Random.Range(-5, 5)), Quaternion.identity); //for now generating them in random locations that should be visible
            temp.name = objectList[i].id; //give it a name we can refer to later - this is the museum's object ID number, e.g. H6653-3 or similar. Guaranteed unique, unlike the label
        }

        

        var points1 = new Vector3[3]; //I should really refactor this now I have it working
        points1[0] = new Vector3(-10, 5, -2);
        points1[1] = new Vector3(-15, 5, 2);
        points1[2] = new Vector3(-15, 5, 2);

        SetupLine(points1, "line1");


        //now we put some nodes at the intersections of the line segments
        List<Edge> edgeList = readEdgesFile();

        //ADD MORE CODE HERE. Note: somehow I've broken LEAP.
     
     
        for (int i = 1; i < points1.Length; i++)
        {
           GameObject s = Instantiate(sphere, points1[i], Quaternion.identity);
            s.name = string.Concat("point1_", i.ToString()); //give it a tag we can refer to later

            Renderer rend = s.GetComponent<Renderer>(); //this is just to make sure it's the same blue we will change it back to later in the flashNodes script.
            rend.material.SetColor("_Color", Color.blue);

         
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

        line.sortingOrder = 1;
        line.positionCount = points.Length;
        line.SetPositions(points);
        line.widthMultiplier = 0.002f;
        line.useWorldSpace = true;
        line.material = new Material(Shader.Find("Particles/Standard Unlit"));
        line.material.SetColor("_Color", Color.blue);

    }

    List<DataPoint> readObjectFile() 
    {

        string sLine;
        
        List<DataPoint> aDataPoints = new List<DataPoint>();

        // First line in the file is headings so skip over it
        sLine = objectfile.ReadLine();

        while ((sLine = objectfile.ReadLine()) != null) //read until the end of file
        {
          
            string[] fields = sLine.Split(',');

             //construct a list of Datapoints
                DataPoint tempDataPoint;
                tempDataPoint.id = fields[0];
                tempDataPoint.label = fields[1];
                aDataPoints.Add(tempDataPoint);
        }
        objectfile.Close();

        return aDataPoints;
    }


    List<Edge> readEdgesFile()  //I don't think it's very elegant to have this separate instead of combining with readObjectFile() but I can't see an easy way to refactor and I'm lazy
    {

        string sLine;

        List<Edge> networkEdgeList = new List<Edge>();

        // First line in the file is headings so skip over it
        sLine = edgesfile.ReadLine();

        while ((sLine = edgesfile.ReadLine()) != null) //read until the end of file
        {

            string[] fields = sLine.Split(',');

            //construct a list of string tuples from the sources and targets
            Edge tempEdge;
            tempEdge.source = fields[0];
            tempEdge.target = fields[1];
            networkEdgeList.Add(tempEdge);
        }
        edgesfile.Close();

        return networkEdgeList;
    }


}
