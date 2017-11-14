using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitchReceiver : MonoBehaviour {

	[HideInInspector]
	public bool on = true;
	public string offAnimParameter = "Embarrased";

	private Animator anim;
	private Collider2D col;
	private EnemyMoveOnSight mov;

	void Awake()
	{
		anim = GetComponent<Animator> ();
		mov = GetComponent<EnemyMoveOnSight> ();
	}

	public void SwitchEnemy(bool value)
	{
		on = value;
		anim.SetBool (offAnimParameter, !value);
		if (!value) {
			anim.SetBool ("Idle", false);
			anim.SetBool ("Walking", false);
		}
		mov.enabled = value;
	}
}
