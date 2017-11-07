using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public float speed = 4.5f;
	public Vector2 maxDistanceFromPlayer = new Vector2(1.0f, 2.0f);
	public float speedIncreaseUnit = 2f;
	public Vector3 offset = new Vector3(0, 0, 0);

	public bool freezeX = false;
	public bool freezeY = false;
	public bool freezeZ = true;

	private GameObject player;
	private Vector3 start;
	private float respawnTime;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		start = transform.position;
		if (player != null)
			respawnTime = player.GetComponent<PlayerDie> ().respawnTime;
	}

	void Update()
	{
		if (player == null) {
			Start ();
			return;
		}

		/* Get distance between camera and player and calculate speed multiplier. */
		float dist = Vector2.Distance (transform.position, player.transform.position);
		boxXCoord (dist);

		/* Freeze positions. */
		Vector3 frozen = new Vector3(transform.position.x,
			                         transform.position.y,
			                         transform.position.z);
		if (freezeX)
			frozen = new Vector3 (start.x, frozen.y, frozen.z);
		if (freezeY)
			frozen = new Vector3 (frozen.x, start.y, frozen.z);
		if (freezeZ)
			frozen = new Vector3 (frozen.x, frozen.y, start.z);
		transform.position = frozen;
    }

	public void setCameraOnPlayer()
	{
		transform.position = new Vector3(player.transform.position.x,
										 player.transform.position.y,
			                             transform.position.z);
	}

	private void centerFromFar()
	{
		float tempSpeed = player.GetComponent<PlayerController> ().speed * 3;

		Vector2 moving = Vector2.MoveTowards (transform.position,
			                                  player.transform.position,
			                                  tempSpeed * Time.deltaTime);

		transform.position = new Vector3 (moving.x, moving.y, start.z);
	}

	private void boxXCoord(float dist)
	{
		float tempSpeed = speed;
		if (Mathf.Abs (transform.position.x - player.transform.position.x) > maxDistanceFromPlayer.x) {
			tempSpeed = player.GetComponent<PlayerController> ().speed;
			tempSpeed *= (dist / speedIncreaseUnit);
		}

		Vector2 moving = Vector2.MoveTowards (transform.position,
			                                  player.transform.position + offset,
			                                  tempSpeed * Time.deltaTime);

		transform.position = new Vector3 (moving.x, moving.y, start.z);
	}
}
