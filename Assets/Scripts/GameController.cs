using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class GameController : MonoBehaviour {

    public static ArrayList allPlayers;
    public static ArrayList allButtons;
    public static ArrayList allGates;
    public static ArrayList allArrows;
    public static ArrayList allBlocks;


    public static float speed = 4.5f;
    public static GameController instance;
    public static bool cooldown = false;
    public static int buttonCooldown = 14;
    public bool levelCompleted = false;
    public static ItemController.ItemType[] inventory = new ItemController.ItemType[10];
    public GameObject completeLevelUI;

    public ItemController.ItemType[] currentInv;

	// Use this for initialization
	void Awake () {
		if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

    allPlayers = new ArrayList(GameObject.FindGameObjectsWithTag("Player"));
    allButtons = new ArrayList(GameObject.FindGameObjectsWithTag("Button"));
    allGates = new ArrayList(GameObject.FindGameObjectsWithTag("Gate"));
    allArrows = new ArrayList(GameObject.FindGameObjectsWithTag("Arrow"));
    allBlocks = new ArrayList(GameObject.FindGameObjectsWithTag("Block"));


}

    // Update is called once per frame
    void Update () {
        
        
        CheckCollisions();
        if (Input.GetKey("w") && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.up);
        }
        else if (Input.GetKey("a") && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.left);
        }
        else if (Input.GetKey("s") && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.down);
        }
        else if (Input.GetKey("d") && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.right);
        }

        MoveObjects();




        currentInv = inventory;

        if (cooldown)
        {
            GameController.cooldown = false;
        }

    }
    public void MoveObjects()
    {
        for (int i = allBlocks.Count - 1; i >= 0; i--)
        {
            GameObject o = (GameObject)allBlocks[i];
            BlockController blockController = o.GetComponent<BlockController>();
            if (blockController.isMoving)
            {
                if (blockController.transform.position == blockController.pos)
                {
                    blockController.isMoving = false;
                }
                else
                {
                    blockController.transform.position = Vector3.MoveTowards(blockController.transform.position, blockController.pos, Time.deltaTime * speed);
                }
            }
            else
            {
                blockController.pos = blockController.transform.position;
            }
        }
        for (int i = allPlayers.Count - 1; i >= 0; i--)
        {
            GameObject o = (GameObject)allPlayers[i];
            PlayerMovement playerMovement = o.GetComponent<PlayerMovement>();
            if (playerMovement.isMoving)
            {
                if (playerMovement.transform.position == playerMovement.pos)
                {
                    playerMovement.isMoving = false;
                }
                else
                {
                    playerMovement.transform.position = Vector3.MoveTowards(playerMovement.transform.position, playerMovement.pos, Time.deltaTime * speed);
                }
            }
            else
            {
                playerMovement.pos = playerMovement.transform.position;
            }
        }
        
    }
    public void CheckUnder(GameObject obj)
    {
        GameObject[] objectsUnder = Tools.GetObjectsUnderSelf(obj);

        if (objectsUnder != null)
        {
            foreach (GameObject objectUnder in objectsUnder)
            {
                if (obj.tag == "Player")
                {
                    PlayerCollision playerCollision = obj.GetComponent<PlayerCollision>();
                    switch (objectUnder.tag)
                    {
                        case "Gate":
                            if (!objectUnder.GetComponent<GateController>().isOpen)
                            {
                                playerCollision.canMoveDown = false;
                                playerCollision.canMoveLeft = false;
                                playerCollision.canMoveRight = false;
                                playerCollision.canMoveUp = false;
                            }
                            break;
                        case "Finish":
                            if (playerCollision.finished == false)
                            {
                                playerCollision.allowFinishSound = true;
                            }
                            playerCollision.finished = true;

                            obj.transform.localScale -= new Vector3(1.1f * Time.fixedDeltaTime, 1.1f * Time.fixedDeltaTime, 0);

                            if (allPlayers.Count == 1 && GameController.instance.levelCompleted == false)
                            {
                                GameController.instance.LevelCompleted();
                            }
                            if (obj.transform.localScale.x <= 0)
                            {
                                Destroy(obj);
                                allPlayers.Remove(obj);
                            }
                            break;
                        case "Button":
                            if (playerCollision.buttonUnderPlayer == null)
                            {
                                playerCollision.buttonUnderPlayer = objectUnder;
                            }
                            ButtonController buttonController = playerCollision.buttonUnderPlayer.GetComponent<ButtonController>();
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
                else if (obj.tag == "Block")
                {
                    BlockController blockController = obj.GetComponent<BlockController>();
                    switch (objectUnder.tag)
                    {
                        case "BlockTarget":
                            if (blockController.targetUnderBlock == null)
                            {
                                
                                blockController.targetUnderBlock = objectUnder;
                            }
                            blockController.targetUnderBlock.GetComponent<TargetController>().standingBlock = obj;
                            break;
                        case "Gate":
                            if (objectUnder.GetComponent<GateController>())
                            {
                                blockController.canMoveDown = false;
                                blockController.canMoveLeft = false;
                                blockController.canMoveRight = false;
                                blockController.canMoveUp = false;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        else
        {
            if (obj.tag == "Player")
            {
                PlayerCollision playerCollision = obj.GetComponent<PlayerCollision>();
                if (playerCollision.buttonUnderPlayer != null)
                {
                    ButtonController buttonController = playerCollision.buttonUnderPlayer.GetComponent<ButtonController>();
                    if (buttonController.resetable == true)
                    {
                        buttonController.allowSound = true;
                        buttonController.pressed = false;
                        buttonController.standingPlayer = null;
                    }
                }
                playerCollision.buttonUnderPlayer = null;
            }
            else if (obj.tag == "Block")
            {
                BlockController blockController = obj.GetComponent<BlockController>();
                if (blockController.targetUnderBlock != null)
                {
                    TargetController targetController = blockController.targetUnderBlock.GetComponent<TargetController>();

                    //targetController.allowSound = true;
                    targetController.pressed = false;
                    targetController.standingBlock = null;

                }
                blockController.targetUnderBlock = null;
            }
        }
        
    }
    public void CheckCollisions()
    {
        for(int i = allPlayers.Count-1; i>= 0; i--)
        {
            GameObject o = (GameObject)allPlayers[i];
            PlayerCollision playerCollision = o.GetComponent<PlayerCollision>();
            
            if (playerCollision.finished)
            {
                playerCollision.canMoveLeft = false;
                playerCollision.canMoveRight = false;
                playerCollision.canMoveUp = false;
                playerCollision.canMoveDown = false;
            }
            else
            {
                Tools.CheckDirection(o.transform, -0.48f, 0f, Vector2.left, 0.5f, DirectionFacing.LEFT, ref playerCollision.canMoveLeft);
                Tools.CheckDirection(o.transform, 0.48f, 0f, Vector2.right, 0.5f, DirectionFacing.RIGHT, ref playerCollision.canMoveRight);
                Tools.CheckDirection(o.transform, 0f, 0.48f, Vector2.up, 0.5f, DirectionFacing.UP, ref playerCollision.canMoveUp);
                Tools.CheckDirection(o.transform, 0f, -0.48f, Vector2.down, 0.5f, DirectionFacing.DOWN, ref playerCollision.canMoveDown);
                
            }
            CheckUnder(o);

        }
        for (int i = allBlocks.Count - 1; i >= 0; i--)
        {
            
            GameObject o = (GameObject)allBlocks[i];
            BlockController blockController = o.GetComponent<BlockController>();
            //CheckUnder(o);
            blockController.playerOnLeft = Tools.CheckDirection(o.transform, -0.48f, 0f, Vector2.left, 0.5f, DirectionFacing.LEFT, ref blockController.canMoveLeft);
            blockController.playerOnRight = Tools.CheckDirection(o.transform, 0.48f, 0f, Vector2.right, 0.5f, DirectionFacing.RIGHT, ref blockController.canMoveRight);
            blockController.playerOnUp = Tools.CheckDirection(o.transform, 0f, 0.48f, Vector2.up, 0.5f, DirectionFacing.UP, ref blockController.canMoveUp);
            blockController.playerOnDown = Tools.CheckDirection(o.transform, 0f, -0.48f, Vector2.down, 0.5f, DirectionFacing.DOWN, ref blockController.canMoveDown);
            CheckUnder(o);
        }
        
    }
    public void SetNewPositions(Vector3 movVec)
    {
        foreach(GameObject o in allPlayers)
        {
            
            PlayerCollision playerCollision = o.GetComponent<PlayerCollision>();
            bool condition = (playerCollision.canMoveUp && movVec == Vector3.up) ||
                            (playerCollision.canMoveDown && movVec == Vector3.down) ||
                            (playerCollision.canMoveLeft && movVec == Vector3.left) ||
                            (playerCollision.canMoveRight && movVec == Vector3.right);
            if(movVec == Vector3.up)
            {
                playerCollision.directionFacing = DirectionFacing.UP;
            }
            if (movVec == Vector3.down)
            {
                playerCollision.directionFacing = DirectionFacing.DOWN;
            }
            if (movVec == Vector3.left)
            {
                playerCollision.directionFacing = DirectionFacing.LEFT;
            }
            if (movVec == Vector3.right)
            {
                playerCollision.directionFacing = DirectionFacing.RIGHT;
            }
            if (condition)
            {
                o.GetComponent<PlayerMovement>().isMoving = true;
                o.GetComponent<PlayerMovement>().pos += movVec;
            }
            
        }
        foreach (GameObject o in allBlocks)
        {
            BlockController blockController = o.GetComponent<BlockController>();
            bool condition = (blockController.canMoveUp && blockController.playerOnDown != null && blockController.playerOnDown.GetComponent<PlayerCollision>().canMoveUp && movVec == Vector3.up) ||
                            (blockController.canMoveDown && blockController.playerOnUp != null && blockController.playerOnUp.GetComponent<PlayerCollision>().canMoveDown && movVec == Vector3.down) ||
                            (blockController.canMoveLeft && blockController.playerOnRight != null && blockController.playerOnRight.GetComponent<PlayerCollision>().canMoveLeft && movVec == Vector3.left) ||
                            (blockController.canMoveRight && blockController.playerOnLeft != null && blockController.playerOnLeft.GetComponent<PlayerCollision>().canMoveRight && movVec == Vector3.right);
            //Debug.Log("Block can move up:"+ blockController.canMoveUp);
            //Debug.Log(blockController.playerOnDown != null);
            
            if (condition)
            {
                blockController.isMoving = true;
                blockController.pos += movVec;
            }
        }
            





        cooldown = false;
        
    }
    public static bool UseItemFromInventory(ItemController.ItemType item)
    {
        ItemController.ItemType[] inventory = GameController.inventory;
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == item)
            {
                inventory[i] = ItemController.ItemType.None;
                return true;
            }
        }
        return false;
    }
    public void LevelCompleted()
    {

        //Debug.Log("LEVEL COMPLETED");
        completeLevelUI.SetActive(true);
        levelCompleted = true;
        
    }
}
