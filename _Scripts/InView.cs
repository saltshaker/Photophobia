using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InView : MonoBehaviour {

    private GameObject player;

    private void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void inView() {
		float distanceX = player.transform.position.x - transform.position.x;
		float distanceY = player.transform.position.y - transform.position.y;

        if (!wallInWay(distanceX, distanceY)) {
            player.GetComponent<PlayerController>().lit();
        }
        else {
            player.GetComponent<PlayerController>().dim();
        }
    }

    bool wallInWay(float x, float y) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(x, y), 500f);
        if (hit) {
            if (hit.transform.tag == "Wall") {
                return true;
            }
        }

        return false;
    }
	
	// Update is called once per frame
	void Update () {
		inView();
	}
}