using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour {

	public int numberOfTiles = 3;
	private Transform cameraTransform;
	private Vector3 prevCameraLocation;
	private MeshRenderer [] backgroundMesh = new MeshRenderer[3];
	public float scrollSpeed = 20;
	private int startToMoveTile = 0;
	private GameObject[] toBeScrolled;
	private GameObject currentTile;
	private float currentTileMinX;
	private float currentTileMaxX;

	// Use this for initialization
	void Start () {
		toBeScrolled = GameObject.FindGameObjectsWithTag ("ScrollingBackground");
		currentTile = toBeScrolled[1];
		currentTileMinX = transform.position.x + currentTile.GetComponent<MeshRenderer>().bounds.min.x;
		currentTileMaxX = transform.position.x + currentTile.GetComponent<MeshRenderer>().bounds.max.x;

		for (int k = 0; k < toBeScrolled.Length; k++) {
			backgroundMesh [k] = toBeScrolled [k].GetComponent<MeshRenderer> ();
		} 
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		prevCameraLocation = cameraTransform.position;
	}

	public void Update () {
		if (cameraTransform == null) {
			cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		}

		float displacement = cameraTransform.position.x - prevCameraLocation.x;
		prevCameraLocation = cameraTransform.position;
		for (int k = 0; k < backgroundMesh.Length; k++) {
			if (backgroundMesh [k].CompareTag ("ScrollingBackground")) {
				Material mat = backgroundMesh [k].material;
				mat.SetTextureOffset ("_MainTex", new Vector2 (mat.GetTextureOffset ("_MainTex").x + displacement / scrollSpeed, mat.GetTextureOffset ("_MainTex").y));
			}
		}

//		if (playerTransform.position.x >= currentTileMaxX) {
//			Debug.Log ("HELLO");
//			startToMoveTile = (startToMoveTile + 1) % numberOfTiles;
//			float newPos = toBeScrolled [startToMoveTile ].transform.position.x + toBeScrolled [startToMoveTile ].transform.localScale.x;
//			toBeScrolled [startToMoveTile].transform.position.Set (newPos, toBeScrolled [startToMoveTile].transform.position.y, toBeScrolled [startToMoveTile].transform.position.z);
//		}
	}
}
