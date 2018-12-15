using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        Play("ZoneTheme");
    }

    private void Update()
    {

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in gameObjects)
        {
            PlayerCollision playerCollision = player.GetComponent<PlayerCollision>();
            if (player.GetComponent<PlayerCollision>().isMoving && !GetComponent<AudioManager>().IsAlreadyPlaying("PlayerWalk"))
            {
                GetComponent<AudioManager>().Play("PlayerWalk");
                break;
            }
            if (playerCollision.allowFinishSound)
            {
                GetComponent<AudioManager>().Play("ExitSound");
                //player.GetComponent<PlayerCollision>().allowFinishSound = false;
            }
            if (playerCollision.allowFinishSound)
            {
                playerCollision.allowFinishSound = false;
            }
        }

        gameObjects = GameObject.FindGameObjectsWithTag("Lever");
        foreach (GameObject lever in gameObjects)
        {
            if (lever.GetComponent<LeverController>().wasPulled && !GetComponent<AudioManager>().IsAlreadyPlaying("LeverPull"))
            {
                GetComponent<AudioManager>().Play("LeverPull");
                break;
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject item in gameObjects)
        {
            if (item.GetComponent<ItemController>().pickedUp)
            {
                if(item.GetComponent<SpriteRenderer>().enabled == false)
                {
                    //Destroy(item);
                    item.SetActive(false);
                }
                GetComponent<AudioManager>().Play("KeyPickUp");
                item.GetComponent<SpriteRenderer>().enabled = false;
                
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in gameObjects)
        {
            if (block.GetComponent<BlockController>().isMoving && !GetComponent<AudioManager>().IsAlreadyPlaying("BlockPush"))
            {
                GetComponent<AudioManager>().Play("BlockPush");
                break;
            }
        }
        gameObjects = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject button in gameObjects)
        {
            
            if (button.GetComponent<ButtonController>().allowSound && !GetComponent<AudioManager>().IsAlreadyPlaying("ButtonClick"))
            {
                GetComponent<AudioManager>().Play("ButtonClick");
                button.GetComponent<ButtonController>().allowSound = false;
                break;
            }
            else if (button.GetComponent<ButtonController>().allowSound && GetComponent<AudioManager>().IsAlreadyPlaying("ButtonClick"))
            {
                button.GetComponent<ButtonController>().allowSound = false;
                break;
            }

        }
        gameObjects = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject door in gameObjects)
        {
            if (door.GetComponent<DoorController>().stateOn)
            {
                if (door.GetComponent<SpriteRenderer>().enabled == false)
                {
                    //Destroy(door);
                    door.SetActive(false);
                }
                GetComponent<AudioManager>().Play("DoorUnlock");
                door.GetComponent<SpriteRenderer>().enabled = false;
                door.GetComponent<CircleCollider2D>().enabled = false;

            }
            

        }

    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }
    public bool IsAlreadyPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return false;
        }
        return s.source.isPlaying;
    }

}
