using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	public string level = "Bathroom";

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "Player")
			return;
		SceneManager.LoadScene (level);
	}
}
