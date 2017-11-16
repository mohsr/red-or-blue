using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtAndDestroyOnTouch : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		PlayerDie pd;

		if (other.tag == "Player") {
			pd = other.GetComponent<PlayerDie> ();
			if (pd != null)
				pd.Hurt ();
		}

		if (other.tag == "Player"   ||
			other.tag == "Ground"   ||
			other.tag == "RedHor"   ||
			other.tag == "BlueHor"  ||
			other.tag == "RedVert"  ||
			other.tag == "BlueVert" ||
			other.tag == "Red")
			Destroy (gameObject);
	}
}
