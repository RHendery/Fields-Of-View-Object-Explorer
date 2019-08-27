using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class readMetaData : MonoBehaviour
{
    // file name of language json data - must be in streaming assets folder
    public string filename = "languageData.json";

    // game objects for building environment
    [Header("GO represents single language location")]
    public GameObject languageCentroidMarker;
    [Header("GO represents language bounding box")]
    public GameObject languageBoundsCube;
    [Header("GO represents language with related audio")]
    public GameObject audioIcon;
    [Header("GO represents audio bounds in environment")]
    public GameObject ring;

    // holders for the map scale set in start method
    private int scaleX; 
    private int scaleY;

    // array to read json language data into
    private objectMetaData[] metaData;

    // Use this for initialization
    void Start()
    {
        loadData();
    }

    private void loadData()
    {
        //file path
        string dataFilePath = Path.Combine(Application.streamingAssetsPath, filename);

        if (File.Exists(dataFilePath))
        {
            string dataAsJson = File.ReadAllText(dataFilePath);
            objectMetaData[] loadedData = JsonHelper.FromJson<objectMetaData>(dataAsJson);

            for (int i = 0; i < loadedData.Length; i++)
            {
            
            }
        }
    }
}







