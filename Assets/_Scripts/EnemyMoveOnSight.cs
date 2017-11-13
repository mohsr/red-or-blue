using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveOnSight : MonoBehaviour {

	public float viewDistance = 4.0f;
	public bool stomped;
	public float speed = 2.75f;
	public float fallSpeedMultiplier = 1.1f;
	public LayerMask enemyMask;

	public Vector3 visionOffset = new Vector2 (0.0f, 0.0f);

	private bool falling;
	private Rigidbody2D rb2d;
	private Animator anim;

	private void Start()
	{
		stomped = false;
		falling = false;
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	private void FixedUpdate()
	{
		if (stomped) {
			Stomp ();
		} else if (falling) {
			Fall ();
		} else {
			MoveToPlayer ();
		}
	}

	private void Stomp()
	{
		Vector3 newScale = transform.localScale;
		newScale.y /= 2;
		transform.localScale = newScale;

		Vector3 newPosition = transform.position;
		newPosition.y -= newScale.y;
		transform.position = newPosition;

		stomped = false;

		Physics2D.IgnoreCollision (gameObject.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag ("Ground").GetComponent<Collider2D> ());

		falling = true;
		Vector2 newVelocity = rb2d.velocity;
		newVelocity.y = -1.0f;
		newVelocity.x = 0.0f;
	}

	private void Fall()
	{
		Vector2 newVelocity = rb2d.velocity;
		newVelocity.y *= fallSpeedMultiplier;
		rb2d.velocity = newVelocity;
	}

	private void MoveToPlayer()
	{
		int i;
		RaycastHit2D point;
		
		/* Cast a line to the left, then to the right. */
		for (i = -1; i < 2; i += 2) {
			/* Draw a line. */
			point = Physics2D.Linecast (transform.position + visionOffset,
				new Vector2 (transform.position.x + visionOffset.x + (i * viewDistance),
					transform.position.y + visionOffset.y),
				~(1 << LayerMask.NameToLayer ("Enemy")));
			
			/* Draw the line in scene viewer. */
			Debug.DrawLine (transform.position + visionOffset, new Vector2 (transform.position.x + visionOffset.x + (i * viewDistance),
				transform.position.y + visionOffset.y));
				
			/* If player found, move. */
			if (point.collider == null) {
				if (!anim.GetBool ("Idle")) {
					anim.SetBool ("Idle", true);
					anim.SetBool ("Walking", false);
				}
				continue;
			}
			if (point.collider.tag == "Player") {
				transform.position = Vector2.MoveTowards (transform.position, 
					point.collider.transform.position,
					speed * Time.deltaTime);
				if (transform.localScale.x != -i)
					transform.localScale = new Vector3 (-i, transform.localScale.y, transform.localScale.z);
				if (!anim.GetBool ("Walking")) {
					anim.SetBool ("Idle", false);
					anim.SetBool ("Walking", true);
				}
				return;
			}
		}
	}
}
