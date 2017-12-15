using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Transparent : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// store a reference to the SpriteRenderer on the current GameObject
		SpriteRenderer [] invisibles = gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer s in invisibles) 
		{
			Color col = s.color;
			col.a = 0.5f;
			s.color = col;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
