using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	public int numberOfTiles = 3;
	private Vector3 playerLocation; 
	private Transform playerTransform;
	private MeshRenderer [] backgroundMesh = new MeshRenderer[3];
	public float scrollSpeed = 20;
	private int startToMoveTile = 0;
	private GameObject[] toBeScrolled;

	// Use this for initialization
	void Start () {
		toBeScrolled = GameObject.FindGameObjectsWithTag ("ScrollingBackground");

		for (int k = 0; k < toBeScrolled.Length; k++) {
			backgroundMesh [k] = toBeScrolled [k].GetComponent<MeshRenderer> ();
		} 

		playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		playerLocation = playerTransform.position;
	}

	public void Update () {
		if (playerTransform == null) {
			playerTransform = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
		}

		float displacement = playerTransform.position.x - playerLocation.x;
		playerLocation = playerTransform.position;
		for (int k = 0; k < backgroundMesh.Length; k++) {
			if (backgroundMesh [k].CompareTag ("ScrollingBackground")) {
				Material mat = backgroundMesh [k].material;
				mat.SetTextureOffset ("_MainTex", new Vector2 (mat.GetTextureOffset ("_MainTex").x + displacement / scrollSpeed, mat.GetTextureOffset ("_MainTex").y));
			}
		}

//		if (playerTransform.position.x >= toBeScrolled [startToMoveTile].transform.position.x) {
//			Debug.Log ("HELLO");
//			startToMoveTile = (startToMoveTile + 1) % numberOfTiles;
//			float newPos = toBeScrolled [startToMoveTile ].transform.position.x + toBeScrolled [startToMoveTile ].transform.localScale.x;
//			toBeScrolled [startToMoveTile].transform.position.Set (newPos, toBeScrolled [startToMoveTile].transform.position.y, toBeScrolled [startToMoveTile].transform.position.z);
//		}
//		Debug.Log ("0: " + backgroundMesh [0]);
//		Debug.Log ("1: " + backgroundMesh [1]);
//		Debug.Log ("2: " + backgroundMesh [2]);
	}
}
