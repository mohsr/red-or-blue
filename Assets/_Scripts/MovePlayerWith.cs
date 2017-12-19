using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWith : MonoBehaviour {

	[HideInInspector]
	public bool playerOn = false;

	private Transform otherInitParent;
	private float otherZ;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;

		otherInitParent = other.transform.parent;
		otherZ = other.transform.position.z;

		playerOn = true;
		
		if (other.GetComponent<Rigidbody2D> () != null) {
			other.transform.parent = gameObject.transform;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;
		
		other.transform.parent = otherInitParent;
		playerOn = false;
	}
}
