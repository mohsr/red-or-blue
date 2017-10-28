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

	private Rigidbody2D rb2d;
	private CapsuleCollider2D cc2d;

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
		/* Check for jumping */
		if (Input.GetButtonDown ("Jump") && isGrounded) {
			isGrounded = false;
			rb2d.velocity = new Vector3(rb2d.velocity.x, jumpSpeed, rb2d.velocity.y);
		}
		/* Execute movement */
		transform.Translate (moveDirection.normalized * speed * Time.deltaTime);
	}

}