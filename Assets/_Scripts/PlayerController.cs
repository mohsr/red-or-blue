using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Prime31;

public class PlayerController : MonoBehaviour {
	public float speed = 6.0f;
	public float gravity = -25f;
	public float MinJumpHeight = 0.6f;
	public float MaxJumpHeight = 2.6f;

	public float jumpBuffer = 0.15f;
	public float jumpHeight = 3f;
	public float fallingGravModifier = 1.35f;

	public Vector2 wallJumpVelocity = new Vector2(10, 10);
	public float wallsSlideModifier = 1.4f;
	public float postWallJumpDelayBuffer = 0.5f;
	public float preWallJumpBuffer = 0.15f;
	public bool allowSwitch = true;


	[HideInInspector]
	private float postWallJumpDelayBuffer_counter = 0;
	private float jump_buffer_counter = 0;

	private float preWallJumpBuffer_counter = 0;
	private bool isPreWallJumpBuffer = false;

	private bool isPostWallJumpDelayBuffer = false;
	private int postWallJumpDir = 0;
	private bool isBufferedJump = false;

	private bool isCollidingWall = false;
	private bool isWallSliding = false;

	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private Animator _animator;
	private Rigidbody2D _rigidbody2d;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;

    
    // combat
    public int health = 3;
    public float invincibleTimeAfterHurt = 2;
    [HideInInspector]
    Collider2D[] myColliders;
    bool alreadyHurt = false;

    
    void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
		_animator = GetComponent<Animator> ();
		_rigidbody2d = GetComponent<Rigidbody2D> ();

		// Subscribe to event listners
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
        
