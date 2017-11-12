using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoZoneController : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("InNoZone");

		if (other.CompareTag("Piece")) {
			other.gameObject.GetComponent<PieceController>().inNoZone = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		Debug.Log("Not InNoZone");

		if (other.CompareTag("Piece")) {
			other.gameObject.GetComponent<PieceController>().inNoZone = false;
		}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		Debug.Log("InNoZone");

		if (other.collider.gameObject.CompareTag("Piece")) {
			other.collider.gameObject.GetComponent<PieceController>().inNoZone = true;
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		Debug.Log("Not InNoZone");

		if (other.collider.gameObject.CompareTag("Piece")) {
			other.collider.gameObject.GetComponent<PieceController>().inNoZone = false;
		}
	}
}
