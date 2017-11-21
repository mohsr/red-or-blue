using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnOnCollision : MonoBehaviour {

	public bool notOnPlayer = true;

	void OnCollisionEnter2D(Collision2D other)
	{
		if (notOnPlayer && other.transform.tag == "Player") {
			return;
		}
		Destroy (gameObject);
	}
}
