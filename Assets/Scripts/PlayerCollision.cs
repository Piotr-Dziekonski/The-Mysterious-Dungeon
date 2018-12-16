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
    public bool canPushBlocks;
    public DirectionFacing directionFacing = DirectionFacing.RIGHT;
    [Header("Movement data")]
    public Vector3 newPos;
    public Vector3 movementVec;
    public bool isMoving;
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

    private Animator animator;
    private SpriteRenderer sprite;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMoving)
        {
            GameController.cooldown = true;
        }
        animator.SetBool("isMoving", isMoving);

        Tools.SetAnimationFacingBools(directionFacing, animator, sprite);
    }

}

