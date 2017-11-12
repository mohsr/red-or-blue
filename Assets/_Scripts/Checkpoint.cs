using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public bool OnlySetOnce = false;
	[HideInInspector]
	public bool used = false;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (OnlySetOnce && used)
			return;
		
		if (other.tag == "Player") {
			other.GetComponent<PlayerDie> ().respawnLocation = transform.position;
			used = true;
		}
	}
}
