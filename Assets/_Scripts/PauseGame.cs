using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    public Transform pauseMenu;

	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.gameObject.activeInHierarchy == false)
            {
                pauseMenu.gameObject.SetActive(true);
            } else
            {
                pauseMenu.gameObject.SetActive(false);
            }
        }
	}
}
