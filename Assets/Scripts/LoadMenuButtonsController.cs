using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMenuButtonsController : MonoBehaviour
{

    public Button save1, save2, save3, returnButton;
    // Use this for initialization
    void Start()
    {
        save1.onClick.AddListener(() => LoadGame("save1"));
        save2.onClick.AddListener(() => LoadGame("save2"));
        save3.onClick.AddListener(() => LoadGame("save3"));
        returnButton.onClick.AddListener(OnReturnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void LoadGame(string save)
    {
        GameController.LoadGame();



    }
    void OnReturnButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }


}
