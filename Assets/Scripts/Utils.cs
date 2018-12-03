using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils{
    public enum DirectionFacing { RIGHT, LEFT, UP, DOWN }
    /*public static class GameObjectExtension
    {
        public static class defaultController { get; set; };
        public static void DoSomething(this GameObject obj)
        {

        }
    }*/
    public class Tools : MonoBehaviour
    {

        public static GameObject[] GetObjectsUnderSelf(GameObject thisObject)
        {
            GameObject[] objects = FindObjectsOfType<GameObject>();
            ArrayList foundObjects = new ArrayList();
            foreach (GameObject o in objects)
            {
                if (thisObject.transform.position == o.transform.position && o != thisObject.gameObject)
                {
                    foundObjects.Add(o);
                    
                }

            }
            if(foundObjects.Count > 0)
            {
                return foundObjects.ToArray(typeof (GameObject)) as GameObject[];
            }
            return null;

        }

        public static GameObject CheckDirection(Transform transform, float offsetX, float offsetY, Vector2 direction, float distance, DirectionFacing directionToCheck, ref bool boolToChange)
        {
            GameObject playerFound = null;

            Vector2 objectCurrentPos = transform.position;
            Vector2 startCast = objectCurrentPos;
            startCast.x = objectCurrentPos.x + offsetX;
            startCast.y = objectCurrentPos.y + offsetY;

            RaycastHit2D[] hit = Physics2D.RaycastAll(startCast, direction, distance);

            
            if (hit.Length > 0)
            {
                bool arrow = false;
               
                for (var i = 0; i < hit.Length; i++)
                {
                    Debug.DrawLine(startCast, hit[i].point, Color.blue);
                    if (hit[i].collider.tag == "Arrow")
                    {
                        ArrowController arrowController = hit[i].collider.GetComponent<ArrowController>();



                        if (directionToCheck == DirectionFacing.RIGHT)
                        {
                            if (arrowController.directionFacing == DirectionFacing.LEFT)
                            {
                                boolToChange = false;
                                arrow = true;
                            }
                            else
                                boolToChange = true;
                        }
                        if (directionToCheck == DirectionFacing.LEFT)
                        {
                            if (arrowController.directionFacing == DirectionFacing.RIGHT)
                            {
                                boolToChange = false;
                                arrow = true;
                            }
                            else
                                boolToChange = true;
                        }
                        if (directionToCheck == DirectionFacing.UP)
                        {
                            if (arrowController.directionFacing == DirectionFacing.DOWN)
                            {
                                boolToChange = false;
                                arrow = true;
                            }
                            else
                                boolToChange = true;
                        }
                        if (directionToCheck == DirectionFacing.DOWN)
                        {
                            if (arrowController.directionFacing == DirectionFacing.UP)
                            {
                                boolToChange = false;
                                arrow = true;
                            }
                            else
                                boolToChange = true;
                        }
                    }
                    else if (hit[i].collider.tag == "Player")
                    {
                        playerFound = hit[i].collider.gameObject;
                        if (arrow == false)
                        { 
                            PlayerCollision playerCollision = hit[i].collider.GetComponent<PlayerCollision>();
                            switch (directionToCheck)
                            {
                                case DirectionFacing.RIGHT:
                                    boolToChange = playerCollision.canMoveRight;
                                    break;
                                case DirectionFacing.LEFT:
                                    boolToChange = playerCollision.canMoveLeft;
                                    break;
                                case DirectionFacing.UP:
                                    boolToChange = playerCollision.canMoveUp;
                                    break;
                                case DirectionFacing.DOWN:
                                    boolToChange = playerCollision.canMoveDown;
                                    break;
                            }
                        }
                    }
                    else if (hit[i].collider.tag == "Block" && arrow == false)
                    {
                        BlockController blockController = hit[i].collider.GetComponent<BlockController>();
                        switch (directionToCheck)
                        {
                            case DirectionFacing.RIGHT:
                                boolToChange = blockController.canMoveRight;
                                break;
                            case DirectionFacing.LEFT:
                                boolToChange = blockController.canMoveLeft;
                                break;
                            case DirectionFacing.UP:
                                boolToChange = blockController.canMoveUp;
                                break;
                            case DirectionFacing.DOWN:
                                boolToChange = blockController.canMoveDown;
                                break;
                        }
                        if (transform.tag == "Block")
                        {
                            boolToChange = false;
                        }

                    }
                    else if (hit[i].collider.tag == "Door" && transform.gameObject.tag == "Player" /*|| hit.collider.GetComponent<DoorController>().itemNeeded == ItemController.ItemType.None*/)
                    {
                        //Debug.Log(hit[i].collider.tag);
                        DoorController doorController = hit[i].collider.GetComponent<DoorController>();
                        if ((Input.GetKey("d") && directionToCheck == DirectionFacing.RIGHT || Input.GetKey("a") && directionToCheck == DirectionFacing.LEFT || Input.GetKey("w") && directionToCheck == DirectionFacing.UP || Input.GetKey("s") && directionToCheck == DirectionFacing.DOWN) && GameController.UseItemFromInventory(hit[i].collider.GetComponent<DoorController>().itemNeeded))
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
                    case DirectionFacing.RIGHT:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing.LEFT:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing.UP:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;
                    case DirectionFacing.DOWN:
                        if (boolToChange == false)
                            boolToChange = true;
                        break;

                }

            }
            return playerFound;
        }


    }
}

