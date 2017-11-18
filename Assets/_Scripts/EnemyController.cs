using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public float viewDistance = 4.0f;
    public bool stomped;
    public float speed = 2.75f;
    public float fallSpeedMultiplier = 1.1f;
    public LayerMask enemyMask;

    public Vector3 visionOffset = new Vector2(0.0f, 0.0f);

    private bool falling;
    private Animator anim;
    [HideInInspector]
    public bool embarrassed = false;
    Rigidbody2D myBody;

    // autoturn
    Transform myTrans;
    float myWidth;
    float horiRayLen = 0.05f;
    float directionChangeBuffer = 0.5f;
    bool isChangingDirection = false;
    float directionBufferCounter = 0;

    public bool moveOnSight = true;

    private void Start()
    {
        stomped = false;
        falling = false;
        int enemyLayer = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(enemyLayer, enemyLayer);
        anim = GetComponent<Animator>();
        myBody = GetComponent<Rigidbody2D>();
        myTrans = transform;
        myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    private void FixedUpdate()
    {
        if (stomped)
        {
            Stomp();
        }
        else if (falling)
        {
            Fall();
        }
        else if (!embarrassed)
        {
            if (moveOnSight)
            {
                MoveToPlayer();
            }
            else
            {
                AutoTurn();
            }
        } else
        {
            Vector2 myVelocity = myBody.velocity;
            myVelocity.x = 0;
            myBody.velocity = myVelocity;
        }
    }

    private void Stomp()
    {
        Debug.Log("Stomping");
        Vector3 newScale = transform.localScale;
        newScale.y /= 2;
        transform.localScale = newScale;

        Vector3 newPosition = transform.position;
        newPosition.y -= newScale.y;
        transform.position = newPosition;

        stomped = false;

        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider2D>());

        falling = true;
        Vector2 newVelocity = myBody.velocity;
        newVelocity.y = -1.0f;
        newVelocity.x = 0.0f;
    }

    private void Fall()
    {
        Vector2 newVelocity = myBody.velocity;
        newVelocity.y *= fallSpeedMultiplier;
        myBody.velocity = newVelocity;
    }

    private void MoveToPlayer()
    {
        int i;
        RaycastHit2D point;

        /* Cast a line to the left, then to the right. */
        for (i = -1; i < 2; i += 2)
        {
            /* Draw a line. */
            point = Physics2D.Linecast(transform.position + visionOffset,
                new Vector2(transform.position.x + visionOffset.x + (i * viewDistance),
                    transform.position.y + visionOffset.y),
                ~(1 << LayerMask.NameToLayer("Enemy")));

            /* Draw the line in scene viewer. */
            Debug.DrawLine(transform.position + visionOffset, new Vector2(transform.position.x + visionOffset.x + (i * viewDistance),
                transform.position.y + visionOffset.y));

            /* If player found, move. */
            if (point.collider == null)
            {
                if (!anim.GetBool("Idle"))
                {
                    anim.SetBool("Idle", true);
                    anim.SetBool("Walking", false);
                }
                continue;
            }
            if (point.collider.tag == "Player")
            {
                transform.position = Vector2.MoveTowards(transform.position,
                    point.collider.transform.position,
                    speed * Time.deltaTime);
                if (transform.localScale.x != -i)
                    transform.localScale = new Vector3(-i, transform.localScale.y, transform.localScale.z);
                if (!anim.GetBool("Walking"))
                {
                    anim.SetBool("Idle", false);
                    anim.SetBool("Walking", true);
                }
                return;
            }
        }
    }

    private void AutoTurn()
    {
        if (!anim.GetBool("Walking"))
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Walking", true);
        }
        // check to see if there's ground in front of us before moving forward
        Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth / 2;
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        // check if there are obstacles ahead
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * horiRayLen, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * horiRayLen);

        if (isChangingDirection)
        {
            directionBufferCounter += Time.deltaTime;
            if (directionBufferCounter >= directionChangeBuffer)
                isChangingDirection = false;
        }

        // if there's no ground or hit obstacle, turn around
        if ((!isGrounded || isBlocked) && !isChangingDirection)
        {
            Vector3 currRotation = myTrans.eulerAngles;
            currRotation.y += 180;
            myTrans.eulerAngles = currRotation;
            isChangingDirection = true;
            directionBufferCounter = 0;
        }

        // always move forward
        Vector2 myVelocity = myBody.velocity;
        myVelocity.x = -myTrans.right.x * speed;
        myBody.velocity = myVelocity;
    }
}
