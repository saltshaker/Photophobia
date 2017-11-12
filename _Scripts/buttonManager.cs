using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonManager : MonoBehaviour {

	//private GameObject GameManager;

	// Use this for initialization
	void Start () {
		//GameManager = GameObject.FindWithTag("GameManager");
	}

	public void playButton() {
		GameManager.instance.SendMessage("beginPlayPhase");
	}

	public void resetButton() {
		GameManager.instance.SendMessage("reset");
	}
}
