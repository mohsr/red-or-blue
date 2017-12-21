using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameRate : MonoBehaviour {

	void Start () {
		Application.targetFrameRate = 60;
	}
}
