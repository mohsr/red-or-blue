using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public string switcher = "e";
	[HideInInspector]
	public bool red = false;
	[HideInInspector]
	public bool blue = true;

	private const string R = "Red";
	private const string B = "Blue";
	private Camera mainCamera;

	void Start()
	{
		mainCamera = Camera.main;
		SwitchColorsHelper (R, B);
	}

	public void Update()
	{
		if (Input.GetKeyDown(switcher))
			SwitchColors ();
	}

	public void SwitchColors()
	{
		if (red) {
			SwitchColorsHelper (B, R);
			mainCamera.backgroundColor = new Color32(255, 40, 33, 1);
		} else if (blue) {
			SwitchColorsHelper (R, B);
			mainCamera.backgroundColor = new Color32(8, 182, 239, 1);
		}
	}

	private void SwitchColorsHelper(string new_on, string new_off)
	{
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_on))
			ChangeState (i, true);
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_off))
			ChangeState (i, false);
		red = !red;
		blue = !blue;
	}

	private void ChangeState(GameObject obj, bool state)
	{
		SpriteRenderer spr = obj.GetComponent<SpriteRenderer> ();
		Collider2D col = obj.GetComponent<Collider2D> ();

		if (spr != null)
			spr.enabled = state;
		if (col != null)
			col.enabled = state;
	}
}
