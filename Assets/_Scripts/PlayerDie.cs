using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour {

	public bool useCheckpoints = true;

	/* Variables to be passed to respawn coordinator. */
	public float respawnTime = 2.0f;
	public Vector3 respawnLocation = new Vector3 (0, 0, 0);
	
	public GameObject respawnCoordinator;
	public AudioClip deathSound;

	private GameObject rc;
	private RespawnCoordinator rc_comp;
    private bool dead = false;

	private Sprite inactiveHealthSprite;

	public void Hurt()
	{
		Die();
	}

	public void Die()
	{
        if (!dead)
        {
			if (inactiveHealthSprite != null) {
				GameObject.FindGameObjectWithTag ("UI_Heart0").GetComponent<Image> ().sprite = inactiveHealthSprite;
				GameObject.FindGameObjectWithTag ("UI_Heart1").GetComponent<Image> ().sprite = inactiveHealthSprite;
			}

			if (deathSound != null)
				AudioSource.PlayClipAtPoint (deathSound, transform.position);

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
		inactiveHealthSprite = GetComponent<PlayerController> ().inactiveHealthSprite;
	}

	private void PassVals()
	{
		rc_comp.respawnTime = respawnTime;
		rc_comp.respawnLocation = respawnLocation;
	}
}
