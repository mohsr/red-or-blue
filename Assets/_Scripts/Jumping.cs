using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : MonoBehaviour {

	public float jumpSpeed = 4.5f;
	public float higherJumpTime = 0.07f;
	public float higherJumpSpeed = 0.7f;
	public int maxHigherJumps = 1;
	[HideInInspector]
	public bool isGrounded = false;
	public float jumpBuffer = 0.15f;
	public float fallingGravityScale = 1.1f;

	private Rigidbody2D rb2d;
	private float buffer_counter = 0;
	private float realGravity;
	private bool isBufferedJump = false;
	private int higherJumpCounter = 0;

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
			Debug.Log (buffer_counter);
			if (buffer_counter > jumpBuffer)
				isBufferedJump = false;
		}

		/* Check for jumping */
		if (((Input.GetButtonDown ("Jump")) || isBufferedJump) && isGrounded) {
			isGrounded = false;
			isBufferedJump = false;
			rb2d.velocity = new Vector2(rb2d.velocity.x, jumpSpeed);
			StartCoroutine (WaitForAddForce ());
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

	private IEnumerator WaitForAddForce()
	{
		while (higherJumpCounter < maxHigherJumps) {
			yield return new WaitForSeconds (higherJumpTime);
			if (Input.GetButton ("Jump")) {
				rb2d.velocity += new Vector2 (0, higherJumpSpeed);
				higherJumpCounter++;
			} else {
				break;
			}
		}
		higherJumpCounter = 0;
	}
}
