using UnityEngine;

using Utils;

public class PlayerMovement : MonoBehaviour
{

    public bool isMoving, canPushBlocks;
    
    
    [HideInInspector]
    public Vector3 newPos, movementVec;
    private Animator animator;
    
    [HideInInspector]
    public bool canMoveRight, canMoveLeft, canMoveUp, canMoveDown;
    private void Start()
    {
        animator = GetComponent<Animator>();
        //isMoving = false;
        //canPushBlocks = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            GameController.cooldown = true;
        }
            animator.SetBool("isMoving", isMoving);
        
    }
}
