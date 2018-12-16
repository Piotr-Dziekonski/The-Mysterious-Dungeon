using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
public class SaveData {

    public SaveData current;
    public int scene;

    public ItemController.ItemType[] inventory;
    public bool[] completedLevels;

    /*
     * 
     * PLAYERS DATAS
     * 
     * */
    public float[][] allPlayers_position, allPlayers_newPos, allPlayers_movementVector;
    public bool[] allPlayers_isMoving;
    public DirectionFacing[] allPlayers_directionFacing;

    /*
     * 
     * BUTTONS DATAS
     * 
     * */
     
    public bool[] allButtons_pressed;

    /*
     * 
     * LEVER DATAS
     * 
     * */
    public bool[] allLevers_stateOn;

    /*
     * 
     * BLOCKS DATAS
     * 
     * */

    public float[][] allBlocks_position, allBlocks_newPos, allBlocks_movementVector;
    public bool[] allBlocks_isMoving;

    /*
     * 
     * COUNTER DATAS
     * 
     * */
    public bool[] allCounters_enabled;
    /*
     * 
     * DOORS DATAS
     * 
     * */
    public bool[] allDoors_enabled;
    /*
     * 
     * GATES DATAS
     * 
     * */
    //public bool[] allGates_enabled;

    /*
     * 
     * ITEMS DATAS
     * 
     * */
    public bool[] allItems_enabled;

}

