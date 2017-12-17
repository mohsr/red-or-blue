using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTutorial : MonoBehaviour {

	public float ztransform = -5.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{	
		if (other.tag == "Player") {
			StartCoroutine (playvid());
		}
	}

	IEnumerator playvid() {
		gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer> ().Play ();
		yield return new WaitForSeconds(.4f);
		Vector3 loc = gameObject.transform.position;
		loc.z = 1;
		Quaternion rot = gameObject.transform.rotation;
		gameObject.transform.SetPositionAndRotation (loc, rot);
	}

	private void OnTriggerExit2D(Collider2D other)
	{	
		if (other.tag == "Player") {
			Vector3 loc = gameObject.transform.position;
			loc.z = 6;
			Quaternion rot = gameObject.transform.rotation;
			gameObject.transform.SetPositionAndRotation (loc, rot);
			gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer> ().Stop ();
		}
	}
}
