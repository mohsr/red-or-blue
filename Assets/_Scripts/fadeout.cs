using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

	public Texture2D fadeTexture;
	public float waitforfade = 5f;
	public bool fadein = false;
	private float seconds = 0f;

	[Range(0.1f,1f)]
	public float fadespeed;
	public int drawDepth = -1000;

	private float alpha = 0f;
	private float fadeDir = -1f;

	// Use this for initialization
	void Start () {
		if (fadein)
			alpha = 1f;
	}

	void Update() {
		seconds += Time.deltaTime;
	}


	void OnGUI() {
		if (seconds < waitforfade)
			return;

		if (fadein)
			alpha += fadeDir * fadespeed * Time.deltaTime;
		else 
			alpha -= fadeDir * fadespeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		Color newColor = GUI.color; 
		newColor.a = alpha;

		GUI.color = newColor;

		GUI.depth = drawDepth;

		GUI.DrawTexture( new Rect(0,0, Screen.width, Screen.height), fadeTexture);

	}
}