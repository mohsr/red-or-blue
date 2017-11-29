using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Vector3[] movementPath;
	public float speed = 2.0f;
	public float waitAtEndsTime = 2.0f;
	public bool oneWay = false;
	public bool loop = false;

	private Vector3 initLoc;
	private Vector3 currLoc;
	private int currSpot = 0;
	private int nextSpot;
	private bool ascending;
	private bool fin = false;
	private bool moving = true;

	void Start()
	{
		ascending = true;

		movementPath [0] = new Vector3 (0, 0, 0);
		initLoc = transform.position;
	}

	void Update()
	{
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
			
		currLoc = transform.position;

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
