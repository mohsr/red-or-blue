using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMusic : MonoBehaviour {

	public float loopToTime = 0.5f;

	private AudioSource aud;

	void Start()
	{
		aud = GetComponent<AudioSource> ();
		aud.Play ();
	}

	void Update()
	{
		if (aud.time == aud.clip.length) {
			aud.Play ();
			aud.time = loopToTime;
		}
	}
}
