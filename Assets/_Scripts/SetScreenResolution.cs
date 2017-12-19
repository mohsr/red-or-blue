using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetScreenResolution : MonoBehaviour {

	public int width = 1920;
	public int height = 1080;
	public bool fullscreen = false;
	public int refreshRate = 60;

	void Start () {
		Screen.SetResolution (width, height, fullscreen, refreshRate);
	}
}
