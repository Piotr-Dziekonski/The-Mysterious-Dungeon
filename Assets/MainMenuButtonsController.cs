using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonsController : MonoBehaviour {

    public Button newGame, loadGame, exit;
    // Use this for initialization
    void Start () {
        newGame.onClick.AddListener(OnNewGameButtonClick);
        loadGame.onClick.AddListener(OnLoadGameButtonClick);
        exit.onClick.AddListener(OnExitButtonClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnNewGameButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void OnLoadGameButtonClick()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        GameController.LoadGame();
    }
    void OnExitButtonClick()
    {
        Application.Quit();
    }


}
