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

        Frame frame = controller.Frame(); // controller is a Controller object
      
        if (frame.Hands.Count > 0)
        {
            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];
            Hand secondHand = hands[1];

            previousPosition = currentPosition;
            currentPosition = firstHand.PalmPosition.x - secondHand.PalmPosition.x; //this will increase as they move away from each other.       
            
        }

    }

    public void EnlargeObject()
    {
          Debug.Log(gameObject.GetComponent<InteractionBehaviour>().graspingHands.Count);
        var numberOfHands = gameObject.GetComponent<InteractionBehaviour>().graspingHands.Count;

        if ( numberOfHands > 1 && previousPosition < currentPosition) ; //while hands are moving apart
        {
            Debug.Log("entered loop");
            Debug.Log(gameObject.GetComponent<InteractionBehaviour>().graspingHands.Count);
            gameObject.transform.localScale += new Vector3(incrementSize, incrementSize, incrementSize);
           //    Debug.Log(string.Concat("Previously: " , previousPosition, "; Currently: ", currentPosition));
        }

        if (previousPosition > currentPosition && numberOfHands > 1) //while hands are moving closer
        {
            gameObject.transform.localScale -= new Vector3(incrementSize, incrementSize, incrementSize);
        }



    }

    public void ShrinkObject()
    {
       
       

    }
}
