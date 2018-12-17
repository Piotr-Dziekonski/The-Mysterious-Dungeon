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
    bool orButtonpressed;
    //public GateController defaultController;

    

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        isOpen = false;
        orButtonpressed = false;
        //defaultController = this;
    }
	
	// Update is called once per frame
	void Update () {
        if(isOpen)
            animator.SetBool("isOpen", true);
        else
            animator.SetBool("isOpen", false);

        buttonsPressed = 0;
        notResetableButtonsPressed = 0;
        orButtonpressed = false;

        foreach (GameObject g in connectedTargets)
        {
            if (g.GetComponent<TargetController>().pressed)
            {
                buttonsPressed++;
            }

        }
        foreach (GameObject g in connectedButtons)
        {
            ButtonController controller = g.GetComponent<ButtonController>();
            
            if (controller.pressed)
            {
                if (controller.orButton)
                {
                    orButtonpressed = true;
                }

                buttonsPressed++;
                if (!controller.resetable)
                {
                    notResetableButtonsPressed++;
                }
            }
            
        }
        if ((buttonsPressed == connectedButtons.Length + connectedTargets.Length && connectedButtons.Length + connectedTargets.Length != 0) || orButtonpressed)
        {
            isOpen = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;

        }
        else if (buttonsPressed != connectedButtons.Length + connectedTargets.Length && connectedButtons.Length + connectedTargets.Length != 0)
        {
            isOpen = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
    
}
