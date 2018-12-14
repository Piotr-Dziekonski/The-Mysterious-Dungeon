using System;
using UnityEngine;
using Utils;
using System.Linq;

public class BlockController : MonoBehaviour
{
    [Header("Mechanics data")]
    public GameObject targetUnderBlock;
    [Header("Movement data")]
    public bool canMoveRight;
    public bool canMoveLeft;
    public bool canMoveUp;
    public bool canMoveDown;
    public bool isMoving;
    public Vector3 newPos, movementVec;
    [Header("Detected collisions")]
    public GameObject playerOnLeft;
    public GameObject playerOnRight;
    public GameObject playerOnUp;
    public GameObject playerOnDown;
    public GameObject entityOnTopRight;
    public GameObject entityOnTopLeft;
    public GameObject entityOnBottomRight;
    public GameObject entityOnBottomLeft;
    public GameObject entity2BlocksAwayOnRight;
    public GameObject entity2BlocksAwayOnLeft;
    public GameObject entity2BlocksAwayOnUp;
    public GameObject entity2BlocksAwayOnDown;

    void Start()
    {
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

