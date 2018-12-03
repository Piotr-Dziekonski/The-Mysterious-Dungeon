using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour {

    public enum ItemType { None, SilverKey, GoldKey, Message, GoldPotion, GreenPotion}
    public bool pickedUp;
    public ItemType itemType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        


        /*switch (itemType)
        {
            case ItemType.SilverKey:
                
                break;
            default:
                break;
        }*/
        
    }
}