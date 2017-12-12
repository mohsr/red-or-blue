using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadFirstLevel : MonoBehaviour {

	public string levelName = "Bathroom";

	public void LoadLevelOne()
	{
		SceneManager.LoadScene(levelName);
	}
}
