﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector2 walkAmount;
    
    //public float wallLeft;
    //public float wallRight;
    //float walkingDirection = 1.0f;
    //public float walkSpeed = 1.0f;
    //public float moveAmount = 2.5f;
    public bool stomped;
    private bool fall;

    public float speed;
    public LayerMask enemyMask;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth;
    float horizontalRayLength = 0.02f;

    private void Start()
    {
        //wallLeft = transform.position.x - moveAmount;
        //wallRight = transform.position.x + moveAmount;
        stomped = false;
        myTrans = transform;
        myBody = GetComponent<Rigidbody2D>();
        myWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    private void FixedUpdate()
    {
        if (stomped)
        {
            Vector3 newScale = transform.localScale;
            newScale.y = newScale.y / 2;
            transform.localScale = newScale;
            Vector3 newPosition = transform.position;
            newPosition.y -= newScale.y;
            transform.position = newPosition;
            stomped = false;
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), GameObject.FindGameObjectWithTag("Ground").GetComponent<Collider2D>());
            fall = true;
            Vector2 myVelocity = myBody.velocity;
            myVelocity.y = -1.0f;
            myVelocity.x = 0;
            myBody.velocity = myVelocity;
        }
        else if (fall)
        {
            Vector2 myVelocity = myBody.velocity;
            myVelocity.y *= 1.1f;
            myBody.velocity = myVelocity;
        }
        else
        {
            // check to see if there's ground in front of us before moving forward
            Vector2 lineCastPos = myTrans.position - myTrans.right * myWidth / 2;
            bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            bool isBlocked = Physics2D.Linecast(myTrans.position, lineCastPos, enemyMask);
            Debug.DrawLine(myTrans.position, lineCastPos);

            // if there's no ground, turn around
            if (!isGrounded || isBlocked)
            {
                Vector3 currRotation = myTrans.eulerAngles;
                currRotation.y += 180;
                myTrans.eulerAngles = currRotation;
            }

            // always move forward
            Vector2 myVelocity = myBody.velocity;
            myVelocity.x = -myTrans.right.x * speed;
            myBody.velocity = myVelocity;
        }
    }

    /*
    private void walk()
    {
        walkAmount.x = walkingDirection * walkSpeed * Time.deltaTime;
        if (walkingDirection > 0.0f && transform.position.x >= wallRight)
        {
            walkingDirection = -1.0f;
        }
        else if (walkingDirection < 0.0f && transform.position.x <= wallLeft)
        {
            walkingDirection = 1.0f;
        }
        transform.Translate(walkAmount);
    }*/
}
