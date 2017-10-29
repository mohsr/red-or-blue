using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jumpSpeed = 5.0f;
	public float colliderHeight = 0.30f;
	public Vector2 moveDirection = Vector2.zero;
	public Vector2 jumpDirection = Vector2.zero;
	[HideInInspector]
	public bool isGrounded = false;
	public float jumpBuffer = 0.15f;

	private Rigidbody2D rb2d;
	private CapsuleCollider2D cc2d;
	private float buffer_counter = 0;
	private bool isBufferedJump = false;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void OnCollisionStay2D(Collision2D coll) {
		if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Blue" || coll.gameObject.tag == "Red")
			isGrounded = true;
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Blue" || coll.gameObject.tag == "Red")
			isGrounded = false;
	}

	void FixedUpdate()
	{

		/* Check for horizontal movement */
		if (Input.GetAxis("Horizontal") != 0) {
			moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
		}

		if (isBufferedJump) {
			buffer_counter += Time.deltaTime;
			Debug.Log (buffer_counter);
			if (buffer_counter > jumpBuffer)
				isBufferedJump = false;
		}

		/* Check for jumping */
		if (((Input.GetButtonDown ("Jump")) || isBufferedJump) && isGrounded) {
			isGrounded = false;
			isBufferedJump = false;
			rb2d.velocity = new Vector3(rb2d.velocity.x, jumpSpeed, rb2d.velocity.y);
		}

		if (Input.GetButtonDown ("Jump") && !isGrounded) {
			isBufferedJump = true;
			buffer_counter = 0;
		}
		/* Execute movement */
		transform.Translate (moveDirection.normalized * speed * Time.deltaTime);
	}

}