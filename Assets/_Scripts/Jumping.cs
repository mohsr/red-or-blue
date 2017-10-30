using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour {

	public float minJumpVelocity = 4f;
	public float maxJumpVelocity = 6.5f;
	//	[HideInInspector]
	public bool isGrounded = false;
	public float jumpBuffer = 0.15f;
	public float fallingGravityScale = 1.65f;

	private Rigidbody2D rb2d;
	private float buffer_counter = 0;
	private float realGravity;
	private bool isBufferedJump = false;
	private int higherJumpCounter = 0;
    public float speed = 5.0f;

    void Start()
	{
		rb2d = GetComponentInParent<Rigidbody2D> ();
		realGravity = rb2d.gravityScale;
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

	private void FixedUpdate()
	{
		/* Check for buffered jumping. */
		if (isBufferedJump) {
			buffer_counter += Time.deltaTime;
			if (buffer_counter > jumpBuffer)
				isBufferedJump = false;
		}

		if (((Input.GetButtonDown ("Jump")) || isBufferedJump) && isGrounded) {
			isGrounded = false;
			isBufferedJump = false;
            rb2d.transform.Translate(new Vector2(0, 0.05f));
            rb2d.velocity = new Vector2(rb2d.velocity.x, maxJumpVelocity);
		}

		if (Input.GetButtonUp ("Jump")) {
			if (rb2d.velocity.y > minJumpVelocity) {
				rb2d.velocity = new Vector2(rb2d.velocity.x, minJumpVelocity);
			}
		}

		if (Input.GetButtonDown ("Jump") && !isGrounded) {
			isBufferedJump = true;
			buffer_counter = 0;
		}

		/* Increase gravity slightly when falling. */
		if (rb2d.velocity.y < 0)
			rb2d.gravityScale = fallingGravityScale;
		else
			rb2d.gravityScale = realGravity;
	}
}
