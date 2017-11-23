using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	private void OnTriggerEnter2D(Collider2D other)
	{	
		if (other.tag == "Player") {
			SceneManager.LoadScene("Cafeteria");
		}
	}
}
