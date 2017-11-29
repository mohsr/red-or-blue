using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

	public GameObject spawnedObject;
	public float timeBetweenSpawns = 1.0f;
	public Vector3 spawnOffset;
	public bool alterGravity = false;
	public float newGravity = 0.25f;
	public Vector2 directionAndForce = Vector2.down;

	void Start()
	{
		StartCoroutine (TimedSpawner());
	}

	IEnumerator TimedSpawner()
	{
		while (gameObject.activeSelf) {
			yield return new WaitForSeconds (timeBetweenSpawns);
			Spawn ();
		}
	}

	void Spawn()
	{
		if (spawnedObject == null) {
			return;
		}

		GameObject spawned = Instantiate (spawnedObject);
		Rigidbody2D platerb2d = spawned.GetComponent<Rigidbody2D> ();
		spawned.transform.position = transform.position + spawnOffset;

		/* Change gravity scale and add throw force. */
		if (platerb2d != null) {
			if (alterGravity) {
				platerb2d.gravityScale = newGravity;
			}
			platerb2d.AddForce (directionAndForce);
		}
	}
}
