using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Vector2 walkAmount;
    
    public float wallLeft;       // Define wallLeft
    public float wallRight;      // Define wallRight
    float walkingDirection = 1.0f;
    public float walkSpeed = 1.0f;
    public float moveAmount = 2.5f;
    public bool stomped;
    private bool fall;

    public FollowPlayer followplayer;
    private GameObject player;

    private void Start()
    {
        wallLeft = transform.position.x - moveAmount;
        wallRight = transform.position.x + moveAmount;
        followplayer = FindObjectOfType(typeof(FollowPlayer)) as FollowPlayer;
        player = GameObject.FindGameObjectWithTag("Player");
        stomped = false;
    }

    private void Update()
    {
        if (stomped)
        {
            Vector3 newScale = transform.localScale;
            newScale.y /= 2;
            transform.localScale = newScale;
            Vector3 newPosition = transform.position;
            newPosition.y -= newScale.y;
            transform.position = newPosition;
            fall = true;
        }
        else if (fall)
        {
            Vector3 newPosition = transform.position;
            newPosition.y -= 0.05f;
            transform.position = newPosition;
        }
        else
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !stomped)
        {
            //Die
            player.transform.position = new Vector2(player.transform.position.x - 5.0f, 0.0f);
            followplayer.setCameraOnPlayer();
        }
    }
}
