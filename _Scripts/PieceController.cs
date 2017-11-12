using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour {

	public Vector2 origin;
	public bool moveable = true;
	public bool inNoZone = false;

	private bool isDragging = false;

	void Start() {
		origin = transform.position;
	}

	void Update() {
		if (isDragging) {
			if (Input.GetKey(KeyCode.LeftArrow)) {
				transform.Rotate(new Vector3(0, 0, -90) * Time.deltaTime);
			}
			if (Input.GetKey(KeyCode.RightArrow)) {
				transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
			}
		}
	}

	// Only called when dragged with the mouse
	private void OnMouseDrag() {
		isDragging = true;
		if (moveable) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			transform.position = mousePos;
		}
	}

	private void OnMouseUp() {
		isDragging = false;
		if (inNoZone) {
			resetPosition();
		}
	}

	public void resetPosition() {
		transform.position = origin;
		transform.rotation = Quaternion.identity;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("InNoZoneT");

		if (other.CompareTag("NoZone")) {
			inNoZone = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		Debug.Log("Not InNoZoneT");

		if (other.CompareTag("NoZone")) {
			inNoZone = false;
		}
	}
}
