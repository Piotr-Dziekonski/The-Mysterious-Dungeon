using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    public bool pressed;

    public GameObject standingBlock;
    public GameObject neededBlock;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(standingBlock == neededBlock)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
	}
}
