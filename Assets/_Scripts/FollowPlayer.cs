using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public float speed = 4.5f;
	public Vector2 maxDistanceFromPlayer = new Vector2(0.5f, 2.0f);
//	public float maxXDistanceFromPlayer = 0.5f;
	public Vector3 offset = new Vector3(0, 0, 0);

	public bool freezeX = false;
	public bool freezeY = false;
	public bool freezeZ = true;

	private GameObject player;
	private Vector3 start;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
        offset = transform.position - player.transform.position;
		start = transform.position;

	}

	void Update()
	{
		if (player == null)
			Start ();

		boxXCoord ();

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

	private void boxXCoord()
	{
		float tempSpeed = speed;
		if (Mathf.Abs (transform.position.x - player.transform.position.x) > maxDistanceFromPlayer.x)
			tempSpeed = player.GetComponent<PlayerController> ().speed;

		transform.position = new Vector3 (Vector2.MoveTowards(transform.position,
			player.transform.position, tempSpeed * Time.deltaTime).x, player.transform.position.y, start.z);
		transform.position += offset;
	}
}
