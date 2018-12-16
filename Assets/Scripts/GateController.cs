using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour {

    public GameObject[] connectedButtons, connectedTargets;

    //[HideInInspector]
    public bool isOpen;
    private GameObject playerInGate;
    public int buttonsPressed = 0, notResetableButtonsPressed = 0;
    private Animator animator;
    //public GateController defaultController;

    

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        isOpen = false;
        //defaultController = this;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(isOpen)
            animator.SetBool("isOpen", true);
        else
            animator.SetBool("isOpen", false);

        buttonsPressed = 0;
        notResetableButtonsPressed = 0;

        foreach (GameObject g in connectedTargets)
        {
            if (g.GetComponent<TargetController>().pressed)
            {
                buttonsPressed++;
            }

        }
        foreach (GameObject g in connectedButtons)
        {
            if (g.GetComponent<ButtonController>().pressed)
            {
                buttonsPressed++;
                if (!g.GetComponent<ButtonController>().resetable)
                {
                    notResetableButtonsPressed++;
                }
            }
            
        }
        if (buttonsPressed == connectedButtons.Length + connectedTargets.Length && connectedButtons.Length + connectedTargets.Length != 0)
        {
            //Destroy(gameObject);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            isOpen = true;
        }
        else if (buttonsPressed != connectedButtons.Length + connectedTargets.Length && connectedButtons.Length + connectedTargets.Length != 0)
        {
            isOpen = false;
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
        }
        if (isOpen)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    
}
