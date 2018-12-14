using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class EyeballController : MonoBehaviour {

    [Header("Mechanics data")]
    public bool playerInRange;
    public DirectionFacing directionFacing;
    public DirectionFacing directionToTurnTo;
    [Header("Movement data")]
    public float maxTurnTimeWhenPlayerInRange;
    public float minTurnTimeWhenPlayerInRange;
    public float movementMinDelay;
    public float movementMaxDelay;
    public Vector3 newPos;
    public bool isMoving;
    public bool canMoveLeft;
    public bool canMoveRight;
    public bool canMoveDown;
    public bool canMoveUp;
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
    
    
    private Animator animator;
    private SpriteRenderer sprite;

    [HideInInspector]
    public bool turningCRRunning, moveCRRunning;
    



    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        turningCRRunning = false;
        moveCRRunning = false;
    }
	
	// Update is called once per frame
	void Update () {

        animator.SetBool("isMoving", isMoving);

        Tools.SetAnimationFacingBools(directionFacing, animator, sprite);

        playerInRange = playerOnLeft || playerOnRight || playerOnUp || playerOnDown;
        float time = Random.Range(minTurnTimeWhenPlayerInRange, maxTurnTimeWhenPlayerInRange);
        IEnumerator coroutine = Turn(DirectionFacing.RIGHT, 0);


        if (playerInRange && !turningCRRunning)
        {
            
            if (playerOnLeft != null)
            {
                PlayerCollision playerCollision = playerOnLeft.GetComponent<PlayerCollision>();
                if (directionFacing == DirectionFacing.LEFT && playerCollision.directionFacing == DirectionFacing.RIGHT)
                {
                    Debug.Log("REKT");
                }
                else if(directionFacing != DirectionFacing.LEFT)
                {
                    directionToTurnTo = DirectionFacing.LEFT;
                    //StopCoroutine(coroutine);
                    coroutine = Turn(DirectionFacing.LEFT, time);
                }
                
                
            }
            else if (playerOnRight != null)
            {
                PlayerCollision playerCollision = playerOnRight.GetComponent<PlayerCollision>();
                if (directionFacing == DirectionFacing.RIGHT && playerCollision.directionFacing == DirectionFacing.LEFT)
                {
                    Debug.Log("REKT");
                }
                else if (directionFacing != DirectionFacing.RIGHT)
                {
                    directionToTurnTo = DirectionFacing.RIGHT;
                    //StopCoroutine(coroutine);
                    coroutine = Turn(DirectionFacing.RIGHT, time);
                }

            }
            else if (playerOnUp != null)
            {
                PlayerCollision playerCollision = playerOnUp.GetComponent<PlayerCollision>();
                if (directionFacing == DirectionFacing.UP && playerCollision.directionFacing == DirectionFacing.DOWN)
                {
                    Debug.Log("REKT");
                }
                else if (directionFacing != DirectionFacing.UP)
                {
                    directionToTurnTo = DirectionFacing.UP;
                    //StopCoroutine(coroutine);
                    coroutine = Turn(DirectionFacing.UP, time);
                }

            }
            else if (playerOnDown != null)
            {
                PlayerCollision playerCollision = playerOnDown.GetComponent<PlayerCollision>();
                if (directionFacing == DirectionFacing.DOWN && playerCollision.directionFacing == DirectionFacing.UP)
                {
                    Debug.Log("REKT");
                }
                else if (directionFacing != DirectionFacing.DOWN)
                {
                    directionToTurnTo = DirectionFacing.DOWN;
                    //StopCoroutine(coroutine);
                    coroutine = Turn(DirectionFacing.DOWN, time);
                }

            }

            if (coroutine != null)
            {
                StartCoroutine(coroutine);
            }
        }
        
        
        
        
        
	}

    private IEnumerator Turn(DirectionFacing direction, float time)
    {
        turningCRRunning = true;
        yield return new WaitForSeconds(time);
        if (direction == directionToTurnTo)
        {
            directionFacing = directionToTurnTo;
        }
        turningCRRunning = false;
            
        

    }
}
