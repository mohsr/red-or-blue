using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour {

	public bool useCheckpoints = true;

	/* Variables to be passed to respawn coordinator. */
	public float respawnTime = 2.0f;
	public Vector3 respawnLocation = new Vector3 (0, 0, 0);
	
	public GameObject respawnCoordinator;

	private GameObject rc;
	private RespawnCoordinator rc_comp;
    private bool dead = false;

	public void Hurt()
	{
		Die();
	}

	public void Die()
	{
        if (!dead)
        {
            dead = true;
            rc = Instantiate(respawnCoordinator, new Vector3(0, 0, 0), Quaternion.identity);
            rc_comp = rc.GetComponent<RespawnCoordinator>();

            PassVals();

            Destroy(gameObject);
        }
	}

	private void Start()
	{
		respawnLocation = transform.position;
	}

	private void PassVals()
	{
		rc_comp.respawnTime = respawnTime;
		rc_comp.respawnLocation = respawnLocation;
	}
}
