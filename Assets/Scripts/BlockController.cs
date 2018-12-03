using System;
using UnityEngine;
using Utils;
using System.Linq;

public class BlockController : MonoBehaviour
{

    public Rigidbody2D rb;
    public bool canMoveRight, canMoveLeft, canMoveUp, canMoveDown;
    public GameObject targetUnderBlock;
    public bool isMoving;
    public bool isBeingPushed;
    public Vector3 pos, movementVec;
    public bool isAround;
    public GameObject playerOnLeft, playerOnRight, playerOnUp, playerOnDown;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        canMoveRight = true;
        canMoveLeft = true;
        canMoveUp = true;
        canMoveDown = true;
    }

    void Update()
    {
       
        /*else if (CheckIfUnderSelf("Button") != null)
        {
            if (targetUnderBlock == null)
            {
                targetUnderBlock = CheckIfUnderSelf("Button");
            }
            targetUnderBlock.GetComponent<ButtonController>().pressed = true;
            targetUnderBlock.GetComponent<ButtonController>().standingPlayer = gameObject;
        }*/



        /*if (CheckIfUnderSelf("Button") == null)
        {
            if (targetUnderBlock != null && targetUnderBlock.GetComponent<ButtonController>().resetable == true)
            {
                targetUnderBlock.GetComponent<ButtonController>().pressed = false;
                targetUnderBlock.GetComponent<ButtonController>().standingPlayer = null;
            }
            targetUnderBlock = null;
        }*/

    }
}

