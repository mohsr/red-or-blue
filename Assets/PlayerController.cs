using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5.0f;
	public float jump = 5.0f;
	public float colliderHeight = 0.30f;
	[HideInInspector]
	public bool grounded = true;

	private Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		transform.rotation = new Quaternion (0, 0, 0, 0);

		/* Handles horizontal movement. */
		float moveH = Input.GetAxis ("Horizontal");
		transform.position += new Vector3(moveH * speed * Time.deltaTime, 0, 0);

		/* Handles jumping. */
		RaycastHit2D jump_check = Physics2D.Raycast(transform.position,
			new Vector2(0, -1),
			colliderHeight);

		grounded = (jump_check.collider == null ||
			jump_check.collider.gameObject.tag == "Enemy");

		if (Input.GetKeyDown ("space") && grounded) {
			rb2d.AddForce (new Vector2 (0, jump * 45.0f));
		}
	}

}