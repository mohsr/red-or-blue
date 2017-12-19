using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showTutorial : MonoBehaviour {

	public float ztransform = -5.5f;

	// Use this for initialization
	void Start () {
		transform.GetChild (0).gameObject.SetActive (false);
		transform.GetChild (1).gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{	
		if (other.tag == "Player") {
			transform.GetChild (0).gameObject.SetActive (true);
			transform.GetChild (1).gameObject.SetActive (true);
			StartCoroutine (playvid());
		}
	}

	IEnumerator playvid() {
		transform.GetChild (0).gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponentInChildren<UnityEngine.Video.VideoPlayer> ().Play ();
		yield return new WaitForSeconds(.45f);
		transform.GetChild (0).gameObject.GetComponent<MeshRenderer> ().enabled = true;
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
			transform.GetChild (0).gameObject.SetActive (false);
			transform.GetChild (1).gameObject.SetActive (false);
		}
	}
}
