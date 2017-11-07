using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bounds : MonoBehaviour {

	private GameObject player;
	public FollowPlayer followplayer;

	void Start ()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		followplayer = FindObjectOfType(typeof(FollowPlayer)) as FollowPlayer; 
	}

	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Player") {
            //Die
            player.transform.position = new Vector2 (player.transform.position.x - 5.0f, 1.0f);
			followplayer.setCameraOnPlayer ();
		} else
        {
            Destroy(coll.gameObject);
        }
	}
}