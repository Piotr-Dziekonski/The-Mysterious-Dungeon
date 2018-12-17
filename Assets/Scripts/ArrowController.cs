using UnityEngine;
using Utils;

public class ArrowController : MonoBehaviour {

    [HideInInspector]
    public GameObject playerOnIt;
    public DirectionFacing directionFacing;
    private GameObject[] objectsUnderIt;

    void Start () {
        if (directionFacing == DirectionFacing.LEFT)
        {
            transform.Rotate(Vector3.forward * 180);
        }
        else if (directionFacing == DirectionFacing.UP)
        {
            transform.Rotate(Vector3.forward * 90);
        }
        else if (directionFacing == DirectionFacing.DOWN)
        {
            transform.Rotate(Vector3.forward * -90);
        }
    }
	void Update () {

        objectsUnderIt = Tools.GetObjectsUnderSelf(gameObject);
        if (objectsUnderIt != null)
        {
            foreach (GameObject o in objectsUnderIt)
            {
                if (o.tag == "Player")
                {
                    playerOnIt = o;
                    break;
                }
            }
        }
    }
    
}
