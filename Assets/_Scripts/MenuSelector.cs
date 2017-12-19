using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSelector : MonoBehaviour {
	/*
	 * Note: This script is hard coded and is not great. I would
	 * make it great, but I don't have a lot of time and have other
	 * work to do.
	 */

	public Vector3 selectorOffsetToExit;
	public string switcher = "switch";
	public string firstLevelName = "Bathroom";
	public string selectorName = "SelectorArrow";

	private GameObject selector;
	private bool isPlay = true;
	private Vector3 playLocation;
	private Vector3 exitLocation;

	void Start()
	{
		selector = GameObject.Find (selectorName);
		isPlay = true;
		playLocation = selector.transform.position;
		exitLocation = selector.transform.position + selectorOffsetToExit;
		Debug.Log (transform.position);
	}

	void Update()
	{
		float push = Input.GetAxisRaw ("Vertical");

		if (push > 0) {
			isPlay = true;
			selector.transform.position = playLocation;
		} else if (push < 0) {
			isPlay = false;
			selector.transform.position = exitLocation;
		}

		if (Input.GetButtonDown (switcher)) {
			if (isPlay) {
				SceneManager.LoadScene (firstLevelName);
			} else {
				Application.Quit ();
			}
		}
	}
}
