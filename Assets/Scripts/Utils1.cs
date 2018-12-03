using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils1{
    public enum DirectionFacing2 { RIGHT, LEFT, UP, DOWN }
    public class Tools : MonoBehaviour
    {

        public static GameObject GetObjectUnderSelf(GameObject thisObject)
        {
            GameObject[] objects = FindObjectsOfType<GameObject>();
            foreach (GameObject o in objects)
            {
                if (thisObject.transform.position == o.transform.position && o != thisObject.gameObject)
                {
                    return o;
                }

            }
            return null;

        }

        public static GameObject CheckDirection(Transform transform, float offsetX, float offsetY, Vector2 direction, float distance, DirectionFacing2 directionToCheck, ref bool boolToChange)
        {
            GameObject playerFound = null;

            Vector2 objectCurrentPos = transform.position;
            Vector2 startCast = objectCurrentPos;
            startCast.x = objectCurrentPos.x + offsetX;
            startCast.y = objectCurrentPos.y + offsetY;

            RaycastHit2D[] hit = Physics2D.RaycastAll(startCast, direction, distance);

            
            if (hit.Length > 0)
            {
                for(var i = 0; i < hit.Length; i++)
                {
                    Debug.DrawLine(startCast, hit[i].point, Color.blue);
                    Debug.Log(hit[i].collider.name);
                    if (hit[i].collider.tag == "Arrow")
                    {
                        

                    }
                    else if (hit[i].collider.tag == "Player")
                    {
                        playerFound = hit[i].collider.gameObject;
                        PlayerCollision playerCollision = hit[i].collider.GetComponent<PlayerCollision>();
                        switch (directionToCheck)
                        {
                            case DirectionFacing2.RIGHT:
                                boolToChange = playerCollision.canMoveRight;
                                break;
                            case DirectionFacing2.LEFT:
                                boolToChange = playerCollision.canMoveLeft;
                                break;
                            case DirectionFacing2.UP:
                                boolToChange = playerCollision.canMoveUp;
                                break;
                            case DirectionFacing2.DOWN:
                                boolToChange = playerCollision.canMoveDown;
                                break;
                        }
                    }
                    else if (hit[i].collider.tag == "Block")
                    {
                        BlockController blockController = hit[i].collider.GetComponent<BlockController>();
                        switch (directionToCheck)
                        {
                            case DirectionFacing2.RIGHT:
                                boolToChange = blockController.canMoveRight;
                                break;
                            case DirectionFacing2.LEFT:
                                boolToChange = blockController.canMoveLeft;
                                break;
                            case DirectionFacing2.UP:
                                boolToChange = blockController.canMoveUp;
                                break;
                            case DirectionFacing2.DOWN:
                                boolToChange = blockController.canMoveDown;
                                break;
                        }
                        if (transform.tag == "Block")
                        {

                        }
                        if (transform.tag == "Player")
                        {
                            BlockController hitBlockController = hit[i].collider.gameObject.GetComponent<BlockController>();
                            if (hitBlockController.isMoving == false)
                            {
                                if (hitBlockController.playerOnDown != null && hitBlockController.playerOnDown.GetComponent<PlayerMovement>().canPushBlocks && hitBlockController.playerOnDown.GetComponent<PlayerMovement>().canMoveUp && !hitBlockController.playerOnDown.GetComponent<PlayerMovement>().isMoving)
                                {
                                    if (Input.GetKey("w") && hitBlockController.canMoveUp)
                                    {
                                        hitBlockController.movementVec = Vector3.up;
                                        hitBlockController.isMoving = true;
                                        hitBlockController.pos += hitBlockController.movementVec;
                                    }
                                }
                                if (hitBlockController.playerOnUp != null && hitBlockController.playerOnUp.GetComponent<PlayerMovement>().canPushBlocks && hitBlockController.playerOnUp.GetComponent<PlayerMovement>().canMoveDown && !hitBlockController.playerOnUp.GetComponent<PlayerMovement>().isMoving)
                                {
                                    if (Input.GetKey("s") && hitBlockController.canMoveDown)
                                    {
                                        hitBlockController.movementVec = Vector3.down;
                                        hitBlockController.isMoving = true;
                                        hitBlockController.pos += hitBlockController.movementVec;
                                    }
                                }
                                if (hitBlockController.playerOnRight != null && hitBlockController.playerOnRight.GetComponent<PlayerMovement>().canPushBlocks && hitBlockController.playerOnRight.GetComponent<PlayerMovement>().canMoveLeft && !hitBlockController.playerOnRight.GetComponent<PlayerMovement>().isMoving)
                                {
                                    if (Input.GetKey("a") && hitBlockController.canMoveLeft)
                                    {
                                        hitBlockController.movementVec = Vector3.left;
                                        hitBlockController.isMoving = true;
                                        hitBlockController.pos += hitBlockController.movementVec;
                                    }
                                }
                                if (hitBlockController.playerOnLeft != null && hitBlockController.playerOnLeft.GetComponent<PlayerMovement>().canPushBlocks && hitBlockController.playerOnLeft.GetComponent<PlayerMovement>().canMoveRight && !hitBlockController.playerOnLeft.GetComponent<PlayerMovement>().isMoving)
                                {
                                    if (Input.GetKey("d") && hitBlockController.canMoveRight)
                                    {
                                        hitBlockController.movementVec = Vector3.right;
                                        hitBlockController.isMoving = true;
                                        hitBlockController.pos += hitBlockController.movementVec;
                                    }
                                }
                                /*
                                    * jezeli wcisne 'w' && klocek.canmoveup
                                    * move();
                                    * 
                                    * 
                                    * 
                                    * */
                                
                                //Debug.Log(hit.collider.GetComponent<PlayerCollision>().canMoveRight);
                            }

                        }


                    }
                    
                    else if (hit[i].collider.tag == "Door"/*|| hit.collider.GetComponent<DoorController>().itemNeeded == ItemController.ItemType.None*/)
                    {
                        Debug.Log(hit[i].collider.tag);
                        DoorController doorController = hit[i].collider.GetComponent<DoorController>();
                        if ((Input.GetKey("d") && directionToCheck == DirectionFacing2.RIGHT || Input.GetKey("a") && directionToCheck == DirectionFacing2.LEFT || Input.GetKey("w") && directionToCheck == DirectionFacing2.UP || Input.GetKey("s") && directionToCheck == DirectionFacing2.DOWN) && GameController.UseItemFromInventory(hit[i].collider.GetComponent<DoorController>().itemNeeded))
                        {
                            doorController.stateOn = (doorController.stateOn == true) ? false : true;
                        }
                        else if (boolToChange == true)
                        {
                            boolToChange = false;
                        }



                    }
                    else if (boolToChange == true)
                    {
                        boolToChange = false;
                    }
                }
                

            }
            else
            {
                switch (directionToCheck)
                {
                    case DirectionFacing2.RIGHT:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing2.LEFT:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing2.UP:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing2.DOWN:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;

                }

            }
            return playerFound;
        }


    }
}

