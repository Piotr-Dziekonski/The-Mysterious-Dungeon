using UnityEngine;

using Utils;

public class PlayerMovement : MonoBehaviour
{

    public bool isMoving, canPushBlocks;
    
    
    [HideInInspector]
    public Vector3 pos, movementVec;
    public Rigidbody2D hitboxRight;
    private float speed = GameController.speed;
    private Animator animator;
    
    [HideInInspector]
    public bool canMoveRight, canMoveLeft, canMoveUp, canMoveDown;
    private void Start()
    {
        animator = GetComponent<Animator>();
        isMoving = false;
        canPushBlocks = true;
    }

    private void Update()
    {
        if (isMoving)
            GameController.cooldown = true;
        animator.SetBool("isMoving", isMoving);


        

        PlayerCollision playerCollision = GetComponent<PlayerCollision>();
        /*canMoveRight = playerCollision.canMoveRight;
        canMoveLeft = playerCollision.canMoveLeft;
        canMoveUp = playerCollision.canMoveUp;
        canMoveDown = playerCollision.canMoveDown;*/

        /*if (buttonCooldown > 0)
        {
            buttonCooldown--;
            canPushBlocks = false;
        }*/
            

        /*if (isMoving)
        {
            if (transform.position == pos)
            {
                canPushBlocks = true;
                isMoving = false;
                //Move();
            }
            else
            {
                canPushBlocks = false;
                transform.position = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * speed);
            }
        }
        else
        {
            canPushBlocks = true;
            pos = transform.position;
            //Move();
        }*/
    }
    private void LateUpdate()
    {
        
    }

    /*void Move()
    {
        if(!GameController.cooldown && !isMoving)
        {
            canPushBlocks = true;
            if (Input.GetKey("d"))
            {
                Tools.CheckDirection(transform, 0.48f, 0f, Vector2.right, 0.5f, DirectionFacing.RIGHT, ref canMoveRight);
                directionFacing = DirectionFacing.RIGHT;
                if (canMoveRight)
                {
                    movementVec = Vector3.right;
                    isMoving = true;
                    pos += movementVec;
                }
            }
            else if (Input.GetKey("a"))
            {
                Tools.CheckDirection(transform, -0.48f, 0f, Vector2.left, 0.5f, DirectionFacing.LEFT, ref canMoveLeft);
                directionFacing = DirectionFacing.LEFT;
                if (canMoveLeft)
                {
                    isMoving = true;
                    pos += Vector3.left;
                }
            }
            else if (Input.GetKey("w"))
            {
                Tools.CheckDirection(transform, 0f, 0.48f, Vector2.up, 0.5f, DirectionFacing.UP, ref canMoveUp);
                directionFacing = DirectionFacing.UP;
                if (canMoveUp)
                {
                    isMoving = true;
                    pos += Vector3.up;
                }
            }
            else if (Input.GetKey("s"))
            {
                Tools.CheckDirection(transform, 0f, -0.48f, Vector2.down, 0.5f, DirectionFacing.DOWN, ref canMoveDown);
                directionFacing = DirectionFacing.DOWN;
                if (canMoveDown)
                {
                    isMoving = true;
                    pos += Vector3.down;
                }
            }
            
        }  
    }*/
}
