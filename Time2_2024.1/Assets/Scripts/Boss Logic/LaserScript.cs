using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserScript : SpellScript
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        switch (collisionTag)
        {

            case "Player":
                if (targetTag == "Player")
                {
                    collision.GetComponent<PlayerController>().TakeDamage(damage);
                }
                break;

            case "Wall":
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                break;
        }
    }
}
