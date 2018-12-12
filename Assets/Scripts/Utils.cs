using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
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

        public static Vector3 VectorToMoveEyeball(EyeballController controller)
        {
            var topRightEntity = controller.entityOnTopRight;
            var topLeftEntity = controller.entityOnTopLeft;
            var bottomRightEntity = controller.entityOnBottomRight;
            var bottomLeftEntity = controller.entityOnBottomLeft;
            var entityNewPos = new Vector3();

            if (topRightEntity != null)
            {
                if(topRightEntity.tag == "Player")
                    entityNewPos = topRightEntity.GetComponent<PlayerMovement>().newPos;
                if (topRightEntity.tag == "Eyeball")
                    entityNewPos = topRightEntity.GetComponent<EyeballController>().newPos;
            }
            if (topLeftEntity != null)
            {
                if (topLeftEntity.tag == "Player")
                    entityNewPos = topLeftEntity.GetComponent<PlayerMovement>().newPos;
                if (topLeftEntity.tag == "Eyeball")
                    entityNewPos = topLeftEntity.GetComponent<EyeballController>().newPos;
            }
            if (bottomRightEntity != null)
            {
                if (bottomRightEntity.tag == "Player")
                    entityNewPos = bottomRightEntity.GetComponent<PlayerMovement>().newPos;
                if (bottomRightEntity.tag == "Eyeball")
                    entityNewPos = bottomRightEntity.GetComponent<EyeballController>().newPos;
            }
            if (bottomLeftEntity != null)
            {
                if (bottomLeftEntity.tag == "Player")
                    entityNewPos = bottomLeftEntity.GetComponent<PlayerMovement>().newPos;
                if (bottomLeftEntity.tag == "Eyeball")
                    entityNewPos = bottomLeftEntity.GetComponent<EyeballController>().newPos;
            }


            controller.canMoveRight = controller.canMoveRight && entityNewPos != controller.transform.localPosition + Vector3.right;
            controller.canMoveLeft = controller.canMoveLeft && entityNewPos != controller.transform.localPosition + Vector3.left;
            controller.canMoveUp = controller.canMoveUp && entityNewPos != controller.transform.localPosition + Vector3.up;
            controller.canMoveDown = controller.canMoveDown && entityNewPos != controller.transform.localPosition + Vector3.down;
            Debug.Log(controller.name + entityNewPos);
            int rnd = UnityEngine.Random.Range(0, 100);
            Vector3 movDir = new Vector3();
            bool canMoveToPickedDir = false;

            switch (controller.directionFacing)
            {
                case DirectionFacing.RIGHT:
                    if (controller.canMoveRight)
                    {
                        canMoveToPickedDir = true;
                        movDir = Vector3.right;
                    }
                        
                    break;
                case DirectionFacing.LEFT:
                    if (controller.canMoveLeft)
                    {
                        canMoveToPickedDir = true;
                        movDir = Vector3.left;
                    }
                    break;
                case DirectionFacing.UP:
                    if (controller.canMoveUp)
                    {
                        canMoveToPickedDir = true;
                        movDir = Vector3.up;
                    }
                    break;
                case DirectionFacing.DOWN:
                    if (controller.canMoveDown)
                    {
                        canMoveToPickedDir = true;
                        movDir = Vector3.down;
                    }
                    break;
                default:
                    break;
            }

            if(rnd < 60 && canMoveToPickedDir)
            { 
                return movDir;
            }
            else if(rnd < 70 && controller.canMoveLeft && controller.prevDirection != DirectionFacing.LEFT)
            {
                controller.directionFacing = DirectionFacing.LEFT;
                return Vector3.left;
            }
            else if(rnd < 80 && controller.canMoveRight && controller.prevDirection != DirectionFacing.RIGHT)
            {
                controller.directionFacing = DirectionFacing.RIGHT;
                return Vector3.right;
            }
            else if(rnd < 90 && controller.canMoveUp && controller.prevDirection != DirectionFacing.UP)
            {
                controller.directionFacing = DirectionFacing.UP;
                return Vector3.up;
            }
            else if(controller.canMoveDown && controller.prevDirection != DirectionFacing.DOWN)
            {
                controller.directionFacing = DirectionFacing.DOWN;
                return Vector3.down;
            }
            else
            {
                int rndDir = UnityEngine.Random.Range(0, 3);
                switch (rndDir)
                {
                    case 0:
                        if (controller.canMoveLeft)
                        {
                            controller.directionFacing = DirectionFacing.LEFT;
                            return Vector3.left;
                        }
                        break;
                    case 1:
                        if (controller.canMoveRight)
                        {
                            controller.directionFacing = DirectionFacing.RIGHT;
                            return Vector3.right;
                        }
                        break;
                    case 2:
                        if (controller.canMoveUp)
                        {
                            controller.directionFacing = DirectionFacing.UP;
                            return Vector3.up;
                        }
                        break;
                    case 3:
                        if (controller.canMoveDown)
                        {
                            controller.directionFacing = DirectionFacing.DOWN;
                            return Vector3.down;
                        }   
                        break;
                    default:
                        break;
                }
            }
            return movDir;
        }

        public static void SetAnimationFacingBools(DirectionFacing objectDirectionFacing, Animator animator, SpriteRenderer sprite)
        {
            if (objectDirectionFacing == DirectionFacing.LEFT)
            {
                animator.SetBool("looksRight", false);
                animator.SetBool("looksDown", false);
                animator.SetBool("looksLeft", true);
                animator.SetBool("looksUp", false);
                sprite.flipX = true;
            }
            else if (objectDirectionFacing == DirectionFacing.RIGHT)
            {
                animator.SetBool("looksRight", true);
                animator.SetBool("looksDown", false);
                animator.SetBool("looksLeft", false);
                animator.SetBool("looksUp", false);
                sprite.flipX = false;
            }
            else if (objectDirectionFacing == DirectionFacing.UP)
            {
                animator.SetBool("looksRight", false);
                animator.SetBool("looksDown", false);
                animator.SetBool("looksLeft", false);
                animator.SetBool("looksUp", true);
            }
            else if (objectDirectionFacing == DirectionFacing.DOWN)
            {
                animator.SetBool("looksRight", false);
                animator.SetBool("looksDown", true);
                animator.SetBool("looksLeft", false);
                animator.SetBool("looksUp", false);
            }
        }
        public static float[] Vector3ToFloatArray(Vector3 v)
        {
            float x, y, z;
            x = v.x;
            y = v.y;
            z = v.z;
            return new float[] { x, y, z };
        }
        public static Vector3 FloatArrayToVector3(float[] arr)
        {
            float x, y, z;
            x = arr[0];
            y = arr[1];
            z = arr[2];
            return new Vector3( x, y, z );
        }
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
            if(transform.tag == "Eyeball")
            
            if (hit.Length > 0)
            {
                
                Debug.DrawLine(startCast, hit[0].point, Color.blue);
                bool arrow = false;
               
                for (var i = 0; i < hit.Length; i++)
                {
                    //Debug.DrawLine(startCast, hit[i].point, Color.blue);
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
                        if (transform.tag == "Eyeball" )
                        {
                            if (i == 0)
                            {
                                playerFound = hit[0].collider.gameObject;
                                boolToChange = false;
                            }

                            
                        }
                        else if(transform.tag != "Eyeball")
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
                        
                        
                    }
                    else if (hit[i].collider.tag == "Block" && arrow == false)
                    {
                        if(transform.tag == "Player")
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
                        }
                        else
                        {
                            boolToChange = false;
                        }

                    }
                    else if (hit[i].collider.tag == "Door" && transform.gameObject.tag == "Player" /*|| hit.collider.GetComponent<DoorController>().itemNeeded == ItemController.ItemType.None*/)
                    {
                        //Debug.Log(hit[i].collider.tag);
                        DoorController doorController = hit[i].collider.GetComponent<DoorController>();
                        if ((Input.GetAxisRaw("Horizontal") > 0 && directionToCheck == DirectionFacing.RIGHT || Input.GetAxisRaw("Horizontal") < 0 && directionToCheck == DirectionFacing.LEFT || Input.GetAxisRaw("Vertical") > 0 && directionToCheck == DirectionFacing.UP || Input.GetAxisRaw("Vertical") < 0 && directionToCheck == DirectionFacing.DOWN) && GameController.UseItemFromInventory(hit[i].collider.GetComponent<DoorController>().itemNeeded))
                        {
                            doorController.stateOn = (doorController.stateOn == true) ? false : true;
                        }
                        else if (boolToChange == true)
                        {
                            boolToChange = false;
                        }



                    }
                    else if (hit[i].collider.tag == "Eyeball" && transform.gameObject.tag == "Eyeball" && directionToCheck == new DirectionFacing())
                    {
                        return hit[i].collider.gameObject;
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

