using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveOrLoad : MonoBehaviour {

	// Use this for initialization
	public void DisableSaveText()
    {
        GameObject.Find("MessageUI").GetComponent<Animator>().SetBool("saved", false);
    }
    public void DisableLoadText()
    {
        GameObject.Find("MessageUI").GetComponent<Animator>().SetBool("loaded", false);
    }
}
