using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Vector3[] movementPath;
	public float speed = 2.0f;
	public float waitAtEndsTime = 2.0f;
	public bool oneWay = false;
	public bool loop = false;
	public bool startOnPlayerTouch = false;

	private Vector3 initLoc;
	private Vector3 currLoc;
	private int currSpot = 0;
	private int nextSpot;
	private bool ascending;
	private bool fin = false;
	private bool moving = true;
	private bool playerOn = false;

	void Start()
	{
		LineRenderer lr;

//		transform.position = new Vector3 (transform.position.x, transform.position.y, 15);

		ascending = true;
		if (movementPath.Length == 0)
			return;
		movementPath [0] = new Vector3 (0, 0, 0);
		initLoc = transform.position;

		lr = GetComponent<LineRenderer> ();
		if (lr != null) {
			lr.loop = loop;

			lr.positionCount = movementPath.Length;
			for (int i = 0; i < movementPath.Length; i++) {
				lr.SetPosition(i, new Vector3(movementPath[i].x + transform.position.x, movementPath[i].y + transform.position.y, -1));
//				lr.SetPosition (i, movementPath [i] + transform.position);
			}
		}
	}

	void Update()
	{
		currLoc = transform.position;

//		if (startOnPlayerTouch) {
//			PlayerOnMovement ();
//			return;
//		}

		/* Handle edge cases. */
		if (movementPath.Length == 0)
			return;
		if (oneWay) {
			if (fin) {
				return;
			} else {
				if (transform.position == initLoc + movementPath [movementPath.Length - 1])
					fin = true;
			}
		}

		/* Check for loop or shifts in direction. */
		/* TODO: The following chunk of code isn't great and is not modular.
		 *       Fix later when I have more time.
		 */
		if (loop) {
			if (currSpot == movementPath.Length - 1) {
				nextSpot = 0;
			}
		} else if (ascending) {
			if (moving) {
				if (currSpot == movementPath.Length - 1) {
					SwitchDirection (false);
					StartCoroutine (WaitAtEnd ());
					return;
				}
				nextSpot = currSpot + 1;
			} else {
				return;
			}
		} else {
			if (moving) {
				if (currSpot == 0) {
					SwitchDirection (true);
					StartCoroutine (WaitAtEnd ());
					return;
				}
				nextSpot = currSpot - 1;
			} else {
				return;
			}
		}
			
		/* Check if next element in path has been traversed. */
		if (currLoc == (initLoc + movementPath [nextSpot])) {
			currSpot = nextSpot;
		} 

		/* Check for end-of-path for loops and moving to next loop path element. */
		if (loop) {
			if (nextSpot == 0) {
				if (currLoc == initLoc) {
					currSpot = nextSpot;
				}
			}
			if (nextSpot == currSpot) {
				nextSpot++;
				if (nextSpot == movementPath.Length)
					nextSpot = 0;
			}
		}

		/* Perform the move B) */
		transform.position = Vector2.MoveTowards (currLoc, initLoc + movementPath [nextSpot], speed * Time.deltaTime);
	}

	void PlayerOnMovement()
	{
		bool changed = false;
		bool ascend;

		/* Check if player is on. */
		playerOn = GetComponentInChildren<MovePlayerWith> ().playerOn;

		if (!playerOn) {
			ascend = true;

			/* Stop if at start. */
			if (currLoc == initLoc) {
				return;
			}
			/* Move back along the path. */
			if (currLoc == initLoc + movementPath [currSpot]) {
				nextSpot = currSpot - 1;
				changed = true;
			}
			if (currLoc == initLoc + movementPath [nextSpot]) {
				currSpot = nextSpot;
				nextSpot = currSpot - 1;
				changed = true;
			}

			if (!changed) {
				nextSpot = currSpot - 1;
				if (nextSpot < 0)
					nextSpot = 0;
			}
		} else {
			ascend = false;

			if (currLoc == initLoc + movementPath [movementPath.Length - 1]) {
				return;
			}
			if (currLoc == initLoc + movementPath [nextSpot]) {
				currSpot = nextSpot;
				nextSpot++;
				changed = true;
			}

			if (!changed) {
				nextSpot = currSpot + 1;
				if (nextSpot >= movementPath.Length)
					nextSpot = movementPath.Length - 1;
			}
		}

		transform.position = Vector2.MoveTowards (currLoc, initLoc + movementPath [nextSpot], speed * Time.deltaTime);
	}

	void SwitchDirection(bool newDirIsUp)
	{
		if (newDirIsUp) {
			ascending = true;
			nextSpot = 1;
		} else {
			ascending = false;
			nextSpot = movementPath.Length - 2;
		}
	}

	IEnumerator WaitAtEnd()
	{
		moving = false;
		yield return new WaitForSeconds (waitAtEndsTime);
		moving = true;
	}

	/* TODO: Lines currently move in scene view with gameObject.
	 *       Fix when I have more time.
	 */
	void OnDrawGizmos()
	{
		int i;
		Vector3 start, end;

		for (i = 0; i < movementPath.Length - 1; i++) {
			if (i == 0)
				start = transform.position;
			else
				start = transform.position + movementPath [i];
			end = transform.position + movementPath [i + 1];

			Gizmos.DrawLine (start, end);
		}
	}
}
