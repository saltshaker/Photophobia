using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Color dark = new Color(0.5f, 0.5f, 0.5f);
    private Color light = new Color(1f, 1f, 1f);
    private SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer>();
	}

	public void lit() {
		rend.color = light;
	}

	public void dim() {
		rend.color = dark;
	}
}
