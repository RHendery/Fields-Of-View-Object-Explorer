using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class readMetaData : MonoBehaviour
{
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
    }

    public void pickUp()
    {
        loadData();
    }

    public void drop()
    {
        unloadData();
    }

    private void loadData()
    {
        //file path
        string dataFilePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(dataFilePath))
        {
            string dataAsJson = File.ReadAllText(dataFilePath);
            objectMetaData[] loadedData = JsonHelper.FromJson<objectMetaData>(dataAsJson);

            // keep record of all obejcts we create so we can get ridf of them easily
            // this may cause issues with garbage collection, but lets see
            loadedMetaDataObjects = new GameObject[loadedData.Length * 2];

            for (int i = 0; i < loadedData.Length; i++)
            {
                print(loadedData[i].type);

                // the position should be radius of infoWisdom
                // position on ARC from date remapped to degrees

                // what is the radius?
                //float positionradius = loadedData[i].infoWisdom * 2;
                // map info wisdom range from 0-1 to 0-2
                float positionradius = helpers.Remap(loadedData[i].infoWisdom, 0, 1, 0, 2);

                // what is our remapped date to a degree
                float degree = helpers.Remap(loadedData[i].date, lowestDate, highestDate, -90,90) + Mathf.Abs(Camera.main.transform.eulerAngles.y);
                print("camera Angle is: " + Camera.main.transform.eulerAngles.y / 2);

                // what is the position on a circumference?
                // need to make relative to the object when touched
                //(x, y) = (12 * sin(115), 12 * cos(115))
                float xPosOnradius = positionradius * Mathf.Sin(degree * Mathf.Deg2Rad) + transform.position.x;
                float zPosOnradius = positionradius * Mathf.Cos(degree * Mathf.Deg2Rad) + transform.position.z;
                float yPos = this.transform.position.y; // + 0.5f;

                //GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(transform.position.x, transform.position.y, transform.position.z + loadedData[i].infoWisdom), Quaternion.identity);
                GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(xPosOnradius, yPos, zPosOnradius), Quaternion.identity);

                //instatiate a ring as a visual clue
                GameObject visualRing = Instantiate(ring, new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity);
                visualRing.transform.localScale = new Vector3(positionradius, 0.01f, positionradius);

                thisMetaDataObject.transform.localScale = new Vector3(loadedData[i].importance/2, loadedData[i].importance/2, loadedData[i].importance/2);

                // are we an audio type?
                if(loadedData[i].type == 1)
                {
                    print(loadedData[i].fileName);
                    AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                    sphereAudioSource.clip = Resources.Load<AudioClip>(loadedData[i].fileName);

                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = loadedData[i].text;
                }

                if(loadedData[i].type == 2)
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

                if(loadedData[i].type == 3)
                {
                    TextMeshPro sphereText = thisMetaDataObject.transform.GetChild(1).gameObject.GetComponent<TextMeshPro>();
                    sphereText.text = loadedData[i].text;
                }

                // save this object in the array
                loadedMetaDataObjects[i] = thisMetaDataObject;
                loadedMetaDataObjects[i + loadedData.Length] = visualRing;
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
}







