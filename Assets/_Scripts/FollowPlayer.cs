using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public bool followPlayer = true;
	public float speed = 2.0f;
	public Vector3 offset = new Vector3(0, 0, 0);

	private GameObject player;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");
        offset = transform.position - player.transform.position;
	}

	void Update()
	{
		if (player == null)
			Start ();

		if (followPlayer == false)
			return;

        transform.position = player.transform.position + offset;

        /*transform.position = Vector3.MoveTowards(transform.position,
                                                 new Vector3(player.transform.position.x,
                                                             player.transform.position.y,
                                                             transform.position.z) + offset,
                                                 speed * Time.deltaTime);*/
    }

	public void setCameraOnPlayer()
	{
		transform.position = new Vector3(player.transform.position.x,
										 player.transform.position.y,
										transform.position.z) + offset;

	}
}
