using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity.Interaction;

public class resizeObject : MonoBehaviour
{
    float incrementSize = 0.01f; //increment to increase/decrease by. Change to speed up or slow down
    Controller controller;
    float previousPosition =0;
    float currentPosition = 0;


    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
    }

    // Update is called once per frame
    void Update()
    {

       

    }

    public void EnlargeObject() //this will be called once per frame because it's running on the "graspstay" interaction
    {
        Frame frame = controller.Frame(); // controller is a Controller object

        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];
            Hand secondHand = hands[1];

            previousPosition = currentPosition;
            currentPosition = secondHand.PalmPosition.x - firstHand.PalmPosition.x; //this will increase as they move away from each other.       

            var numberOfHands = gameObject.GetComponent<InteractionBehaviour>().graspingHands.Count;
            Debug.Log(numberOfHands);

            if ( (numberOfHands > 1) ); //while hands are moving apart
            {
                if (previousPosition < currentPosition)
                { 
                    Debug.Log("entered grow loop");
                    Debug.Log(gameObject.GetComponent<InteractionBehaviour>().graspingHands.Count);
                    gameObject.transform.localScale += new Vector3(incrementSize, incrementSize, incrementSize);
                    //       Debug.Log(string.Concat("Previously: " , previousPosition, "; Currently: ", currentPosition));
                }
            
                Debug.Log("exited grow loop");

                if(previousPosition > currentPosition) //while hands are moving closer
                {
                    Debug.Log("entered shrink loop");
                    gameObject.transform.localScale -= new Vector3(incrementSize, incrementSize, incrementSize); 

                }
                Debug.Log("exited shrink loop");
            }
        } 
        
    }

    public void ShrinkObject()
    {
       
       

    }
}
