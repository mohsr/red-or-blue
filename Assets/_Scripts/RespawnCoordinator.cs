using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnCoordinator : MonoBehaviour {

	[HideInInspector]
	public float respawnTime = 2.0f;
	[HideInInspector]
	public Vector3 respawnLocation = new Vector3(0, 0, 0);
	public GameObject toRespawn;

	void Awake()
	{
		StartCoroutine(WaitToSpawnPlayer());
	}

	private IEnumerator WaitToSpawnPlayer()
	{
		yield return new WaitForSeconds(respawnTime);
		Instantiate (toRespawn, respawnLocation, Quaternion.identity);

		Destroy (gameObject);
	}
}
