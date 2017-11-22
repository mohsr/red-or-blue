﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

public class Switch : MonoBehaviour {

	public string switcher = "e";
	public Color32 redBackground = new Color32(180, 114, 114, 1);
	public Color32 blueBackground = new Color32(129, 198, 221, 1);
	public float backgroundFadeSpeed = 0.25f;
	[HideInInspector]
	public bool red = false;
	[HideInInspector]
	public bool blue = true;

	private const string R = "Red";
	private const string B = "Blue";
	private Color32 nextColor;
	private Camera mainCamera;
	private PlayerController _playerController;

	void Start()
	{
		mainCamera = Camera.main;
		SwitchColorsHelper (R, B);
		mainCamera.backgroundColor = blueBackground;
		nextColor = blueBackground;
		_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	public void Update()
	{
		GameObject pl = GameObject.FindGameObjectWithTag ("Player");

		if (pl == null)
			return;
		
		if (_playerController == null) {
			_playerController = pl.GetComponent<PlayerController>();
			return;
		}
		if (Input.GetKeyDown(switcher) && _playerController.allowSwitch) {
			_playerController.allowSwitch = false;
			SwitchColors ();
		}
		/* Lerping background color. */
		mainCamera.backgroundColor = Color.Lerp (mainCamera.backgroundColor,
			                                     nextColor,
			                                     Mathf.PingPong(Time.time, 1 * backgroundFadeSpeed));
	}

	public void SwitchColors()
	{
		if (red) {
			SwitchColorsHelper (B, R);
			nextColor = redBackground;
		} else if (blue) {
			SwitchColorsHelper (R, B);
			nextColor = blueBackground;
		}
	}

	private void SwitchColorsHelper(string new_on, string new_off)
	{
		EnemySwitchReceiver currEnemy;

		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_on + "Hor"))
			ChangeState (i, true);
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_on + "Vert"))
			ChangeState (i, true);
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_on + "Enemy")) {
			currEnemy = i.GetComponent<EnemySwitchReceiver> ();
			if (currEnemy != null)
				currEnemy.SwitchEnemy (true);
		}
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_off + "Hor"))
			ChangeState (i, false);
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_off + "Vert"))
			ChangeState (i, false);
		foreach (GameObject i in GameObject.FindGameObjectsWithTag(new_off + "Enemy")) {
			currEnemy = i.GetComponent<EnemySwitchReceiver> ();
			if (currEnemy != null)
				currEnemy.SwitchEnemy (false);
		}

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

		int numChildren = obj.transform.childCount;
		for (int i = 0; i < numChildren; i++) {
			obj.transform.GetChild (i).gameObject.SetActive (state);
		}
	}
}
