using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

	public int orderInLevel = 1;
	public bool OnlySetOnce = false;
	[HideInInspector]
	public bool used = false;

	private GameObject player = null;
	private PlayerController pc;

	void Update()
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
			if (player != null) {
				pc = player.GetComponent<PlayerController> ();
			} else {
				return;
			}
		}

		if (used) {
			if (pc.checkPointNum > orderInLevel) {
				Destroy (gameObject);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (OnlySetOnce && used)
			return;

		if (other.tag == "Player") {
			if (pc.checkPointNum >= orderInLevel) {
				return;
			} else {
				other.GetComponent<PlayerDie> ().respawnLocation = transform.position;
				pc.checkPointNum = orderInLevel;
				used = true;
			}
		}
	}
}
