using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDespawn : MonoBehaviour {

	public float destroyTime;

	void Start()
	{
		StartCoroutine(TimedDestroy ());
	}

	IEnumerator TimedDestroy()
	{
		yield return new WaitForSeconds (destroyTime);
		Destroy (gameObject);
	}
}
