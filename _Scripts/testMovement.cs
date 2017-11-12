using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMovement : MonoBehaviour {

	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		movement();
	}

	private void movement() {
		if (Input.GetKeyDown(KeyCode.W)) {
			rb.AddForce(new Vector3(0, 100f, 0), ForceMode2D.Force);
		}

		float horz = Input.GetAxis ("Horizontal");

		float velocityMagnitudeH = 25 * horz;
		Vector2 newVelocity = new Vector2 (velocityMagnitudeH, 0f);

		rb.velocity = newVelocity;
	}
}
