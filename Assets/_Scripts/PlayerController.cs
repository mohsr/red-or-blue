using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public Vector2 moveDirection = Vector2.zero;

	private float moveDir = 0.0f;
	private Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
//		moveDir = Input.GetAxis ("Horizontal");
//		/* Check for horizontal movement */
//		if (moveDir != 0) {
//			rb2d.velocity = new Vector2 (speed * moveDir, rb2d.velocity.y);
//		} else {
//			rb2d.velocity = new Vector2 (0, rb2d.velocity.y);
//		}

		if (Input.GetAxis ("Horizontal") != 0) {
			moveDirection = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
			transform.Translate (moveDirection.normalized * speed * Time.deltaTime);
		}

		/* Execute movement. */

	}
}