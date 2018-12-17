using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {
    public bool pressed;

    public GameObject standingBlock;
    public List<GameObject> neededBlocks = new List<GameObject>();
    private List<BlockController> blockControllers = new List<BlockController>();

    // Use this for initialization
    void Start () {
        foreach(GameObject block in neededBlocks)
        {
            blockControllers.Add(block.GetComponent<BlockController>());
        }
	}
	
	// Update is called once per frame
	void Update () {

        if(blockControllers.Find(x => x.gameObject == standingBlock))
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
	}
}
