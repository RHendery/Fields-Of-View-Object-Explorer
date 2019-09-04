using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class readMetaData : MonoBehaviour
{
    public bool flipAxes = false;
    public bool presentOnGrid = false;
    public bool stepLayout = false;
    public float arcTheta = 180;
    public LineRenderer theLineRenderer;
    
    // file name of language json data - must be in streaming assets folder
    public string filename = "languageData.json";

    // highest and lowest dates. This should be common accros an object set
    // should store as a static variable for the whole project
    public float lowestDate;
    public float highestDate;

    // game objects for building environment
    [Header("GO represents single piece of metadata")]
    public GameObject metaDataSphere;

    public GameObject ring;

    // array to read json language data into
    private objectMetaData[] metaData;


    private GameObject[] loadedMetaDataObjects;

    // Use this for initialization
    void Start()
    {
      //loadData();
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
        // create file path
        string dataFilePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(dataFilePath))
        {
            // read data in
            string dataAsJson = File.ReadAllText(dataFilePath);
            objectMetaData[] loadedData = JsonHelper.FromJson<objectMetaData>(dataAsJson);

            // keep record of all obejcts we create so we can get ridf of them easily
            // this may cause issues with garbage collection, but lets see
            // seems to be ok so far? maybe check on the profiler
            loadedMetaDataObjects = new GameObject[loadedData.Length * 2];

            // iterate through each item of metadata
            for (int i = 0; i < loadedData.Length; i++)
            {
                if (presentOnGrid == false)
                {
                    // set axis data
                    // radiusAxisData is the data that sets radius of the arc.
                    float radiusAxisData;
                    // arcPositionData is the data that sets a relative position on the arc.
                    float arcPositionData;

                    // everything mapped to 0 - 1
                    // info wisdom already is 0 - 1
                    // but date needs too be remapped using lowestDate and highestDate public fields.
                    float remappedDate = helpers.Remap(loadedData[i].date, lowestDate, highestDate, 0, 1);

                    // what should map to what
                    if (flipAxes == false)
                    {
                        radiusAxisData = loadedData[i].infoWisdom;
                        arcPositionData = remappedDate;
                    }
                    else
                    {
                        radiusAxisData = remappedDate;
                        //radiusAxisData = helpers.Remap(radiusAxisData, lowestDate, highestDate, 0, 1);
                        arcPositionData = loadedData[i].infoWisdom;
                        //arcPositionData = helpers.Remap(arcPositionData, 0, 1, lowestDate, highestDate);
                    }

                    // the position should be radius of infoWisdom
                    // position on ARC from date remapped to degrees

                    // what is the radius?
                    //float positionradius = loadedData[i].infoWisdom * 2;
                    // map info wisdom range from 0-1 to 0-2
                    float positionradius = helpers.Remap(radiusAxisData, 0, 1, 0, 2);

                    // what is our remapped date to a degree
                    float degree = helpers.Remap(arcPositionData, 0, 1, -arcTheta / 2, arcTheta / 2) + Mathf.Abs(Camera.main.transform.eulerAngles.y);
                    print("camera Angle is: " + Camera.main.transform.eulerAngles.y / 2);

                    // what is the position on a circumference?
                    // need to make relative to the object when touched
                    //(x, y) = (12 * sin(115), 12 * cos(115))
                    float xPosOnradius = positionradius * Mathf.Sin(degree * Mathf.Deg2Rad) + transform.position.x;
                    float zPosOnradius = positionradius * Mathf.Cos(degree * Mathf.Deg2Rad) + transform.position.z;
                    float yPos = this.transform.position.y; // + 0.5f;

                    if (stepLayout)
                    {
                        yPos = yPos + positionradius / 2;
                    }

                    //GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(transform.position.x, transform.position.y, transform.position.z + loadedData[i].infoWisdom), Quaternion.identity);
                    GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(xPosOnradius, yPos, zPosOnradius), Quaternion.identity);

                    //instatiate a ring as a visual clue
                    GameObject visualRing = Instantiate(ring, new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity);
                    visualRing.transform.localScale = new Vector3(positionradius, 0.01f, positionradius);

                    thisMetaDataObject.transform.localScale = new Vector3(loadedData[i].importance / 2, loadedData[i].importance / 2, loadedData[i].importance / 2);

                    // are we an audio type?
                    if (loadedData[i].type == 1)
                    {
                        print(loadedData[i].fileName);
                        AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                        sphereAudioSource.clip = Resources.Load<AudioClip>(loadedData[i].fileName);

                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }

                    if (loadedData[i].type == 2)
                    {
                        /*Renderer childRenderer = thisMetaDataObject.GetComponentInChildren<Renderer>();
                        print(loadedData[i].fileName);
                        childRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(loadedData[i].fileName));
                        */
                        if (Resources.Load<Texture2D>(loadedData[i].fileName) != null)
                        {
                            GameObject SphereChild = thisMetaDataObject.transform.GetChild(0).gameObject;
                            Renderer sphereChildRenderer = SphereChild.GetComponent<Renderer>();
                            sphereChildRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(loadedData[i].fileName));
                        }
                        else
                        {
                            thisMetaDataObject.transform.GetChild(0).gameObject.SetActive(false);
                        }


                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }
                    else
                    {
                        Destroy(thisMetaDataObject.transform.GetChild(0).gameObject);
                    }

                    if (loadedData[i].type == 3)
                    {
                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }

                    // save this object in the array
                    loadedMetaDataObjects[i] = thisMetaDataObject;
                    loadedMetaDataObjects[i + loadedData.Length] = visualRing;
                }

                else if (presentOnGrid)
                {
                    float remappedDate = helpers.Remap(loadedData[i].date, lowestDate, highestDate, -2f, 2f);
                    float infoWisdomScale = helpers.Remap(loadedData[i].infoWisdom, 0, 1, 0, 4);

                    GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(remappedDate, infoWisdomScale, 1), Quaternion.identity);

                    thisMetaDataObject.transform.localScale = new Vector3(loadedData[i].importance / 2, loadedData[i].importance / 2, loadedData[i].importance / 2);

                    // are we an audio type?
                    if (loadedData[i].type == 1)
                    {
                        print(loadedData[i].fileName);
                        AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                        sphereAudioSource.clip = Resources.Load<AudioClip>(loadedData[i].fileName);

                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }

                    if (loadedData[i].type == 2)
                    {
                        /*Renderer childRenderer = thisMetaDataObject.GetComponentInChildren<Renderer>();
                        print(loadedData[i].fileName);
                        childRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(loadedData[i].fileName));
                        */

                        GameObject SphereChild = thisMetaDataObject.transform.GetChild(0).gameObject;
                        Renderer sphereChildRenderer = SphereChild.GetComponent<Renderer>();
                        sphereChildRenderer.material.SetTexture("_MainTex", Resources.Load<Texture2D>(loadedData[i].fileName));

                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }
                    else
                    {
                        Destroy(thisMetaDataObject.transform.GetChild(0).gameObject);
                    }

                    if (loadedData[i].type == 3)
                    {
                        TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                        sphereText.text = loadedData[i].text;
                    }

                    loadedMetaDataObjects[i] = thisMetaDataObject;
                }
            }
        }
    }

    private void unloadData()
    {
        foreach(GameObject obj in loadedMetaDataObjects)
        {
            Destroy(obj);
        }

        loadedMetaDataObjects = new GameObject[0];
    }

    void drawArc()
    {
        List<Vector3> arcPoints = new List<Vector3>();
        //arcTheta = arcTheta + Mathf.Abs(Camera.main.transform.eulerAngles.y);
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
}







