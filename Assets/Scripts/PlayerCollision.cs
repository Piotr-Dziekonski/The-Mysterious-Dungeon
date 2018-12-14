using System;
using UnityEngine;
using Utils;

public class PlayerCollision : MonoBehaviour
{
    [Header("Mechanics data")]
    public bool allowFinishSound;
    public GameObject buttonUnderPlayer;
    public bool isExiting = false;
    public bool finished = false;
    public DirectionFacing directionFacing = DirectionFacing.RIGHT;
    [Header("Movement data")]
    public bool canMoveRight;
    public bool canMoveLeft;
    public bool canMoveUp;
    public bool canMoveDown;
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

    [HideInInspector]
    public PlayerMovement playerMovement;
    private Animator animator;

    void Awake()
    {
        canMoveRight = true;
        canMoveLeft = true;
        canMoveUp = true;
        canMoveDown = true;
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (directionFacing == DirectionFacing.LEFT)
        {
            animator.SetBool("looksRight", false);
            animator.SetBool("looksDown", false);
            animator.SetBool("looksLeft", true);
            animator.SetBool("looksUp", false);
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (directionFacing == DirectionFacing.RIGHT)
        {
            animator.SetBool("looksRight", true);
            animator.SetBool("looksDown", false);
            animator.SetBool("looksLeft", false);
            animator.SetBool("looksUp", false);
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (directionFacing == DirectionFacing.UP)
        {
            animator.SetBool("looksRight", false);
            animator.SetBool("looksDown", false);
            animator.SetBool("looksLeft", false);
            animator.SetBool("looksUp", true);
        }
        else if (directionFacing == DirectionFacing.DOWN)
        {
            animator.SetBool("looksRight", false);
            animator.SetBool("looksDown", true);
            animator.SetBool("looksLeft", false);
            animator.SetBool("looksUp", false);
        }
        //GameObject objectUnder = Tools.GetObjectUnderSelf(gameObject);
        /*playerOnLeft = Tools.CheckDirection(transform, -0.48f, 0f, Vector2.left, 0.5f, DirectionFacing.LEFT, ref canMoveLeft);
        playerOnRight = Tools.CheckDirection(transform, 0.48f, 0f, Vector2.right, 0.5f, DirectionFacing.RIGHT, ref canMoveRight);
        playerOnUp = Tools.CheckDirection(transform, 0f, 0.48f, Vector2.up, 0.5f, DirectionFacing.UP, ref canMoveUp);
        playerOnDown = Tools.CheckDirection(transform, 0f, -0.48f, Vector2.down, 0.5f, DirectionFacing.DOWN, ref canMoveDown);*/
        /*if (objectUnder != null)
        {
            switch (objectUnder.tag)
            {
                case "Gate":
                    canMoveDown = false;
                    canMoveLeft = false;
                    canMoveRight = false;
                    canMoveUp = false;
                    break;
                case "Finish":
                    if (playerCollider.enabled)
                    {
                        isExiting = true;
                    }
                    else
                    {
                        isExiting = false;
                    }
                    playerMovement.enabled = false;
                    transform.localScale -= new Vector3(1.1f * Time.fixedDeltaTime, 1.1f * Time.fixedDeltaTime, 0);

                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    if (players.Length == 1 && GameController.instance.levelCompleted == false)
                    {
                        GameController.instance.LevelCompleted();
                    }
                    if (transform.localScale.x <= 0)
                    {
                        Destroy(gameObject);
                    }
                    break;
                case "Button":
                    if (buttonUnderPlayer == null)
                    {
                        buttonUnderPlayer = objectUnder;
                    }
                    ButtonController buttonController = buttonUnderPlayer.GetComponent<ButtonController>();
                    if (buttonController.pressed == false)
                    {
                        buttonController.allowSound = true;
                    }
                    buttonController.pressed = true;
                    buttonController.standingPlayer = gameObject;
                    break;
                case "Item":
                    ItemController itemController = objectUnder.GetComponent<ItemController>();
                    for (int i = 0; i < GameController.inventory.Length; i++)
                    {
                        if (GameController.inventory[i] == ItemController.ItemType.None && objectUnder.GetComponent<SpriteRenderer>().enabled)
                        {
                            GameController.inventory[i] = itemController.itemType;
                            itemController.pickedUp = true;
                            break;
                        }
                    }
                    break;
                
                default:
                    break;
            }
        }
        else
        {
            
            if (buttonUnderPlayer != null)
            {
                ButtonController buttonController = buttonUnderPlayer.GetComponent<ButtonController>();
                if (buttonController.resetable == true)
                {
                    buttonController.allowSound = true;
                    buttonController.pressed = false;
                    buttonController.standingPlayer = null;
                }  
            }
            buttonUnderPlayer = null;
        }*/
    }

}

