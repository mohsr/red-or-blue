using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitThenLoadScene : MonoBehaviour {

	public float waitTime = 1.8f;
	public string levelName = "Bathroom";

	void Start()
	{
		StartCoroutine (WaitThenLoad ());
	}

	public IEnumerator WaitThenLoad()
	{
		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadScene (levelName);
	}
}
