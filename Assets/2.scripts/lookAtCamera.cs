/*
 * any game game object with this script attached to it will look for an object called 'player'
 * and continually 'look' towards it
 * used to keep in world text looking at the player camera front on
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour {

    Camera targetPlayer;

	// Use this for initialization
	void Start () {
        targetPlayer = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(targetPlayer.transform.position);
    }
}
