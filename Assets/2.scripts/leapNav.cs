using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;

public class leapNav : MonoBehaviour {

    Controller controller;

    // Use this for initialization
    void Start () {
        controller = new Controller();
    }
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        if (frame.Hands.Count > 0)
        {
            //print(frame.Hands[0]);
            if (frame.Hands[0].IsRight)
            {
                if (frame.Hands[0].Fingers[1].IsExtended)
                {
                    //print("right index finger extended");
                }
                else
                {
                    //print("not");
                }
            }

            print(frame.Hands[0].PalmNormal);
            
                
            if (frame.Hands[0].PalmNormal[0] > -0.5 && frame.Hands[0].PalmNormal[0] < 0.5 && frame.Hands[0].PalmNormal[1] < 0)
            {
                print("palms up");
            }
            else
            {
                print("not");
            }
            
        }
    }
}
