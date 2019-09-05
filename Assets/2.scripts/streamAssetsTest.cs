using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class streamAssetsTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        print("Streaming Assets Path: " + Application.streamingAssetsPath);
        FileInfo[] allFiles = directoryInfo.GetFiles("*.*");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
