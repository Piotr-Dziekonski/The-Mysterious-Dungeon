using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

[System.Serializable]
public class GameController : MonoBehaviour {

    public static List<GameObject> allPlayers;
    public static List<GameObject> allButtons;
    //public static ArrayList allGates = new ArrayList();
    public static List<GameObject> allArrows;
    public static List<GameObject> allBlocks;
    public static List<GameObject> allLevers;
    public static List<GameObject> allCounters;
    public static List<GameObject> allDoors;
    public static List<GameObject> allItems;
    public static List<GameObject> allEyeballs;

    public static bool[] completedLevels = new bool[11]; // 10 levels + boss level
    public static int activeLevelNumber;

    public static float speed = 4.5f;
    public static GameController instance;
    public static bool cooldown = false;
    public static int buttonCooldown = 14;
    public bool levelCompleted = false;
    public static ItemController.ItemType[] inventory = new ItemController.ItemType[10];
    public GameObject completeLevelUI;


    private static SaveData dataToLoad;

	void Awake () {
		if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        allPlayers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        allButtons = new List<GameObject>(GameObject.FindGameObjectsWithTag("Button"));
        //allGates = new ArrayList(GameObject.FindGameObjectsWithTag("Gate"));
        allArrows = new List<GameObject>(GameObject.FindGameObjectsWithTag("Arrow"));
        allBlocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("Block"));
        allLevers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Lever"));
        allCounters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Counter"));
        allDoors = new List<GameObject>(GameObject.FindGameObjectsWithTag("Door"));
        allItems = new List<GameObject>(GameObject.FindGameObjectsWithTag("Item"));
        allEyeballs = new List<GameObject>(GameObject.FindGameObjectsWithTag("Eyeball"));
        //completedLevels = new bool[11];

    }

    // Update is called once per frame
    void Update () {
        CheckCollisions();
        if (Input.GetAxisRaw("Vertical") > 0 && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.up);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0 && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.left);
        }
        else if (Input.GetAxisRaw("Vertical") < 0 && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.down);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0 && !cooldown)
        {
            cooldown = true;
            SetNewPositions(Vector3.right);
        }
        else if (Input.GetKey("r"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown("["))
        {
            SaveGame();
        }
        else if (Input.GetKeyDown("]"))
        {
            LoadGame();
        }
        SetNewNPCPositions();
        MoveObjects();

        cooldown = false;

    }
    private void SetNewNPCPositions()
    {
        foreach (GameObject o in allEyeballs)
        {
            EyeballController eyeballController = o.GetComponent<EyeballController>();
            if (eyeballController.isMoving == false && !eyeballController.playerInRange && !eyeballController.turningCRRunning)
            {    
                if(!eyeballController.moveCRRunning && !eyeballController.turningCRRunning)
                {
                    eyeballController.moveCRRunning = true;
                    StartCoroutine(SetNewPosAfterCooldown(eyeballController, Random.Range(eyeballController.movementMinDelay, eyeballController.movementMaxDelay)));
                }
                
            }
            
        }
    }
    private IEnumerator SetNewPosAfterCooldown(EyeballController controller, float delay)
    {

        yield return new WaitForSeconds(delay);
        Vector3 newPos = Tools.VectorToMoveEyeball(controller);
        if (!controller.turningCRRunning)
        {
            if (newPos == Vector3.up) { controller.directionFacing = DirectionFacing.UP; }
            else if (newPos == Vector3.down) { controller.directionFacing = DirectionFacing.DOWN; }
            else if (newPos == Vector3.left) { controller.directionFacing = DirectionFacing.LEFT; }
            else if (newPos == Vector3.right) { controller.directionFacing = DirectionFacing.RIGHT; }
            controller.isMoving = true;
            controller.newPos += newPos;


        }
        controller.moveCRRunning = false;

    }
    public void MoveObjects()
    {
        for (int i = allBlocks.Count - 1; i >= 0; i--)
        {
            GameObject o = (GameObject)allBlocks[i];
            BlockController blockController = o.GetComponent<BlockController>();
            if (blockController.isMoving)
            {
                if (blockController.transform.position == blockController.newPos)
                {
                    blockController.isMoving = false;
                }
                else
                {
                    blockController.transform.position = Vector3.MoveTowards(blockController.transform.position, blockController.newPos, Time.deltaTime * speed);
                }
            }
            else
            {
                blockController.newPos = blockController.transform.position;
            }
        }
        for (int i = allPlayers.Count - 1; i >= 0; i--)
        {
            GameObject o = (GameObject)allPlayers[i];
            PlayerCollision playerCollision = o.GetComponent<PlayerCollision>();
            if (playerCollision.isMoving)
            {
                if (playerCollision.transform.position == playerCollision.newPos)
                {
                    playerCollision.isMoving = false;
                }
                else
                {
                    playerCollision.transform.position = Vector3.MoveTowards(playerCollision.transform.position, playerCollision.newPos, Time.deltaTime * speed);
                }
            }
            else
            {
                playerCollision.newPos = playerCollision.transform.position;
            }
        }
        for (int i = allEyeballs.Count - 1; i >= 0; i--)
        {
            GameObject o = (GameObject)allEyeballs[i];
            EyeballController eyeballController = o.GetComponent<EyeballController>();
            if (eyeballController.isMoving)
            {
                if (eyeballController.transform.position == eyeballController.newPos)
                {
                    eyeballController.isMoving = false;
                }
                else
                {
                    eyeballController.transform.position = Vector3.MoveTowards(eyeballController.transform.position, eyeballController.newPos, Time.deltaTime * speed);
                }
            }
            else
            {
                eyeballController.newPos = eyeballController.transform.position;
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
                        case "LevelEntrance":
                            if (playerCollision.finished == false)
                            {
                                playerCollision.allowFinishSound = true;
                            }
                            playerCollision.finished = true;

                            obj.transform.localScale -= new Vector3(1.1f * Time.fixedDeltaTime, 1.1f * Time.fixedDeltaTime, 0);

                            if (allPlayers.Count == 1)
                            {
                                //Debug.Log(objectUnder.GetComponent<LevelEntranceController>().level.name);
                                LevelEntranceController entrance = objectUnder.GetComponent<LevelEntranceController>();
                                activeLevelNumber = entrance.levelNumber;
                                SceneManager.LoadScene(entrance.levelName);
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
        for (int i = allPlayers.Count-1; i>= 0; i--)
        {
            GameObject o = (GameObject)allPlayers[i];
            Tools.UpdateCollisionsData<PlayerCollision>(o);
            CheckUnder(o);

        }
        for (int i = allBlocks.Count - 1; i >= 0; i--)
        {
            
            GameObject o = (GameObject)allBlocks[i];
            Tools.UpdateCollisionsData<BlockController>(o);
            CheckUnder(o);
        }
        for (int i = allEyeballs.Count - 1; i >= 0; i--)
        {
            
            GameObject o = (GameObject)allEyeballs[i];
            Tools.UpdateCollisionsData<EyeballController>(o);
        }

    }
    public void SetNewPositions(Vector3 movVec)
    {
        foreach (GameObject o in allBlocks)
        {
            BlockController blockController = o.GetComponent<BlockController>();

            var topRightEntity = blockController.entityOnTopRight;
            var topLeftEntity = blockController.entityOnTopLeft;
            var bottomRightEntity = blockController.entityOnBottomRight;
            var bottomLeftEntity = blockController.entityOnBottomLeft;
            var entity2BlocksAwayOnRight = blockController.entity2BlocksAwayOnRight;
            var entity2BlocksAwayOnLeft = blockController.entity2BlocksAwayOnLeft;
            var entity2BlocksAwayOnUp = blockController.entity2BlocksAwayOnUp;
            var entity2BlocksAwayOnDown = blockController.entity2BlocksAwayOnDown;
            var entityNewPos = new Vector3(0, 0, 0);

            GameObject[] diagonalEntities = { topRightEntity, topLeftEntity, bottomRightEntity, bottomLeftEntity, entity2BlocksAwayOnRight, entity2BlocksAwayOnLeft, entity2BlocksAwayOnUp, entity2BlocksAwayOnDown };

            foreach (GameObject diagonalEntity in diagonalEntities)
            {
                entityNewPos = Tools.GetDiagonalEntityNewPos(diagonalEntity);
                blockController.canMoveRight = blockController.canMoveRight && entityNewPos != blockController.transform.localPosition + Vector3.right;
                blockController.canMoveLeft = blockController.canMoveLeft && entityNewPos != blockController.transform.localPosition + Vector3.left;
                blockController.canMoveUp = blockController.canMoveUp && entityNewPos != blockController.transform.localPosition + Vector3.up;
                blockController.canMoveDown = blockController.canMoveDown && entityNewPos != blockController.transform.localPosition + Vector3.down;
            }

            bool condition = (blockController.canMoveUp && blockController.playerOnDown != null && blockController.playerOnDown.GetComponent<PlayerCollision>().canMoveUp && movVec == Vector3.up) ||
                            (blockController.canMoveDown && blockController.playerOnUp != null && blockController.playerOnUp.GetComponent<PlayerCollision>().canMoveDown && movVec == Vector3.down) ||
                            (blockController.canMoveLeft && blockController.playerOnRight != null && blockController.playerOnRight.GetComponent<PlayerCollision>().canMoveLeft && movVec == Vector3.left) ||
                            (blockController.canMoveRight && blockController.playerOnLeft != null && blockController.playerOnLeft.GetComponent<PlayerCollision>().canMoveRight && movVec == Vector3.right);
            //Debug.Log("Block can move up:"+ blockController.canMoveUp);
            //Debug.Log(blockController.playerOnDown != null);

            if (condition)
            {
                blockController.isMoving = true;
                blockController.newPos += movVec;
            }
        }
        foreach (GameObject o in allPlayers)
        {
            
            PlayerCollision playerCollision = o.GetComponent<PlayerCollision>();

            var topRightEntity = playerCollision.entityOnTopRight;
            var topLeftEntity = playerCollision.entityOnTopLeft;
            var bottomRightEntity = playerCollision.entityOnBottomRight;
            var bottomLeftEntity = playerCollision.entityOnBottomLeft;
            var entity2BlocksAwayOnRight = playerCollision.entity2BlocksAwayOnRight;
            var entity2BlocksAwayOnLeft = playerCollision.entity2BlocksAwayOnLeft;
            var entity2BlocksAwayOnUp = playerCollision.entity2BlocksAwayOnUp;
            var entity2BlocksAwayOnDown = playerCollision.entity2BlocksAwayOnDown;
            var entityNewPos = new Vector3(0, 0, 0);

            GameObject[] diagonalEntities = { topRightEntity, topLeftEntity, bottomRightEntity, bottomLeftEntity, entity2BlocksAwayOnRight, entity2BlocksAwayOnLeft, entity2BlocksAwayOnUp, entity2BlocksAwayOnDown };

            foreach (GameObject diagonalEntity in diagonalEntities)
            {
                entityNewPos = Tools.GetDiagonalEntityNewPos(diagonalEntity);
                playerCollision.canMoveRight = playerCollision.canMoveRight && entityNewPos != playerCollision.transform.localPosition + Vector3.right;
                playerCollision.canMoveLeft = playerCollision.canMoveLeft && entityNewPos != playerCollision.transform.localPosition + Vector3.left;
                playerCollision.canMoveUp = playerCollision.canMoveUp && entityNewPos != playerCollision.transform.localPosition + Vector3.up;
                playerCollision.canMoveDown = playerCollision.canMoveDown && entityNewPos != playerCollision.transform.localPosition + Vector3.down;
            }

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
                playerCollision.isMoving = true;
                playerCollision.newPos += movVec;
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
        completedLevels[activeLevelNumber] = true;
        
    }
    public static void SaveGame()
    {
        if(dataToLoad != null)
        {
            dataToLoad = null;
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.gd");

        SaveData data = new SaveData
        {
            scene = SceneManager.GetActiveScene().buildIndex,
            allPlayers_position = new float[allPlayers.Count][],
            allPlayers_newPos = new float[allPlayers.Count][],
            allPlayers_movementVector = new float[allPlayers.Count][],
            allPlayers_isMoving = new bool[allPlayers.Count],
            allPlayers_directionFacing = new DirectionFacing[allPlayers.Count],
            allButtons_pressed = new bool[allButtons.Count],
            allLevers_stateOn = new bool[allLevers.Count],
            allBlocks_position = new float[allBlocks.Count][],
            allBlocks_newPos = new float[allBlocks.Count][],
            allBlocks_movementVector = new float[allBlocks.Count][],
            allBlocks_isMoving = new bool[allBlocks.Count],
            allCounters_enabled = new bool[allCounters.Count],
            allDoors_enabled = new bool[allDoors.Count],
            inventory = new ItemController.ItemType[10],
            allItems_enabled = new bool[allItems.Count],
            completedLevels = new bool[completedLevels.Length],
            //allGates_enabled = new bool[allGates.Count]
        };

        // adding infomation to savadata object
        data.inventory = inventory;
        data.completedLevels = completedLevels;
        for (int i = 0; i < allPlayers.Count; i++)
        {
            GameObject player = (GameObject)allPlayers[i];
            PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();

            data.allPlayers_position[i] = Tools.Vector3ToFloatArray(player.transform.localPosition);
            data.allPlayers_newPos[i] = Tools.Vector3ToFloatArray(playerCollision.newPos);
            data.allPlayers_movementVector[i] = Tools.Vector3ToFloatArray(playerCollision.movementVec);
            data.allPlayers_isMoving[i] = playerCollision.isMoving;
        }
        for (int i = 0; i < allButtons.Count; i++)
        {
            GameObject button = (GameObject)allButtons[i];
            ButtonController buttonController = button.GetComponent<ButtonController>();

            data.allButtons_pressed[i] = buttonController.pressed;
        }
        for (int i = 0; i < allLevers.Count; i++)
        {
            GameObject lever = (GameObject)allLevers[i];
            LeverController leverController = lever.GetComponent<LeverController>();

            data.allLevers_stateOn[i] = leverController.stateOn;
        }
        for (int i = 0; i < allBlocks.Count; i++)
        {
            GameObject block = (GameObject)allBlocks[i];
            BlockController blockController = block.GetComponent<BlockController>();

            data.allBlocks_position[i] = Tools.Vector3ToFloatArray(block.transform.localPosition);
            data.allBlocks_newPos[i] = Tools.Vector3ToFloatArray(blockController.newPos);
            data.allBlocks_movementVector[i] = Tools.Vector3ToFloatArray(blockController.movementVec);
            data.allBlocks_isMoving[i] = blockController.isMoving;
        }
        for (int i = 0; i < allCounters.Count; i++)
        {
            GameObject counter = (GameObject)allCounters[i];

            data.allCounters_enabled[i] = counter.activeSelf;
        }
        for (int i = 0; i < allDoors.Count; i++)
        {
            GameObject door = (GameObject)allDoors[i];

            data.allDoors_enabled[i] = door.activeSelf;
        }
        /*for (int i = 0; i < allGates.Count; i++)
        {
            GameObject gate = (GameObject)allGates[i];
            
            data.allGates_enabled[i] = gate.activeSelf;
        }*/
        for (int i = 0; i < allItems.Count; i++)
        {
            GameObject item = (GameObject)allItems[i];

            data.allItems_enabled[i] = item.activeSelf;
        }
        



        bf.Serialize(file, data);
        file.Close();
    }
    public static void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.gd", FileMode.Open);
            dataToLoad = (SaveData)bf.Deserialize(file);
            file.Close();

            SceneManager.LoadScene(dataToLoad.scene);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }
    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        inventory = dataToLoad.inventory;
        completedLevels = dataToLoad.completedLevels;
        for (int i = 0; i < dataToLoad.allPlayers_position.Length; i++)
        {
            GameObject player = (GameObject)allPlayers[i];
            PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();

            Vector3 position = Tools.FloatArrayToVector3(dataToLoad.allPlayers_position[i]);
            Vector3 newPos = Tools.FloatArrayToVector3(dataToLoad.allPlayers_newPos[i]);
            Vector3 movementVec = Tools.FloatArrayToVector3(dataToLoad.allPlayers_movementVector[i]);
            bool isMoving = dataToLoad.allPlayers_isMoving[i];

            player.transform.localPosition = position;
            playerCollision.newPos = newPos;
            playerCollision.movementVec = movementVec;
            playerCollision.isMoving = isMoving;
            //Debug.Log(position);
            //Debug.Log(newPos);
        }
        for (int i = 0; i < dataToLoad.allButtons_pressed.Length; i++)
        {
            GameObject button = (GameObject)allButtons[i];
            button.GetComponent<ButtonController>().pressed = dataToLoad.allButtons_pressed[i];
        }
        for (int i = 0; i < dataToLoad.allLevers_stateOn.Length; i++)
        {
            GameObject lever = (GameObject)allLevers[i];
            lever.GetComponent<LeverController>().stateOn = dataToLoad.allLevers_stateOn[i];
        }
        for (int i = 0; i < dataToLoad.allBlocks_position.Length; i++)
        {
            GameObject block = (GameObject)allBlocks[i];
            BlockController blockController = block.GetComponent<BlockController>();

            Vector3 position = Tools.FloatArrayToVector3(dataToLoad.allBlocks_position[i]);
            Vector3 newPos = Tools.FloatArrayToVector3(dataToLoad.allBlocks_newPos[i]);
            Vector3 movementVec = Tools.FloatArrayToVector3(dataToLoad.allBlocks_movementVector[i]);
            bool isMoving = dataToLoad.allBlocks_isMoving[i];

            block.transform.localPosition = position;
            blockController.newPos = newPos;
            blockController.movementVec = movementVec;
            blockController.isMoving = isMoving;
            //Debug.Log(position);
            //Debug.Log(newPos);
        }
        for (int i = 0; i < dataToLoad.allCounters_enabled.Length; i++)
        {
            GameObject counter = (GameObject)allCounters[i];
            counter.SetActive(dataToLoad.allCounters_enabled[i]);
        }
        for (int i = 0; i < dataToLoad.allDoors_enabled.Length; i++)
        {
            GameObject door = (GameObject)allDoors[i];
            door.SetActive(dataToLoad.allDoors_enabled[i]);
        }
        /*for (int i = 0; i < dataToLoad.allGates_enabled.Length; i++)
        {
            GameObject gate = (GameObject)allGates[i];
            gate.SetActive(dataToLoad.allGates_enabled[i]);
        }*/
        for (int i = 0; i < dataToLoad.allItems_enabled.Length; i++)
        {
            GameObject item = (GameObject)allItems[i];
            item.SetActive(dataToLoad.allItems_enabled[i]);
        }
        dataToLoad = null;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
