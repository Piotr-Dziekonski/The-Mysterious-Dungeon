using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockController : MonoBehaviour {

    public GameObject[] connectedButtons;
    public GameObject[] connectedBlockTargets;
    public int buttonsPressed = 0;
    public bool disappearOnZero;
    public GameObject elementToSetActive;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        buttonsPressed = 0;
        foreach (GameObject g in connectedButtons)
        {
            if(g.GetComponent<ButtonController>().pressed)
            {
                buttonsPressed++;
            }
        }

            foreach (GameObject g in connectedBlockTargets)
            {
                if (g.GetComponent<TargetController>().pressed)
                {
                    buttonsPressed++;
                }
            }
        
        
        if (buttonsPressed == connectedButtons.Length + connectedBlockTargets.Length)
        {
            if (disappearOnZero)
            {
                Destroy(gameObject);

            }
            if (elementToSetActive != null)
            {
                elementToSetActive.SetActive(true);
            }

        }
        animator.SetInteger("count", connectedButtons.Length + connectedBlockTargets.Length - buttonsPressed);
	}
}
