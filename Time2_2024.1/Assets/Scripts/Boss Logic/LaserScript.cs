using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LaserScript : SpellScript
{
    float damage;
    string targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionTag = collision.gameObject.tag;
        switch (collisionTag)
        {
            case "Enemy":
                if (targetTag == "Enemy")
                {
                    collision.GetComponent<EnemyBase>().TakeDamage(damage);
                    Destroy(gameObject);
                }
                break;

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
