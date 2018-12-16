using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Reflection;
using UnityEngine;

namespace Utils{
    public enum DirectionFacing { RIGHT, LEFT, UP, DOWN, NONE }


    public class Tools : MonoBehaviour
    {
        public static Vector3 GetDiagonalEntityNewPos(GameObject diagonalEntity)
        {
            Vector3 entityNewPos = new Vector3();
            if (diagonalEntity != null)
            {
                if (diagonalEntity.tag == "Player")
                    entityNewPos = diagonalEntity.GetComponent<PlayerCollision>().newPos;
                if (diagonalEntity.tag == "Eyeball")
                    entityNewPos = diagonalEntity.GetComponent<EyeballController>().newPos;
                if (diagonalEntity.tag == "Block")
                    entityNewPos = diagonalEntity.GetComponent<BlockController>().newPos;
            }
           
            return entityNewPos;
        }
        public static void UpdateCollisionsData<T>(GameObject o)
        {
            T controller = o.GetComponent<T>();
            List<FieldInfo> fields = new List<FieldInfo>(controller.GetType().GetFields());

            FieldInfo canMoveLeft = fields.Find(x => x.Name == "canMoveLeft");
            FieldInfo canMoveRight = fields.Find(x => x.Name == "canMoveRight");
            FieldInfo canMoveUp = fields.Find(x => x.Name == "canMoveUp");
            FieldInfo canMoveDown = fields.Find(x => x.Name == "canMoveDown");
            FieldInfo entityOnTopRight = fields.Find(x => x.Name == "entityOnTopRight");
            FieldInfo entityOnTopLeft = fields.Find(x => x.Name == "entityOnTopLeft");
            FieldInfo entityOnBottomLeft = fields.Find(x => x.Name == "entityOnBottomLeft");
            FieldInfo entityOnBottomRight = fields.Find(x => x.Name == "entityOnBottomRight");
            FieldInfo entity2BlocksAwayOnRight = fields.Find(x => x.Name == "entity2BlocksAwayOnRight");
            FieldInfo entity2BlocksAwayOnLeft = fields.Find(x => x.Name == "entity2BlocksAwayOnLeft");
            FieldInfo entity2BlocksAwayOnUp = fields.Find(x => x.Name == "entity2BlocksAwayOnUp");
            FieldInfo entity2BlocksAwayOnDown = fields.Find(x => x.Name == "entity2BlocksAwayOnDown");
            FieldInfo finished = fields.Find(x => x.Name == "finished");
            FieldInfo range = fields.Find(x => x.Name == "range");
            FieldInfo playerOnLeft = fields.Find(x => x.Name == "playerOnLeft");
            FieldInfo playerOnRight = fields.Find(x => x.Name == "playerOnRight");
            FieldInfo playerOnUp = fields.Find(x => x.Name == "playerOnUp");
            FieldInfo playerOnDown = fields.Find(x => x.Name == "playerOnDown");

            if (finished != null && (bool)finished.GetValue(controller) == true)
            {
                canMoveLeft.SetValue(controller, false);
                canMoveRight.SetValue(controller, false);
                canMoveUp.SetValue(controller, false);
                canMoveDown.SetValue(controller, false);
            }
            else
            {
                bool left = (bool)canMoveLeft.GetValue(controller);
                bool right = (bool)canMoveRight.GetValue(controller);
                bool up = (bool)canMoveUp.GetValue(controller);
                bool down = (bool)canMoveDown.GetValue(controller);
                float raycastRange = 0.5f;

                bool nothing = false;


                playerOnLeft.SetValue(controller, CheckDirection(o.transform, -0.48f, 0f, Vector2.left, 0.5f, DirectionFacing.LEFT, ref left));
                playerOnRight.SetValue(controller, CheckDirection(o.transform, 0.48f, 0f, Vector2.right, 0.5f, DirectionFacing.RIGHT, ref right));
                playerOnUp.SetValue(controller, CheckDirection(o.transform, 0f, 0.48f, Vector2.up, 0.5f, DirectionFacing.UP, ref up));
                playerOnDown.SetValue(controller, CheckDirection(o.transform, 0f, -0.48f, Vector2.down, 0.5f, DirectionFacing.DOWN, ref down));

                if (range != null && range.GetValue(controller) != null)
                {
                    raycastRange = (float)range.GetValue(controller);
                    playerOnLeft.SetValue(controller, CheckDirection(o.transform, -0.48f, 0f, Vector2.left, raycastRange, DirectionFacing.LEFT, ref nothing));
                    playerOnRight.SetValue(controller, CheckDirection(o.transform, 0.48f, 0f, Vector2.right, raycastRange, DirectionFacing.RIGHT, ref nothing));
                    playerOnUp.SetValue(controller, CheckDirection(o.transform, 0f, 0.48f, Vector2.up, raycastRange, DirectionFacing.UP, ref nothing));
                    playerOnDown.SetValue(controller, CheckDirection(o.transform, 0f, -0.48f, Vector2.down, raycastRange, DirectionFacing.DOWN, ref nothing));
                }

                canMoveLeft.SetValue(controller, left);
                canMoveRight.SetValue(controller, right);
                canMoveUp.SetValue(controller, up);
                canMoveDown.SetValue(controller, down);

                entityOnTopRight.SetValue(controller, CheckDirection(o.transform, 0.48f, 0.48f, new Vector2(1, 1), 0.5f, DirectionFacing.NONE, ref nothing));
                entityOnTopLeft.SetValue(controller, CheckDirection(o.transform, -0.48f, 0.48f, new Vector2(-1, 1), 0.5f, DirectionFacing.NONE, ref nothing));
                entityOnBottomLeft.SetValue(controller, CheckDirection(o.transform, -0.48f, -0.48f, new Vector2(-1, -1), 0.5f, DirectionFacing.NONE, ref nothing));
                entityOnBottomRight.SetValue(controller, CheckDirection(o.transform, 0.48f, -0.48f, new Vector2(1, -1), 0.5f, DirectionFacing.NONE, ref nothing));
                entity2BlocksAwayOnRight.SetValue(controller, CheckDirection(o.transform, 1.48f, 0f, Vector2.right, 1.5f, DirectionFacing.NONE, ref nothing));
                entity2BlocksAwayOnLeft.SetValue(controller, CheckDirection(o.transform, -1.48f, 0f, Vector2.left, 1.5f, DirectionFacing.NONE, ref nothing));
                entity2BlocksAwayOnUp.SetValue(controller, CheckDirection(o.transform, 0f, 1.48f, Vector2.up, 1.5f, DirectionFacing.NONE, ref nothing));
                entity2BlocksAwayOnDown.SetValue(controller, CheckDirection(o.transform, 0f, -1.48f, Vector2.down, 1.5f, DirectionFacing.NONE, ref nothing));
            }
        }
        public static Vector3 VectorToMoveEyeball(EyeballController controller)
        {


            var topRightEntity = controller.entityOnTopRight;
            var topLeftEntity = controller.entityOnTopLeft;
            var bottomRightEntity = controller.entityOnBottomRight;
            var bottomLeftEntity = controller.entityOnBottomLeft;
            var entity2BlocksAwayOnRight = controller.entity2BlocksAwayOnRight;
            var entity2BlocksAwayOnLeft = controller.entity2BlocksAwayOnLeft;
            var entity2BlocksAwayOnUp = controller.entity2BlocksAwayOnUp;
            var entity2BlocksAwayOnDown = controller.entity2BlocksAwayOnDown;
            var entityNewPos = new Vector3(0,0,0);

            GameObject[] diagonalEntities = { topRightEntity, topLeftEntity, bottomRightEntity, bottomLeftEntity, entity2BlocksAwayOnRight, entity2BlocksAwayOnLeft, entity2BlocksAwayOnUp, entity2BlocksAwayOnDown };

            foreach(GameObject diagonalEntity in diagonalEntities)
            {
                entityNewPos = GetDiagonalEntityNewPos(diagonalEntity);
                controller.canMoveRight = controller.canMoveRight && entityNewPos != controller.transform.localPosition + Vector3.right;
                controller.canMoveLeft = controller.canMoveLeft && entityNewPos != controller.transform.localPosition + Vector3.left;
                controller.canMoveUp = controller.canMoveUp && entityNewPos != controller.transform.localPosition + Vector3.up;
                controller.canMoveDown = controller.canMoveDown && entityNewPos != controller.transform.localPosition + Vector3.down;
            }
            
            
            
            //Debug.Log(controller.name + entityNewPos);
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
            else if(rnd < 70 && controller.canMoveLeft)
            {
                return Vector3.left;
            }
            else if(rnd < 80 && controller.canMoveRight)
            {
                return Vector3.right;
            }
            else if(rnd < 90 && controller.canMoveUp)
            {
                return Vector3.up;
            }
            else if(controller.canMoveDown)
            {
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
                            return Vector3.left;
                        }
                        break;
                    case 1:
                        if (controller.canMoveRight)
                        {
                            return Vector3.right;
                        }
                        break;
                    case 2:
                        if (controller.canMoveUp)
                        {
                            return Vector3.up;
                        }
                        break;
                    case 3:
                        if (controller.canMoveDown)
                        {
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
                    else if (hit[i].collider.tag == "Player" && directionToCheck != DirectionFacing.NONE)
                    {
                        if (transform.tag == "Eyeball")
                        {
                            bool interrupted = false;
                            if (i == 0 )
                            {
                                playerFound = hit[i].collider.gameObject;
                                boolToChange = false;
                            }
                            else if (i > 0)
                            {
                                for(int j = 0; j < i; j++)
                                {
                                    if(hit[j].collider.tag == "Wall" || hit[j].collider.tag == "Door" || hit[j].collider.tag == "Block" || hit[j].collider.tag == "Counter")
                                    {
                                        interrupted = true;
                                        break;
                                    }
                                    
                                }
                                if (!interrupted)
                                {
                                    playerFound = hit[i].collider.gameObject;
                                }
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
                    else if (hit[i].collider.tag == "Block" && arrow == false && directionToCheck != DirectionFacing.NONE)
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
                    else if ((hit[i].collider.tag == "Eyeball" || hit[i].collider.tag == "Player" || hit[i].collider.tag == "Block") && directionToCheck == DirectionFacing.NONE)
                    {
                        if(transform.gameObject.tag == "Player" || transform.gameObject.tag == "Eyeball" || transform.gameObject.tag == "Block")
                        {
                            return hit[i].collider.gameObject;
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

