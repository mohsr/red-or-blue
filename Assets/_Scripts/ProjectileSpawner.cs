using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour {

	public GameObject spawnedObject;
	public float timeBetweenSpawns = 1.0f;
	public Vector3 spawnOffset;

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
		GameObject spawned = Instantiate (spawnedObject);
		spawned.transform.position = transform.position + spawnOffset;
	}
}
