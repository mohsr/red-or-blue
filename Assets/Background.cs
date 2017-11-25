using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Background : MonoBehaviour {

	public int numberOfTiles = 3;
	private Transform cameraTransform;
	private Vector3 prevCameraLocation;
	private MeshRenderer [] backgroundMesh = new MeshRenderer[3];
	public float scrollSpeed = 20;
	private GameObject[] toBeScrolled;
	private GameObject currentTile;
	private float currentTileMinX;
	private float currentTileMaxX;
	private float tileWidth;

	// Use this for initialization
	void Start () {

		toBeScrolled = GameObject.FindGameObjectsWithTag("ScrollingBackground").OrderBy(x => x.transform.position.x).ToArray();
		GetCurrentTile();
		tileWidth = currentTile.GetComponent<MeshRenderer>().bounds.size.x;
		cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		prevCameraLocation = cameraTransform.position;
	}

	public void Update () {

		if (cameraTransform == null) {
			cameraTransform = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Transform> ();
		}

		// If the player is moving to the right of the middle tile, grab the left tile and put it on the right
		if (cameraTransform.position.x > currentTileMaxX) {
			toBeScrolled[0].transform.position = toBeScrolled[0].transform.position + new Vector3(tileWidth * 3,0f,0f);
			Swap(toBeScrolled, 0, 1);
			Swap(toBeScrolled, 1, 2);
			GetCurrentTile();
		}	// Else if the player is moving the left of the middle tile, grab the right tile and put it on the left
		else if (cameraTransform.position.x < currentTileMinX) {
			toBeScrolled[2].transform.position = toBeScrolled[2].transform.position + new Vector3(- tileWidth * 3,0f,0f);
			Swap(toBeScrolled, 0, 1);
			Swap(toBeScrolled, 0, 2);
			GetCurrentTile();
		}

		float displacement = cameraTransform.position.x - prevCameraLocation.x;
		prevCameraLocation = cameraTransform.position;
		foreach (MeshRenderer mesh in backgroundMesh) {
			Material mat = mesh.material;
			mat.SetTextureOffset ("_MainTex", new Vector2 (mat.GetTextureOffset ("_MainTex").x + displacement / scrollSpeed, mat.GetTextureOffset ("_MainTex").y));
		}

	}

	static void Swap(GameObject[] array, int position1, int position2) {      
		GameObject temp = array[position1]; // Copy the first position's element
		array[position1] = array[position2]; // Assign to the second element
		array[position2] = temp; // Assign to the first element
	}

	void GetCurrentTile() {
		currentTile = toBeScrolled[1];
		currentTileMinX = currentTile.GetComponent<MeshRenderer>().bounds.min.x;
		currentTileMaxX = currentTile.GetComponent<MeshRenderer>().bounds.max.x;
		for (int k = 0; k < toBeScrolled.Length; k++) {
			backgroundMesh [k] = toBeScrolled [k].GetComponent<MeshRenderer> ();
		} 
	}
}
