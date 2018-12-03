using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour {

    public bool stateOn;
    private int leverCooldown = GameController.buttonCooldown;
    public GameObject[] gates;
    private Animator animator;
    public bool wasPulled;


	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        stateOn = false;
        wasPulled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (leverCooldown > 0)
            leverCooldown--;

        CheckDirection(-0.48f, 0f, "LEFT");
        CheckDirection(0.48f, 0f, "RIGHT");
        CheckDirection(0f, 0.48f, "UP");
        CheckDirection(0f, -0.48f, "DOWN");

        animator.SetBool("isOn", stateOn);

        foreach(GameObject gate in gates)
        {
            gate.GetComponent<GateController>().isOpen = stateOn;
            gate.SetActive(!stateOn);
        }
        
        

    }
    private void LateUpdate()
    {
        wasPulled = false;
    }
    void CheckDirection(float offsetX, float offsetY, string direction)
    {
        Vector2 objectCurrentPos = GetComponent<Transform>().position;
        Vector2 startCast, endCast;
        startCast = objectCurrentPos;

        startCast.x = objectCurrentPos.x + offsetX;
        startCast.y = objectCurrentPos.y + offsetY;
        endCast = startCast;
        if (offsetY != 0)
        {
            if (offsetY < 0)
            {
                endCast.y -= 0.5f;
            }
            else
            {
                endCast.y += 0.5f;
            }
        }
        else
        {
            if (offsetX < 0)
            {
                endCast.x -= 0.5f;
            }
            else
            {
                endCast.x += 0.5f;
            }
        }
        RaycastHit2D hit = Physics2D.Linecast(startCast, endCast);
        Debug.DrawLine(startCast, endCast, Color.yellow);
        if (hit.collider != null && leverCooldown <= 0)
        {
            if (hit.collider.tag == "Player")
            {
                if (hit.collider.GetComponent<PlayerMovement>().isMoving == false)
                {
                    if (Input.GetKey("d"))
                    {
                        if (direction == "LEFT")
                        {
                            stateOn = !stateOn;
                            wasPulled = true;
                            leverCooldown = GameController.buttonCooldown;
                        }
                    }
                    else if (Input.GetKey("a"))
                    {
                        if (direction == "RIGHT")
                        {
                            stateOn = !stateOn;
                            wasPulled = true;
                            leverCooldown = GameController.buttonCooldown;
                        }
                    }
                    else if (Input.GetKey("w"))
                    {
                        if (direction == "DOWN")
                        {
                            stateOn = !stateOn;
                            wasPulled = true;
                            leverCooldown = GameController.buttonCooldown;
                        }
                    }
                    else if (Input.GetKey("s"))
                    {
                        if (direction == "UP")
                        {
                            stateOn = !stateOn;
                            wasPulled = true;
                            leverCooldown = GameController.buttonCooldown;
                        }
                    }
                    //Debug.Log(hit.collider.GetComponent<PlayerCollision>().canMoveRight);

                }

            }
        }
    }
}
