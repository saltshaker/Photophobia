using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class PlayerMovement : MonoBehaviour {

	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public float walljump = 8f;
	public bool playerinlight;
	public bool moveable = false;
	public float normalizedHorizontalSpeed = 0;
	public string collisionsub;
	public Sprite jumpSprite;
	public Sprite fallSprite;
	
	[HideInInspector]


	private CharacterController2D _controller;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private GameObject player;
	private GameObject[] lights;

	private AudioSource jumpSource;
	private Animator anim;

	private void Start()
	{
		player = GameObject.FindWithTag("Player");
		lights = GameObject.FindGameObjectsWithTag("Light");
		jumpSource = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		if (anim != null) {
			resetTriggers();
			anim.SetTrigger("Idle");
		}
	}

	void Awake()
	{
		_controller = GetComponent<CharacterController2D> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!moveable) {
			return;
		}

		if( _controller.isGrounded )
			_velocity.y = 0;
		
		playerinlight = PlayerIsInLight ();
		string collisionstate = _controller.collisionState.ToString();

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			if (anim != null) {
				resetTriggers();
				anim.SetTrigger("Run");
			}
		}
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
				normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			if (anim != null) {
				resetTriggers();
				anim.SetTrigger("Run");
			}
		}
		else
		{
			normalizedHorizontalSpeed = 0;
			resetTriggers();
			anim.SetTrigger("Idle");
		}


		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetKeyDown( KeyCode.UpArrow ) && (!PlayerIsInLight()) )
		{
			_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
			jumpSource.Play();
			resetTriggers();
			GetComponent<SpriteRenderer>().sprite = jumpSprite;
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		if (Input.GetKey (KeyCode.RightArrow) && !_controller.isGrounded ) 
		{
			int first = collisionstate.IndexOf ("r: ");
			collisionsub = collisionstate.Substring (first, 7);

			if ((_velocity.y < 0) && (collisionsub == "r: True")) {
				gravity = -10f;
				GetComponent<SpriteRenderer>().sprite = fallSprite;
			}
			
			if (Input.GetKeyDown (KeyCode.UpArrow) && (!playerinlight) && (collisionsub == "r: True")) {
				jumpSource.Play();
				gravity = -25f;
				_velocity.x = -_velocity.x - walljump;
				_velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
				resetTriggers();
				GetComponent<SpriteRenderer>().sprite = jumpSprite;
			}

		} 
		else if (Input.GetKey (KeyCode.LeftArrow) && !_controller.isGrounded ) 
		{
			int first = collisionstate.IndexOf ("l: ");
			collisionsub = collisionstate.Substring (first, 7);

			if ((_velocity.y < 0) && (collisionsub == "l: True")) {
				gravity = -10f;
				GetComponent<SpriteRenderer>().sprite = fallSprite;
			}

			if (Input.GetKeyDown (KeyCode.UpArrow) && (!playerinlight) && (collisionsub == "l: True")) {
				jumpSource.Play();
				gravity = -25f;
				_velocity.x = -_velocity.x + walljump;
				_velocity.y = Mathf.Sqrt (2f * jumpHeight * -gravity);
				resetTriggers();
				GetComponent<SpriteRenderer>().sprite = jumpSprite;
			}

		}

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;


		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		/*if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}*/

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
		gravity = -25f;

		if (_velocity.y > 0.05) {
			resetTriggers();
			GetComponent<SpriteRenderer>().sprite = jumpSprite;
			anim.enabled = false;
		}
		else if (_velocity.y < -0.05) {
			resetTriggers();
			GetComponent<SpriteRenderer>().sprite = fallSprite;
			anim.enabled = false;
		}
		else {
			anim.enabled = true;
		}
	}

	bool PlayerIsInLight()
	{
		foreach (GameObject light in lights) {
			float distanceX = player.transform.position.x - light.transform.position.x;
			float distanceY = player.transform.position.y - light.transform.position.y;

			RaycastHit2D hit = Physics2D.Raycast(light.transform.position, new Vector2(distanceX, distanceY), 500f);
			if (hit)
			{
				if ((hit.transform.tag != "Wall")&&(hit.transform.tag != "Piece")&&(hit.transform.tag != "Ground")) 
				{
					return true;
				}
			}
		}
		return false;
	}

	private void resetTriggers() {
		anim.ResetTrigger("Idle");
		anim.ResetTrigger("Run");
		anim.ResetTrigger("Jump");
	}
}
