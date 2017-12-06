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
	public float groundDamping = 20f;
 	public float inAirDamping = 5f;
	public float jumpBuffer = 0.15f;
	public float jumpHeight;
	public float fallingGravModifier = 1.35f;
	public Vector2 wallJumpVelocity = new Vector2(10, 10);
	public float postWallJumpDelayBuffer = 0.5f;
	public float wallJumpHandicap = 0.25f;
	public float wallsSlideModifier = 1.4f;
	public bool allowSwitch = true;
	[HideInInspector]
	public int checkPointNum = -1;

	private float jump_buffer_counter = 0;
	private float postWallJumpDelayBuffer_counter = 0;
	private bool isPostWallJumpDelayBuffer = false;
	private int postWallJumpDir = 0;
	private bool isBufferedJump = false;

	private bool isCollidingWall = false;
	private bool isWallSliding = false;

	private float normalizedHorizontalSpeed = 0;
	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private float xVel = 0.0F;
    
    // combat
    public int health = 3;
    public float invincibleTimeAfterHurt = 2;
    [HideInInspector]
    bool alreadyHurt = false;

    
    void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
		_animator = GetComponent<Animator> ();

		health = 3;

		// Subscribe to event listners
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }

	#region Event Listeners

//	This function handles collider hits
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
		if (health <= 0) {
			Debug.Log ("dead");
			GetComponent<PlayerDie> ().Die ();
		}
        else
        {
            alreadyHurt = true;
            GetComponent<Animator>().SetLayerWeight(1, 1);
            yield return new WaitForSeconds(invincibleTimeAfterHurt);
            alreadyHurt = false;
            GetComponent<Animator>().SetLayerWeight(1, 0);
        }
    }

    IEnumerator Knockback(float knockDur, float knockBack)
    {
        float timer = 0;

        while (knockDur > timer)
        {
            timer += Time.deltaTime;
            _velocity = new Vector2(knockBack, _velocity.y);
        }

        yield return 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.collider.gameObject.layer) == "Enemy")
        {
            EnemyController enemy = collision.collider.GetComponent<EnemyController>();
            foreach (ContactPoint2D point in collision.contacts)
            {
                Debug.DrawLine(point.point, point.point + point.normal, Color.red, 10);
                if (point.normal.y >= 0.6f)
                {
					var jumpHeight = MinJumpHeight;
					if (isBufferedJump || Input.GetButton ("Jump"))
						jumpHeight = MaxJumpHeight;
					_velocity = new Vector2(_velocity.x, Mathf.Sqrt(2f * jumpHeight * -gravity));
                    if (enemy != null)
                        enemy.stomped = true;
                }
                else {
                    if (!alreadyHurt) {
						if (enemy != null && enemy.embarrassed)
							return;
                        StartCoroutine(Hurt());
                        alreadyHurt = true;
                        float knockBack = 20;
                        Vector3 dir = (collision.gameObject.transform.position - transform.position).normalized;
                        if (dir.x > 0)
                            knockBack = -knockBack;
                        StartCoroutine(Knockback(0.02f, knockBack));
                    }
                }
            }
        }
		if (collision.transform.name.ToUpper().Contains("PLATE"))
			Destroy(collision.gameObject);
//		ColliderOffOnHurtOther off = collision.transform.GetComponent<ColliderOffOnHurtOther> ();
//		if (off != null)
//			collision.transform.GetComponent<Collider2D> ().enabled = false;
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

		float horAxis = Input.GetAxisRaw( "Horizontal" );

		if( horAxis > 0 )
		{
			if (_controller.isGrounded) {
				_animator.SetBool ("Idle", false);
				_animator.SetBool ("Walking", true);
			}
			normalizedHorizontalSpeed = horAxis;

			if (_controller.isCollidingRight && isCollidingWall && !_controller.isGrounded) {
				checkWallJump (1);
				if(_velocity.y < 0) 
					isWallSliding = true;
			}
			//flip sprite if necessary
			if( transform.localScale.x < 0f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			
			//prevents infinite walljumping on same wall
			if (isPostWallJumpDelayBuffer && postWallJumpDir < 0)
				normalizedHorizontalSpeed = wallJumpHandicap;
		}
		
		else if(horAxis < 0)
		{
			if (_controller.isGrounded) {
				_animator.SetBool ("Idle", false);
				_animator.SetBool ("Walking", true);
			}
			normalizedHorizontalSpeed = horAxis;

			if (_controller.isCollidingLeft && isCollidingWall && !_controller.isGrounded) {
				checkWallJump (-1);
				if(_velocity.y < 0) 
					isWallSliding = true;
			}
			if( transform.localScale.x > 0f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
			
			if (isPostWallJumpDelayBuffer && postWallJumpDir > 0)
				normalizedHorizontalSpeed = - wallJumpHandicap;

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

		if (_velocity.y > 0) {
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
				|| _controller.isGrounded))
				isWallSliding = false;

		if (isPostWallJumpDelayBuffer) {
			postWallJumpDelayBuffer_counter += Time.deltaTime;
			if (postWallJumpDelayBuffer_counter > postWallJumpDelayBuffer) {
				postWallJumpDelayBuffer_counter = 0;
				isPostWallJumpDelayBuffer = false;
			}
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

 		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
 		//_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * speed, Time.deltaTime * smoothedMovementFactor );
		 _velocity.x = Mathf.SmoothDamp(_velocity.x, normalizedHorizontalSpeed * speed, ref xVel, Time.deltaTime * smoothedMovementFactor);
		//_velocity.x = Mathf.MoveTowards(_velocity.x, normalizedHorizontalSpeed * speed, 1f );

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