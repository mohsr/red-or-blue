using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayerWith : MonoBehaviour {

	private Transform otherInitParent;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;

		otherInitParent = other.transform.parent;
		
		if (other.GetComponent<Rigidbody2D> () != null)
			other.transform.parent = gameObject.transform;
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;
		
		other.transform.parent = otherInitParent;
	}
}
