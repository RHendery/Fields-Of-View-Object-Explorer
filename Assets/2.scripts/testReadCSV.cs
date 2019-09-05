// https://github.com/tikonen/blog/tree/master/csvreader

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testReadCSV : MonoBehaviour
{
    void Awake()
    {

        List<Dictionary<string, object>> data = CSVReader.Read("SingleObjectData");

        for (var i = 0; i < data.Count; i++)
        {
            print("number " + data[i]["itemNumber"] + " " +
                   "text " + data[i]["text"]);
        }

    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
