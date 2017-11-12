using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLights : MonoBehaviour
{
    public Vector2 startingPoint;//rightmost point
    public Vector2 endingPoint;//leftmost point
    public bool goingFromStartToEnd;

    private Vector2 actualMotion;

	// Use this for initialization
	void Start ()
    {
        transform.position = startingPoint;
        actualMotion = endingPoint - startingPoint;
        goingFromStartToEnd = true;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if(goingFromStartToEnd)
            transform.Translate(actualMotion.normalized * 0.1f);
        else if(!goingFromStartToEnd)
            transform.Translate(actualMotion.normalized * -0.1f);

        if (((transform.position.x <= endingPoint.x + 0.15f) && (transform.position.x >= endingPoint.x - 0.15f)) && ((transform.position.y <= endingPoint.y + 0.15f) && (transform.position.y >= endingPoint.y - 0.15f)))
            goingFromStartToEnd = false;
        else if (((transform.position.x <= startingPoint.x + 0.15f) && (transform.position.x >= startingPoint.x - 0.15f)) && ((transform.position.y <= startingPoint.y + 0.15f) && (transform.position.y >= startingPoint.y - 0.15f)))
            goingFromStartToEnd = true;





    }
}
