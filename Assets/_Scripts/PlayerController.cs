using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float gravity = -25f;
	public float minJumpVelocity = 4f;
	public float maxJumpVelocity = 6.5f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpBuffer = 0.15f;
	public float jumpHeight = 3f;
	public float wallsSlideModifier = 1.25f;

	[HideInInspector]
	// public float fallingGravityScale = 1.65f;
	private float buffer_counter = 0;
	private bool isBufferedJump = false;
	private bool isCollidingWall = false;
	private bool isWallSliding = false;
	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		_controller = GetComponent<CharacterController2D>();

		// Subscribe to event listners
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;
		if (hit.collider.tag == "Ground")
			isCollidingWall = true;
		else
			isCollidingWall = false;

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
	}


	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );
	}

	#endregion

	void Update()
	{
		if( _controller.isGrounded )
			_velocity.y = 0;

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			normalizedHorizontalSpeed = 1;
			if (_controller.isCollidingRight && isCollidingWall && _velocity.y < 0)
				isWallSliding = true;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			normalizedHorizontalSpeed = -1;
			if (_controller.isCollidingLeft && isCollidingWall && _velocity.y < 0)
				isWallSliding = true;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Idle" ) );
		}

		//check if not wallsliding
		if (isWallSliding && ((!_controller.isCollidingRight && !_controller.isCollidingLeft) || _controller.isGrounded))
				isWallSliding = false;

		// we can only jump whilst grounded
		// if( _controller.isGrounded && Input.GetKeyDown( KeyCode.Space ) )
		// {
		// 	_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
		// 	// _animator.Play( Animator.StringToHash( "Jump" ) );
		// }

		if (isBufferedJump) {
			buffer_counter += Time.deltaTime;
			if (buffer_counter > jumpBuffer)
				isBufferedJump = false;
		}

		if (((Input.GetButtonDown ("Jump")) || isBufferedJump) && _controller.isGrounded) {
			isBufferedJump = false;
            //rb2d.transform.Translate(new Vector2(0, 0.05f));
            _velocity = new Vector2(_velocity.x, maxJumpVelocity);
		}

		if (Input.GetButtonUp ("Jump")) {
			if (_velocity.y > minJumpVelocity) {
				_velocity = new Vector2(_velocity.x, minJumpVelocity);
			}
		}

		if (Input.GetButtonDown ("Jump") && !_controller.isGrounded) {
			isBufferedJump = true;
			buffer_counter = 0;
		}


		// TODO: apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * speed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving

		_velocity.y += gravity * Time.deltaTime;

		if (isWallSliding)
			_velocity.y /= wallsSlideModifier;

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}
}