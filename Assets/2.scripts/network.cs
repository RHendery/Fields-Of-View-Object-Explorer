using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class network : MonoBehaviour
{
   
    //LET'S DECLARE A BUNCH OF STUFF

    public GameObject sphere;
    // System.IO.StreamReader objectfile = new System.IO.StreamReader(@"C: \Users\DHRG\Desktop\objects.csv"); // I suspect this isn't the ideal way to do this - should probably use "using" instead?
    // System.IO.StreamReader edgesfile = new System.IO.StreamReader(@"C: \Users\DHRG\Desktop\edgelist.csv");
      

    struct DataPoint //creating a new thing here so that we can easily add other properties for whatever columns of the spreadsheet we end up having
    {
       public string id;
       public string label;
    }

    struct Edge //apparently Unity hasn't heard of Tuples so I'm rolling my own
    {
        public string source; //the id of the source
        public string target; // the id of the target
    }

    

    // BUILD THE NETWORK ON INITALISING A LINE GAMEOBJECT, SURE WHY NOT?
    void Start()
    {

        List<DataPoint> objectList = readObjectFile(); //get list of museum object names and ids from csv file on Desktop (can change location above) 
        

        for (int i = 0; i < objectList.Count; i++) //for each item in the datapoints list, add a sphere to the world.
        {

            GameObject temp = Instantiate(sphere, new Vector3(Random.Range(-2, -15), Random.Range(0, 5), Random.Range(-5, 5)), Quaternion.identity); //for now generating them in random locations that should be visible
            temp.name = string.Concat(objectList[i].id, "_node"); //give it a name we can refer to later - this is the museum's object ID number, e.g. H6653-3 or similar. Guaranteed unique, unlike the label, plus "_node" otherwise might be the same as the actual 3D object in the space if we are also using the id numbers for names there.
            Renderer rend = temp.GetComponent<Renderer>(); //this is just to make sure it's the same blue we will change it back to later in the flashNodes script.
            rend.material.SetColor("_Color", Color.blue);

            //maybe put some code in here that checks whether there is an object in the game space that has the same id number and attaches other spreadsheet information to that object somehow.
        } 


        List<Edge> edgeList = readEdgesFile(); //see below
       


        
        for (int i = 0; i < edgeList.Count; i++) //for each pair in the edge list, find the location of each of the objects and put them into a 2-array of vector3s, and send it to SetUpLine()
        {
            var tempV3List = new Vector3[2];

            tempV3List[0] = GameObject.Find(string.Concat(edgeList[i].source, "_node")).transform.position;
            tempV3List[1] = GameObject.Find(string.Concat(edgeList[i].target, "_node")).transform.position;

            SetupLine(tempV3List, string.Concat("line_", i));
        }

    }

 
    void Update()   // Update is called once per frame
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

    List<DataPoint> readObjectFile()  //this grabs the objects from the objectfile and puts them into a list
    {
        string sLine;
        
        List<DataPoint> aDataPoints = new List<DataPoint>();
        System.IO.StreamReader objectfile = new System.IO.StreamReader(string.Concat(Application.dataPath, @"\objects.csv")); // I suspect this isn't the ideal way to do this - should probably use "using" instead?

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

        System.IO.StreamReader edgesfile = new System.IO.StreamReader(string.Concat(Application.dataPath, @"\edgelist.csv"));


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
