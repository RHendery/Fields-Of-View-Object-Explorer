using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
            loadedMetaDataObjects = new GameObject[loadedData.Length];

            for (int i = 0; i < loadedData.Length; i++)
            {
                print(loadedData[i].type);

                // the position should be radius of infoWisdom
                // position on ARC from date remapped to degrees

                // what is the radius?
                float positionradius = loadedData[i].infoWisdom * 2;

                // what is our remapped date to a degree
                float degree = helpers.Remap(loadedData[i].date, lowestDate, highestDate, 0, 180);

                // what is the position on a circumference?
                //(x, y) = (12 * sin(115), 12 * cos(115))
                float xPosOnradius = positionradius * Mathf.Sin(degree);
                float zPosOnradius = positionradius * Mathf.Cos(degree);
                float yPos = this.transform.position.y;

                //GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(transform.position.x, transform.position.y, transform.position.z + loadedData[i].infoWisdom), Quaternion.identity);
                GameObject thisMetaDataObject = Instantiate(metaDataSphere, new Vector3(xPosOnradius, yPos, zPosOnradius), Quaternion.identity);

                thisMetaDataObject.transform.localScale = new Vector3(loadedData[i].importance, loadedData[i].importance, loadedData[i].importance);

                // are we an audio type?
                if(loadedData[i].type == 1)
                {
                    print(loadedData[i].fileName);
                    AudioSource sphereAudioSource = thisMetaDataObject.GetComponent<AudioSource>();
                    sphereAudioSource.clip = Resources.Load<AudioClip>(loadedData[i].fileName); ;
                }

                // save this object in the array
                loadedMetaDataObjects[i] = thisMetaDataObject;
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







