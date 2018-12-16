using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEntranceController : MonoBehaviour {

    public bool completed;
    public int levelNumber;
    public string levelName;
    public Sprite spriteIfCompleted;



	// Use this for initialization
	void Start () {
        if (GameController.completedLevels[levelNumber] == true)
        {
            completed = true;
            //gameObject.SetActive(false);
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(GameController.completedLevels[levelNumber]);
        if (GameController.completedLevels[levelNumber] == true)
        {
            completed = true;
            //gameObject.SetActive(false);
        }
        if (completed)
        {
            GetComponent<SpriteRenderer>().sprite = spriteIfCompleted;
        }
	}
}