        myColliders = GetComponents<Collider2D>();
    }

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
        // bail out on plain old ground hits cause they arent very interesting
        if ( hit.normal.y == 1f )
			return;
		if (hit.collider.tag == "Ground" || hit.collider.tag == "RedVert" || hit.collider.tag == "BlueVert")
			isCollidingWall = true;
		else
			isCollidingWall = false;

        // logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
        //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
    }

    IEnumerator Hurt()
    {
        Debug.Log("hurt");
        health--;
        if (health <= 0)
            GetComponent<PlayerDie>().Die();
        else
        {
            alreadyHurt = true;
            GetComponent<Animator>().SetLayerWeight(1, 1);
            yield return new WaitForSeconds(invincibleTimeAfterHurt);
            alreadyHurt = false;
            GetComponent<Animator>().SetLayerWeight(1, 0);
        }
    }

    IEnumerator Knockback(float knockDur)
    {
        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            _velocity = new Vector2(_velocity.x * -5, _velocity.y);
        }

        yield return 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Enemy")
        {
            foreach (ContactPoint2D point in collision.contacts)
            {
                Debug.DrawLine(point.point, point.point + point.normal, Color.red, 10);
                if (point.normal.y >= 0.6f)
                {
					var jumpHeight = MinJumpHeight;
					if (isBufferedJump || Input.GetButton ("Jump"))
						jumpHeight = MaxJumpHeight;
					_velocity = new Vector2(_velocity.x, Mathf.Sqrt(2f * jumpHeight * -gravity));
                    EnemyController enemy = collision.collider.GetComponent<EnemyController>();
                    if (enemy != null)
                        enemy.stomped = true;
                }
                else {
                    if (!alreadyHurt)
                    {
                        StartCoroutine(Hurt());
                        alreadyHurt = true;
                        StartCoroutine(Knockback(0.02f));
                    }
                }
            }
        }
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
		if( _controller.isGrounded ) {
			_velocity.y = 0;
			allowSwitch = true;
		}

		if( Input.GetKey( KeyCode.RightArrow ) )
		{
			_animator.SetBool ("Idle", false);
			_animator.SetBool ("Walking", true);
			normalizedHorizontalSpeed = 1;

			if (_controller.isCollidingRight && isCollidingWall && !_controller.isGrounded) {
				checkWallJump (1);
				if(_velocity.y < 0 && !isPreWallJumpBuffer) 
					isWallSliding = true;
			}
			//flip sprite if necessary
			if( transform.localScale.x < 0f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			//prevents infinite walljumping on same wall
			if (isPostWallJumpDelayBuffer  && postWallJumpDir < 0)
				normalizedHorizontalSpeed = 0.25f;

			//(Doesnt work/not fully implemented) allows small window of time to wall jump in opposite direction of wall.
			if (isPreWallJumpBuffer) {
				isWallSliding = true;
				normalizedHorizontalSpeed = 0;
			}
			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		
		else if( Input.GetKey( KeyCode.LeftArrow ) )
		{
			_animator.SetBool ("Idle", false);
			_animator.SetBool ("Walking", true);
			normalizedHorizontalSpeed = -1;

			if (_controller.isCollidingLeft && isCollidingWall && !_controller.isGrounded) {
				checkWallJump (-1);
				if(_velocity.y < 0 && !isPreWallJumpBuffer) 
					isWallSliding = true;
			}
			if( transform.localScale.x > 0f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if (isPostWallJumpDelayBuffer && postWallJumpDir > 0)
				normalizedHorizontalSpeed = -.25f;

			if (isPreWallJumpBuffer) {
				normalizedHorizontalSpeed = 0;
			}
			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			_animator.SetBool ("Walking", false);
			if (!_animator.GetBool("Idle"))
				_animator.SetBool ("Idle", true);
			normalizedHorizontalSpeed = 0;

			// if( _controller.isGrounded )
			// 	_animator.Play( Animator.StringToHash( "Idle" ) );
		}

		if (_velocity.y != 0) {
			_animator.SetBool ("Jumping", true);
			_animator.SetBool ("Walking", false);
			_animator.SetBool ("Idle", false);
		} else {
			_animator.SetBool ("Jumping", false);
		}
			
		//check if not wallsliding
		if (isWallSliding && 
			(((!_controller.isCollidingRight && !_controller.isCollidingLeft)
				|| Input.GetKeyUp( KeyCode.RightArrow ) || Input.GetKeyUp( KeyCode.LeftArrow ))
				|| _controller.isGrounded) && !isPreWallJumpBuffer)
				isWallSliding = false;

		// we can only jump whilst grounded
		// if( _controller.isGrounded && Input.GetKeyDown( KeyCode.Space ) )
		// {
		// 	_velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
		// 	// _animator.Play( Animator.StringToHash( "Jump" ) );
		// }
		if (isPostWallJumpDelayBuffer) {
			postWallJumpDelayBuffer_counter += Time.deltaTime;
			if (postWallJumpDelayBuffer_counter > postWallJumpDelayBuffer) {
				postWallJumpDelayBuffer_counter = 0;
				isPostWallJumpDelayBuffer = false;
			}
		}

		if (isPreWallJumpBuffer) {
			preWallJumpBuffer_counter += Time.deltaTime;
			if (preWallJumpBuffer_counter >= preWallJumpBuffer)
				isPreWallJumpBuffer = false;
		}

		if (isBufferedJump) {
			jump_buffer_counter += Time.deltaTime;
			if (jump_buffer_counter >= jumpBuffer)
				isBufferedJump = false;
		}

		if (((Input.GetButtonDown ("Jump")) || isBufferedJump) && _controller.isGrounded) {
			allowSwitch = true;
			isBufferedJump = false;
			_velocity = new Vector2(_velocity.x, Mathf.Sqrt( 2f * MaxJumpHeight * -gravity ));
		}

		if (Input.GetButtonUp ("Jump") && !isPostWallJumpDelayBuffer) {
			if (_velocity.y > Mathf.Sqrt( 2f * MinJumpHeight * -gravity )) {
				_velocity = new Vector2(_velocity.x, Mathf.Sqrt( 2f * MinJumpHeight * -gravity ));
			}
		}

		if (Input.GetButtonDown ("Jump") && !_controller.isGrounded) {
			isBufferedJump = true;
			jump_buffer_counter = 0;
		}


		_velocity.x = Vector2.MoveTowards(new Vector2 (_velocity.x, 0f), new Vector2 (normalizedHorizontalSpeed * speed, 0f), 1f ).x;

		//modify gravity if falling
		var grav = (_velocity.y < 0) ? gravity * fallingGravModifier : gravity;

		// apply gravity before moving

		_velocity.y += grav * Time.deltaTime;

		if (isWallSliding)
			_velocity.y /= wallsSlideModifier;

        _controller.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
        _velocity = _controller.velocity;
    }

	void checkWallJump(int sign)
	{
		//attempts at making a preWallJumpBuffer
//		if (!isPreWallJumpBuffer && ((Input.GetKeyDown (KeyCode.LeftArrow) && sign < 0) || (Input.GetKeyDown (KeyCode.RightArrow) && sign > 0))) {
//			isPreWallJumpBuffer = true;
//			preWallJumpBuffer_counter = 0;
//		}

		if (Input.GetButtonDown ("Jump")) {
			allowSwitch = true;
			_velocity.x = -sign * wallJumpVelocity.x;
			_velocity.y = wallJumpVelocity.y;
			isWallSliding = false;
			isPostWallJumpDelayBuffer = true;
			postWallJumpDir = -sign;
		}
	}
}