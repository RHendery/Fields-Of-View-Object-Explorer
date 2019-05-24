using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachCard : MonoBehaviour
{
    public GameObject attachmentPoint;
    public GameObject indexCard;
    GameObject theInstatiatedIndexCard;
    public float zoneParameter;

    // Start is called before the first frame update
    void Start()
    {
      if(attachmentPoint == null)
        {
            print("you need to drag an attachment point in the inspector");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attachTheCard(string lower_upper)
    {
        string[] theParams = lower_upper.Split('~');
        print(theParams[0]);
        print(theParams[1]);

        if (zoneParameter >= float.Parse(theParams[0]) && zoneParameter <= float.Parse(theParams[1]))
        {
            theInstatiatedIndexCard = Instantiate(indexCard, attachmentPoint.transform.position, attachmentPoint.transform.rotation);
            theInstatiatedIndexCard.transform.parent = attachmentPoint.transform;
        }
    }

    public void detachTheCard()
    {
        Destroy(theInstatiatedIndexCard);
    }
}
