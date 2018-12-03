using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public enum ElementType { None, SilverDoor, GoldDoor }
    public GameObject[] connectedObjects;
    public bool stateOn = false;
    public ElementType elementType;

    [HideInInspector]
    public ItemController.ItemType itemNeeded;

    void Start()
    {
        switch (elementType)
        {
            case ElementType.SilverDoor:
                itemNeeded = ItemController.ItemType.SilverKey;
                break;
            case ElementType.GoldDoor:
                itemNeeded = ItemController.ItemType.GoldKey;
                break;
            case ElementType.None:
                itemNeeded = ItemController.ItemType.None;
                break;
        }
    }

}