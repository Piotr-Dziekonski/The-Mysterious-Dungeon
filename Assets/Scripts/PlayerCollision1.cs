using System;
using UnityEngine;
using Utils1;

public class PlayerCollision2 : MonoBehaviour
{

    public bool canMoveRight, canMoveLeft, canMoveUp, canMoveDown;
    GameObject buttonUnderPlayer;
    public bool isExiting = false;
    private PlayerMovement playerMovement;
    private CircleCollider2D playerCollider;
    public GameObject playerOnLeft, playerOnRight, playerOnUp, playerOnDown;

    void Start()
    {
        canMoveRight = true;
        canMoveLeft = true;
        canMoveUp = true;
        canMoveDown = true;
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {

    }

}

