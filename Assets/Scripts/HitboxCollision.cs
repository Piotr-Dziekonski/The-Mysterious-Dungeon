using UnityEngine;

public class HitboxCollision : MonoBehaviour {

    public Rigidbody2D player;
    public PlayerMovement movement;
    public Vector3 defaultPosition;

    private void Start()
    {
        defaultPosition = GetComponent<Transform>().localPosition;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.tag);
        if (collision.tag == "Player")
        {
            
            switch (GetComponent<Collider2D>().name)
            {
                case "HitboxUp":
                    GetComponent<Transform>().position += Vector3.up;
                    //transform.Translate(Vector3.up);
                    break;
                case "HitboxDown":
                    GetComponent<Transform>().position += Vector3.down;
                    break;
                case "HitboxLeft":
                    GetComponent<Transform>().position += Vector3.left;
                    break;
                case "HitboxRight":
                    GetComponent<Transform>().position += Vector3.right;
                    break;
                default:
                    break;
            }
            
        }

        if(collision.tag != "PlayerPredictionHitbox")
        {
            switch (GetComponent<Collider2D>().name)
            {
                case "HitboxUp":
                    movement.canMoveUp = false;
                    break;
                case "HitboxDown":
                    movement.canMoveDown = false;
                    break;
                case "HitboxLeft":
                    movement.canMoveLeft = false;
                    break;
                case "HitboxRight":
                    movement.canMoveRight = false;
                    break;
                default:
                    break;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.tag != "PlayerPredictionHitbox")
        {
            switch (GetComponent<Collider2D>().name)
            {
                case "HitboxUp":
                    movement.canMoveUp = true;
                    break;
                case "HitboxDown":
                    movement.canMoveDown = true;
                    break;
                case "HitboxLeft":
                    movement.canMoveLeft = true;
                    break;
                case "HitboxRight":
                    movement.canMoveRight = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "PlayerPredictionHitbox")
        {
            switch (GetComponent<Collider2D>().name)
            {
                case "HitboxUp":
                    movement.canMoveUp = false;
                    break;
                case "HitboxDown":
                    movement.canMoveDown = false;
                    break;
                case "HitboxLeft":
                    movement.canMoveLeft = false;
                    break;
                case "HitboxRight":
                    movement.canMoveRight = false;
                    break;
                default:
                    break;
            }
        }
    }

}
