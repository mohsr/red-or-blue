using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerOnTouch : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
			other.GetComponent<PlayerDie> ().Die ();
	}
}
