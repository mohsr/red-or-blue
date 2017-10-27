using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed = 5;

	private Rigidbody2D rb2d;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		/* Handles horizontal movement. */
		float moveH = Input.GetAxis ("Horizontal");
		transform.position += new Vector3(moveH * speed * Time.deltaTime, 0, 0);

		/* Handles jumping. */
		if (Input.GetKeyDown("space"))
			rb2d.AddForce
	}

}
