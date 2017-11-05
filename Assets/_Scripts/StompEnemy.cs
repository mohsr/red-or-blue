using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompEnemy : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("stompped");
        if (collision.gameObject.name == "PlayerFeet")
        {
            if (!transform.root.gameObject.GetComponent<EnemyController>().stomped)
                transform.root.gameObject.GetComponent<EnemyController>().stomped = true;
        }
    }
}
