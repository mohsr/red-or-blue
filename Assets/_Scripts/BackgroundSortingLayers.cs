using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSortingLayers : MonoBehaviour {

	void Start()
	{
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).GetComponent<MeshRenderer> ().sortingLayerName = "Background";
			for (int j = 0; j < transform.GetChild (i).transform.childCount; j++) {
				transform.GetChild (i).GetChild (j).GetComponent<MeshRenderer> ().sortingLayerName = "Background";
			}
		}
	}
}
