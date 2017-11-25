using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	public Vector3[] movementPath;
	public float speed = 2.0f;
	public float waitAtEnds = 1.0f;
	public float waitTime = 2.0f;
	public bool oneWay = false;

	private Rigidbody2D rb2d;

	private Vector3 initLoc;
	private Vector3 currLoc;
	private int currSpot;
	private int nextSpot;
	private bool ascending;
	private bool fin = false;
	private bool moving = true;

	void Start()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		ascending = true;

		movementPath [0] = transform.position;
		initLoc = transform.position;
	}

	void Update()
	{
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

		if (ascending) {
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

		if (currLoc == (initLoc + movementPath [nextSpot])) {
			currSpot = nextSpot;
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
		yield return new WaitForSeconds (waitTime);
		moving = true;
	}
}
