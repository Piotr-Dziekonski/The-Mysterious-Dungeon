using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterController : MonoBehaviour {

    public GameObject[] connectedButtons, connectedBlockTargets, connectedEntrances;
    public int buttonsPressed = 0;
    public bool disappearOnZero;
    public GameObject elementToSetActive;
    private Animator animator;
    public GateController[] gatesToOpen;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        buttonsPressed = 0;
        foreach (GameObject g in connectedEntrances)
        {
            if(g.GetComponent<LevelEntranceController>().completed)
            {
                buttonsPressed++;
            }
        }
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
        
        
        if (buttonsPressed == connectedButtons.Length + connectedBlockTargets.Length + connectedEntrances.Length)
        {
            if (disappearOnZero)
            {
                gameObject.SetActive(false);
                //Destroy(gameObject);

            }
            if (elementToSetActive != null)
            {
                elementToSetActive.SetActive(true);
                
            }
            if (gatesToOpen.Length > 0)
            {
                foreach(GateController gate in gatesToOpen)
                {
                    gate.isOpen = true;
                }
                

            }

        }
        if (gameObject.activeSelf)
        {
            animator.SetInteger("count", connectedButtons.Length + connectedBlockTargets.Length + connectedEntrances.Length - buttonsPressed);
        }
	}
}
