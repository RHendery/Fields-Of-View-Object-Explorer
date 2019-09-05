using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class readMetaDataCSV : MonoBehaviour
{
    public bool debug = false;
    public bool flipAxes = false;
    public bool presentOnGrid = false;
    public bool stepLayout = false;
    public float arcTheta = 180;
    public float upperRadius = 2;
    public Material lineMaterial;

    public LineRenderer theLineRenderer;
    
    // the CSV file name does not have file extension
    // currently file should be in Resources folder
    public string CSVFileName = "SingleObjectData";

    // list to hold data
    List<Dictionary<string, object>> data; 

    // highest and lowest dates. This should be common accros an object set
    // should store as a static variable for the whole project
    public float lowestDate;
    public float highestDate;

    // game objects for building environment
    [Header("GO to reprasent a single piece of metadata")]
    public GameObject metaDataSphere;
    public GameObject ring;

    private GameObject[] loadedMetaDataObjects;

    void Awake()
    {
        data = CSVReader.Read("SingleObjectData");
    }

    // Use this for initialization
    void Start()
    {
        if (debug)
        {
            loadData();
        }
      //drawArc();
    }

    public void pickUp()
    {
        loadData();
        //drawArc();
    }

    public void drop()
    {
        unloadData();
    }

    private void loadData()
    {

        // keep record of all obejcts we create so we can get ridf of them easily
        // this may cause issues with garbage collection, but lets see
        // seems to be ok so far? maybe check on the profiler
        loadedMetaDataObjects = new GameObject[data.Count * 2];

        // iterate through each item of metadata 
        // in the list created from the CSV
        for (int i = 0; i < data.Count; i++)
        {
            if (presentOnGrid == false)
            {
                // set axis data
                // radiusAxisData is the data that sets radius of the arc.
                float radiusAxisData;
                // arcPositionData is the data that sets a relative position on the arc.
                float arcPositionData;

                // everything should be re-mapped to 0 - 1
                // info wisdom already is 0 - 1
                // but date needs too be remapped using lowestDate and highestDate public fields.
                float remappedDate = helpers.Remap(float.Parse(data[i]["date"].ToString()), lowestDate, highestDate, 0, 1);

                // what should map to what
                if (flipAxes == false)
                {
                    radiusAxisData = float.Parse(data[i]["infoWisdom"].ToString());
                    arcPositionData = remappedDate;
                }
                else
                {
                    radiusAxisData = remappedDate;
                    arcPositionData = float.Parse(data[i]["infoWisdom"].ToString());
                }

                // what is the radius of our arc?
                // map info wisdom range from 0-1 to 0 to upperRadius (set in inspector)
                float positionradius = helpers.Remap(radiusAxisData, 0, 1, 0, upperRadius);

                // what is our remapped data as a degree 
                // half of arcTheta on either side giving an arc of arcTheta range
                // adding the camera Y rotation to offset the whole arc by the rotation of the camera
                float degree = helpers.Remap(arcPositionData, 0, 1, -arcTheta / 2, arcTheta / 2) + Mathf.Abs(Camera.main.transform.eulerAngles.y);

                // what is the position on a circumference?
                // need to make relative to the object when touched
                // we do this by adding the objects position to the x & z
                //(x, y) = (radius * sin(radians), radius * cos(radians))
                float xPosOnradius = positionradius * Mathf.Sin(degree * Mathf.Deg2Rad) + transform.position.x;
                float zPosOnradius = positionradius * Mathf.Cos(degree * Mathf.Deg2Rad) + transform.position.z;
                float yPos = this.transform.position.y; // + 0.5f;

                if (stepLayout)
                {
                    yPos = yPos + positionradius / 2;
                }

                // instatiate the metadata object
                GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(xPosOnradius, yPos, zPosOnradius), Quaternion.identity);

                // rename object so we can find it easily
                thisMetaDataObject.transform.name = data[i]["objectName"].ToString() + data[i]["itemNumber"].ToString();

                // instatiate a ring as a visual clue
                GameObject visualRing = Instantiate(ring, new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity);
                visualRing.transform.localScale = new Vector3(positionradius, 0.01f, positionradius);

                // set scale of object using importance value
                thisMetaDataObject.transform.localScale = new Vector3(float.Parse(data[i]["importance"].ToString()) / 2, float.Parse(data[i]["importance"].ToString()) / 2, float.Parse(data[i]["importance"].ToString()) / 2);

                // create the path to the assets we will need
                string assetPath = data[i]["objectName"].ToString() + "/" + data[i]["fileName"].ToString();
                    
                // are we an audio type?
                if (float.Parse(data[i]["type"].ToString()) == 1)
                {
                    //print(data[i]["fileName"]);
                    AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                    sphereAudioSource.clip = Resources.Load<AudioClip>(assetPath);

                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();
                }

                // are we a image type?
                if (float.Parse(data[i]["type"].ToString()) == 2)
                {  
                    if (Resources.Load<Texture2D>(assetPath) != null)
                    {
                        GameObject SphereChild = thisMetaDataObject.transform.GetChild(0).gameObject;
                        Renderer sphereChildRenderer = SphereChild.GetComponent<Renderer>();
                        sphereChildRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(assetPath));
                        Texture2D thisTexture = Resources.Load<Texture2D>(assetPath);

                        float ratio;
                        if(thisTexture.width > thisTexture.height)
                        {
                            ratio = thisTexture.width / thisTexture.height;
                            SphereChild.gameObject.transform.localScale = new Vector3(-1,ratio, -0.01f);
                        }
                        else
                        {
                            ratio = thisTexture.height / thisTexture.width;
                            SphereChild.gameObject.transform.localScale = new Vector3(-ratio, 1, -0.01f);
                        }
                        SphereChild.gameObject.transform.localScale = new Vector3(-thisTexture.width / 1000, thisTexture.height / 1000, -0.01f);


                        // hide sphere
                        thisMetaDataObject.GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        thisMetaDataObject.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();
                }
                else
                {
                    Destroy(thisMetaDataObject.transform.GetChild(0).gameObject);
                }

                // or are we an abitrary text type?
                if (float.Parse(data[i]["type"].ToString()) == 3)
                {
                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();

                    // hide sphere
                    thisMetaDataObject.GetComponent<Renderer>().enabled = false;
                }

                // save this object in the array
                loadedMetaDataObjects[i] = thisMetaDataObject;
                loadedMetaDataObjects[i + data.Count] = visualRing;
            }

            /// ---------
            /// present on grid code
            /// ---------
            
            else if (presentOnGrid)
            {
                float remappedDate = helpers.Remap(float.Parse(data[i]["date"].ToString()), lowestDate, highestDate, -2f, 2f);
                float infoWisdomScale = helpers.Remap(float.Parse(data[i]["infoWisdom"].ToString()), 0, 1, 0, 4);

                GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(remappedDate, infoWisdomScale, 1), Quaternion.identity);

                thisMetaDataObject.transform.localScale = new Vector3(float.Parse(data[i]["importance"].ToString()) / 2, float.Parse(data[i]["importance"].ToString()) / 2, float.Parse(data[i]["importance"].ToString()) / 2);

                // create the path to the assets we will need
                string assetPath = data[i]["objectName"].ToString() + "/" + data[i]["fileName"].ToString();

                // are we an audio type?
                if (float.Parse(data[i]["type"].ToString()) == 1)
                {
                    //print(data[i]["fileName"]);
                    AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                    sphereAudioSource.clip = Resources.Load<AudioClip>(assetPath);

                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();
                }

                // are we a image type?
                if (float.Parse(data[i]["type"].ToString()) == 2)
                {
                    if (Resources.Load<Texture2D>(assetPath) != null)
                    {
                        GameObject SphereChild = thisMetaDataObject.transform.GetChild(0).gameObject;
                        Renderer sphereChildRenderer = SphereChild.GetComponent<Renderer>();
                        sphereChildRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(assetPath));
                        // hide sphere
                        thisMetaDataObject.GetComponent<Renderer>().enabled = false;
                    }
                    else
                    {
                        thisMetaDataObject.transform.GetChild(0).gameObject.SetActive(false);
                    }

                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();
                }
                else
                {
                    Destroy(thisMetaDataObject.transform.GetChild(0).gameObject);
                }

                // or are we an abitrary text type?
                if (float.Parse(data[i]["type"].ToString()) == 3)
                {
                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = data[i]["text"].ToString();

                    // hide sphere
                    thisMetaDataObject.GetComponent<Renderer>().enabled = false;
                }

                loadedMetaDataObjects[i] = thisMetaDataObject;
            }
        }

        makeConnections();
    }

    private void unloadData()
    {
        foreach(GameObject obj in loadedMetaDataObjects)
        {
            Destroy(obj);
        }

        foreach (GameObject tagged in GameObject.FindGameObjectsWithTag("lineObject"))
        {
            Destroy(tagged);
        }

        loadedMetaDataObjects = new GameObject[0];
    }

    void drawArc()
    {
        List<Vector3> arcPoints = new List<Vector3>();
        float startAngle = -(arcTheta / 2) + Mathf.Abs(Camera.main.transform.eulerAngles.y); 
        float endAngle = (arcTheta / 2) + Mathf.Abs(Camera.main.transform.eulerAngles.y);
        print(startAngle + "  " + endAngle);
        int segments = 15;
        float radius = 2;

        float angle = startAngle;
        float arcLength = endAngle - startAngle;
        for (int i = 0; i <= segments; i++)
        {
            float x = (Mathf.Sin(Mathf.Deg2Rad * angle) * radius) + transform.position.x;
            float y = +transform.position.y;
            float z = (Mathf.Cos(Mathf.Deg2Rad * angle) * radius) + transform.position.z;

            arcPoints.Add(new Vector3(x, y, z));

            angle += (arcLength / segments);
        }

        Vector3[] arrayOfpositions = arcPoints.ToArray();
        theLineRenderer.positionCount = segments;
        theLineRenderer.SetPositions(arrayOfpositions);
    }

    void makeConnections()
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i]["parent"].ToString() != "")
            {
                string parentName = data[i]["objectName"].ToString() + data[i]["parent"].ToString();
                print("parent ---> " + parentName);
                GameObject myParent = GameObject.Find(parentName);
                string thisName = data[i]["objectName"].ToString() + data[i]["itemNumber"].ToString();
                print(thisName);
                GameObject thisObject = GameObject.Find(thisName);

                DrawLine(thisObject.transform.position, myParent.transform.position, Color.red, thisObject, 10);
            }
        }

        void DrawLine(Vector3 start, Vector3 end, Color color, GameObject myParent, float duration = 0.2f)
        {
            GameObject myLine = new GameObject();
            myLine.transform.tag = "lineObject";
            myLine.transform.parent = myParent.transform;
            myLine.transform.position = start;
            myLine.AddComponent<LineRenderer>();
            LineRenderer lr = myLine.GetComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.SetColors(new Color(0,0,0,0.25f), new Color(1, 0, 0, 0.25f));
            lr.SetWidth(0.02f, 0.02f);
            lr.positionCount = 3;

            Vector3 halfwayPoint = (start + end) / 2;
            Vector3 liftedHalfwayPoint = new Vector3(halfwayPoint.x, halfwayPoint.y + 1f, halfwayPoint.z);

            lr.SetPosition(0, start);
            lr.SetPosition(1, liftedHalfwayPoint);
            lr.SetPosition(2, end);
            //GameObject.Destroy(myLine, duration);
        }
    }
}







