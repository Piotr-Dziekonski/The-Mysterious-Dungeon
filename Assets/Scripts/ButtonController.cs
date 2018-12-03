using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour {
    public bool pressed = false;
    public GameObject standingPlayer;
    public bool resetable;
    private Animator animator;
    public bool allowSound = false;
    
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        
        animator.SetBool("pressed", pressed);



        if (!resetable)
        {
            if(standingPlayer != null)
                pressed = true;
        }
	}

}
